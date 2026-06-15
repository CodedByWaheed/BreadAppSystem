using BreadApp_BL;
using BreadApp_DL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BreadApp_DL.BreadPointModel;

namespace BreadApp_API.Controllers
{
    [Route("api/QRs")]
    [ApiController]
    public class QRsControllers : ControllerBase
    {


        [HttpGet("GetAll", Name = "GetAllQRs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<QRCodeModel.QRCodeDTO>> GetAllQRs(int? UserID = null, int? BreadPointID = null,
            string? Status = null, bool? IsScanned = null,
            int PageNumber = 1, int PageSize = 10)
        {
            return Ok(QRs.GetAllQRCodes(UserID:UserID, BreadPointID:BreadPointID, Status:Status, IsScanned:IsScanned, PageNumber:PageNumber, PageSize:PageSize));
        }





        [HttpGet("Scan/{Token}", Name = "Scan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<QRCodeModel.QRScanResultDTO> Scan(string?Token  , int? BreadPointID)
        {
            if (string.IsNullOrWhiteSpace(Token))
                return BadRequest("Token is Invalied.");
            if (BreadPointID.HasValue && BreadPointID < 1)
                return BadRequest("Bread Point ID is Invalied");
            return Ok(QRs.Scan(Token: Token!, BreadPointID: BreadPointID!.Value));
        }




        [HttpGet("GetOneQRBy", Name = "GetQr")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<QRCodeModel.QRCodeDTO> GetOneQrBy(int? QRCodeID = null, Guid? PublicID = null,
            string? Token = null)
        {
            if (QRCodeID.HasValue && QRCodeID < 1)
                return BadRequest("QR Code ID Cannot be less than 1");
            if (!string.IsNullOrEmpty(Token) && Token.Length < 2)
                return BadRequest("Token Can't be less than 2 characters.");

            var qrCode = QRs.GetOneQRCodeBy(QRCodeID: QRCodeID ?? null, PublicID:PublicID ?? null, Token:Token ?? null);

            if (qrCode == null)
                return NotFound("QR Code Not Found");

            return Ok(qrCode);
        }




        [HttpPost("Add", Name = "AddNewQR")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<QRCodeModel.QRCodeDTO> AddNewBreadPoint(QRCodeModel.CreateQRDTO cQrDTO)
        {
            if (cQrDTO == null)
                return BadRequest("There is no Data Came");
            if (cQrDTO.UserID.HasValue && cQrDTO.UserID.Value < 1)
                return BadRequest("User ID is invalid.");
            if (cQrDTO.BreadPointID.HasValue && cQrDTO.BreadPointID.Value < 1)
                return BadRequest("Bread Point ID is invalid.");
           
            QRs Qr = new QRs(cQrDTO);


            if (Qr.Save())
            {
               
                return CreatedAtRoute("GetQr", new { QRCodeID = Qr.QRCodeID }, Qr);
            }
            return BadRequest("Failed to Add Qr.");
        }




        [HttpDelete("Delete/{QrID}", Name = "DeleteQr")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteQr(int QrID, bool HardDelete = false)
        {
            if (QrID < 1)
                return BadRequest("Qr ID Can't Be Less than 1");

            QRs? Qr = QRs.Find(QrID);
            if (Qr == null)
                return NotFound("Qr Not found");

            if (Qr.Delete(HardDelete))
                return Ok("Qr Deleted Successfully");

            return BadRequest("Some error Occured.");
        }




        [HttpPut("Update", Name = "UpdateQr")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<QRCodeModel.QRCodeDTO> UpdateQr(QRCodeModel.QRCodeDTO QrDTO)
        {
            if (!QrDTO.QRCodeID.HasValue)
                return BadRequest("Qr ID is required.");

            QRs? qr = QRs.Find(QrDTO.QRCodeID.Value);
            if (qr == null)
                return NotFound($"Qr with id {QrDTO.QRCodeID} not found.");

            qr.BreadPointID = QrDTO.BreadPointID ?? qr.BreadPointID;
            qr.PortionCount = QrDTO.PortionCount ?? qr.PortionCount;
            qr.Status = QrDTO.Status.HasValue? (QRs.enStatus)QrDTO.Status.Value: qr.Status;
            qr.ExpiresAt = QrDTO.ExpiresAt ?? qr.ExpiresAt;
          
            if (qr.Save())
            {
                return CreatedAtRoute("GetQr", new { QRCodeID = qr.QRCodeID }, qr);
            }
            return BadRequest("Failed to Update Qr.");
        }







    }
}
