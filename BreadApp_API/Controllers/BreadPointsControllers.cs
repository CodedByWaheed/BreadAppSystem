
using BreadApp_BL;
using BreadApp_DL;
using BreadApp_Struct.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using static BreadApp_DL.BreadPointModel;
using static BreadApp_DL.UserModel;

namespace BreadApp_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/BreadPoints")]
    public class BreadPointsControllers : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("All", Name = "GetAllBreadPoints")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<List<BreadPointInfoDTO>>> GetAllBreadPoints(bool? IsActive = true, int PageNumber = 1, int PageSize = 10)
        {

            return Ok(BreadPoints.GetAllBreadPoints(IsActive, PageNumber, PageSize));
        }


        [AllowAnonymous]
        [HttpGet("By", Name = "GetBreadPointBy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<BreadPointInfoDTO> GetBreadPointBy(int? BreadPointID, Guid? PublicID, string? Name)
        {
            
            if (BreadPointID.HasValue && BreadPointID < 1)
                return BadRequest("BreadPoint ID Cannot be less than 1");
            if (!string.IsNullOrEmpty(Name) && Name.Length < 2)
                return BadRequest("Name Can't be less than 2 characters.");

            var breadPoint = BreadPoints.GetBreadPointBy(BreadPointID: BreadPointID ?? null, PublicID: PublicID ?? null, Name: Name ?? null);

            if (breadPoint == null)
                return NotFound("BreadPoint Not Found");

            return Ok(breadPoint);
        }


        [AllowAnonymous]
        [HttpPost("", Name = "AddNewBreadPoint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<BreadPointInfoDTO> AddNewBreadPoint(BreadPointDataDTO breadPointDTO, [FromServices] SessionContextInfo sessionInfo)
        {
            Access.Insert(sessionInfo);
            if (breadPointDTO == null)
                return BadRequest("There is no Data Come");
            if (string.IsNullOrEmpty(breadPointDTO.Name))
                return BadRequest("Name Cant be empty");
            if (breadPointDTO.Name.Length < 2)
                return BadRequest("Name Cant be less than 2 char");
            if (string.IsNullOrEmpty(breadPointDTO.Address))
                return BadRequest("Address Cant be empty");
            if (breadPointDTO.AvailablePortions < 0)
                return BadRequest("AvailablePortions must be greater than or equal 0.");
           
            if (!string.IsNullOrEmpty(breadPointDTO.PhoneNumber) && breadPointDTO.PhoneNumber.Length < 10)
                return BadRequest("Error in Phone, Number must be at least 10 digits.");

            BreadPoints breadPoint = new BreadPoints(breadPointDTO, BreadPoints.enMode.Add);
            breadPoint.IsActive = false;

            if (breadPoint.Save(sessionInfo))
            {
                var createdDTO = breadPoint.ToInfoDTO();
                return CreatedAtRoute("GetBreadPointBy", new { BreadPointID = breadPoint.BreadPointID }, createdDTO);
            }
            return BadRequest("Failed to Add BreadPoint.");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("Activate", Name = "ActivateBreadPoint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<BreadPointInfoDTO> ActivateBreadPoint(int? BreadPointID, Guid? PublicID, string? Name, [FromServices] SessionContextInfo sessionInfo, bool? Activate = true)
        {
            Access.Insert(sessionInfo);
            if (BreadPointID.HasValue && BreadPointID < 1)
                return BadRequest("BreadPoint ID Cannot be less than 1");
            if (!string.IsNullOrEmpty(Name) && Name.Length < 2)
                return BadRequest("Name Can't be less than 2 characters.");

            BreadPoints? BP = BreadPoints.Find (BreadPointID: BreadPointID ?? null, PublicID: PublicID ?? null, Name: Name ?? null);

            if (BP == null)
                return NotFound("BreadPoint Not Found");

            BP.IsActive = Activate!.Value;

            if (BP.Save(sessionInfo))
            {
                return Ok("Bread Point Activated Successfully");
            }
            return BadRequest("Bread Point Failed to Activate.");
        }




        [Authorize(Roles = "Admin")]
        [HttpDelete("{BreadPointID}", Name = "DeleteBreadPoint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteBreadPoint(int BreadPointID, [FromServices] SessionContextInfo sessionInfo, bool HardDelete = false)
        {
            Access.Insert(sessionInfo);
            if (BreadPointID < 1)
                return BadRequest("BreadPoint ID Can't Be Less than 1");

            BreadPoints? breadPoint = BreadPoints.Find(BreadPointID:BreadPointID);
            if (breadPoint == null)
                return NotFound("BreadPoint Not found");

            if (breadPoint.Delete(sessionInfo, HardDelete))
                return Ok("BreadPoint Deleted Successfully");

            return BadRequest("Some error Occured.");
        }


        [HttpPut("", Name = "UpdateBreadPoint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BreadPointInfoDTO>> UpdateBreadPoint(BreadPointDataDTO breadPointDTO, [FromServices] IAuthorizationService authorizationService, [FromServices] SessionContextInfo sessionInfo)
        {
            Access.Insert(sessionInfo);

            if (breadPointDTO.BreadPointID < 1)
                return BadRequest("BreadPointID is required.");

            BreadPoints? breadPoint = BreadPoints.Find(breadPointDTO.BreadPointID);
            if (breadPoint == null)
                return NotFound($"BreadPoint with id {breadPointDTO.BreadPointID} not found.");

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                breadPoint.UserID,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            
            breadPoint.AvailablePortions = breadPointDTO.AvailablePortions?? breadPoint.AvailablePortions;
            breadPoint.Name = string.IsNullOrEmpty(breadPointDTO.Name) ? breadPoint.Name : breadPointDTO.Name;
            breadPoint.Address = string.IsNullOrEmpty(breadPointDTO.Address) ? breadPoint.Address : breadPointDTO.Address;
            breadPoint.PhoneNumber = string.IsNullOrEmpty(breadPointDTO.PhoneNumber) ? breadPoint.PhoneNumber : breadPointDTO.PhoneNumber;

            if (breadPoint.Save(sessionInfo))
            {
                return CreatedAtRoute("GetBreadPointBy", new { BreadPointID = breadPoint.BreadPointID }, breadPoint.ToInfoDTO());
            }
            return BadRequest("Failed to Update BreadPoint.");
        }



        [HttpPut("Deposit", Name = "DepositBreadPointWallet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> DepositBreadPointWallet(int BreadPointID, decimal Amount, [FromServices] IAuthorizationService authorizationService, [FromServices] SessionContextInfo sessionInfo, string INFO = "SIMULATION")
        {
            Access.Insert(sessionInfo);
            BreadPoints? BreadPoint = BreadPoints.Find(BreadPointID: BreadPointID);

            if (BreadPoint == null)
            {
                return NotFound($"Bread Point not found.");
            }
            var authResult = await authorizationService.AuthorizeAsync(
                User,
                BreadPoint.UserID,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            Transactions transactions = new Transactions
            {
                SenderUserID = BreadPoint.BreadPointID,
                Amount = Amount,
                TransactionType = Transactions.enTransactionType.Withdraw,
                Status = Transactions.enStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            if (transactions.Save(sessionInfo))
            {
                return Ok(new
                {
                    TransactionID = transactions.TransactionID,
                    Status = "Pending"
                });
            }
            return BadRequest("Failed to save transaction.");
        }


    }
}