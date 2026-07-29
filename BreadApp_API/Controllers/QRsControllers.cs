
using BreadApp_BL;
using BreadApp_DL;
using BreadApp_Struct.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BreadApp_DL.QRModel;
using static BreadApp_DL.UserModel;

namespace BreadApp_API.Controllers
{
    [Authorize]
    [Route("api/QRs")]
    [ApiController]
    public class QRsControllers : ControllerBase
    {
        [HttpGet("All", Name = "GetAllQRs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<QrInfoDTO>>> GetAllQRs(
            [FromServices] IAuthorizationService authorizationService, int? UserID = null
            , int? BreadPointID = null,int? Status = null, bool? IsScanned = null
            ,int PageNumber = 1, int PageSize = 10)
        {

            var QRsList = QRs.GetAllQRCodes(UserID: UserID, BreadPointID: BreadPointID, Status: Status
                , IsScanned: IsScanned, PageNumber: PageNumber, PageSize: PageSize);

            int? ID = null;
            if (UserID.HasValue)
                ID = UserID;
            else if (BreadPointID.HasValue)
                ID = QRsList.FirstOrDefault(qr => qr.BreadPointID == BreadPointID)?.UserID;
            
            if (ID == null)
                return NotFound("UserID or BreadPointID is Required");

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                ID,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            return Ok(QRsList);
        }


        [HttpGet("By", Name = "GetQr")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<QrInfoDTO>> GetOneQrBy(
            [FromServices] IAuthorizationService authorizationService, int? QRCodeID = null, Guid? PublicID = null,
            string? Token = null)
        {
            if (QRCodeID.HasValue && QRCodeID < 1)
                return BadRequest("QR Code ID Cannot be less than 1");
            if (Token == null || Token.Length < 2)
                return BadRequest("Token Can't be less than 2 characters.");

            byte[] PlainToken = FromBase64Url(Token);

            var qrCode = QRs.GetOneQRCodeBy(QRCodeID: QRCodeID ?? null, PublicID: PublicID ?? null, Token: PlainToken ?? null);

            if (qrCode == null)
                return NotFound("QR Code Not Found");

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                qrCode.UserID,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            return Ok(qrCode);
        }




        private static byte[] FromBase64Url(string input)
        {
            input = input
                .Replace("-", "+")
                .Replace("_", "/");

            switch (input.Length % 4)
            {
                case 2:
                    input += "==";
                    break;
                case 3:
                    input += "=";
                    break;
            }

            return Convert.FromBase64String(input);
        }

        [HttpPut("Scan/{Token}", Name = "Scan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<QrInfoDTO>> Scan(string? Token, int? BreadPointID, [FromServices] IAuthorizationService authorizationService, [FromServices] SessionContextInfo sessionInfo)
        {
            Access.Insert(sessionInfo);
            if (Token == null || Token.Length == 0)
                return BadRequest("Token is Invalid.");
            if (!BreadPointID.HasValue || BreadPointID < 1)
                return BadRequest("Bread Point ID is Invalid");

            byte[] PlainToken = FromBase64Url(Token);

            var qr = QRs.GetOneQRCodeBy(Token: PlainToken);

            if (qr == null)
                return BadRequest("Qr does not Exist.");
            var authResult = await authorizationService.AuthorizeAsync(
                User,
                qr.UserID,
                "UserOwnerOrAdmin");
            if (!authResult.Succeeded)
                return Forbid(); // 403

            var scanned = QRs.Scan(sessionInfo: sessionInfo, Token: PlainToken, BreadPointID: BreadPointID.Value );
            if (scanned == null)
                return BadRequest("Failed to scan QR.");

            return Ok(scanned.ToInfoDTO());
        }
        

        
        [HttpPut("Cancel/{QrCodeID}", Name = "CancelQR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CancelQr(int? QrCodeID, [FromServices] IAuthorizationService authorizationService, [FromServices] SessionContextInfo sessionInfo)
        {
            Access.Insert(sessionInfo);

            if (QrCodeID == null || QrCodeID <= 0)
                return BadRequest("Invalid QR Code ID.");

            var qr = QRs.Find(QRCodeID: QrCodeID.Value);
            if (qr == null)
                return BadRequest("QR Code not found.");

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                qr.UserID,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            if (qr.Cancel(sessionInfo))
            {
                return Ok(new
                {
                    Cancelled = true
                });
            }
            return BadRequest("Failed to cancel QR Code.");

        }



        [HttpPost("", Name = "AddNewQR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<byte[]>> AddNewQR(QrDataDTO cQrDTO , [FromServices] IAuthorizationService authorizationService, [FromServices] SessionContextInfo sessionInfo)
        {
            Access.Insert(sessionInfo);
            if (cQrDTO == null)
                return BadRequest("There is no Data Came");
            if (cQrDTO.UserID < 1)
                return BadRequest("User ID is invalid.");
            if (cQrDTO.BreadPointID < 1)
                return BadRequest("Bread Point ID is invalid.");

            var authResult = await authorizationService.AuthorizeAsync(
            User,
            cQrDTO.UserID,
            "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            QRs Qr = new QRs(cQrDTO);

            if (Qr.Save(sessionInfo))
            {
                return Ok(new
                {
                    Token = Convert.ToBase64String(Qr.Token).Replace("+", "-").Replace("/", "_").TrimEnd('=')
                }
                );

            }
            return BadRequest("Failed to Add Qr.");
        }

       
    }
}