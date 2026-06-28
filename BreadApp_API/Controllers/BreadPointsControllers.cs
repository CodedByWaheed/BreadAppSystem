using BreadApp_BL;
using BreadApp_DL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BreadApp_DL.BreadPointModel;

namespace BreadApp_API.Controllers
{
    [ApiController]
    [Route("api/BreadPoints")]
    public class BreadPointsControllers : ControllerBase
    {

       


        [HttpGet("GetAll", Name = "GetAllBreadPoints")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<BreadPointDTO>> GetAllBreadPoints(bool? IsActive = true, int PageNumber = 1, int PageSize = 10)
        {

            return Ok(BreadPoints.GetAllBreadPoints(IsActive, PageNumber, PageSize));
        }




        [HttpGet("GetBy", Name = "GetBreadPointBy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<BreadPointDTO> GetBreadPointBy(int? BreadPointID, Guid? PublicID, string? Name, bool? IsActive)
        {
            if (BreadPointID.HasValue && BreadPointID < 1)
                return BadRequest("BreadPoint ID Cannot be less than 1");
            if (!string.IsNullOrEmpty(Name) && Name.Length < 2)
                return BadRequest("Name Can't be less than 2 characters.");

            var breadPoint = BreadPoints.GetBreadPointBy(BreadPointID ?? null, PublicID ?? null, Name ?? null, IsActive ?? null);

            if (breadPoint == null)
                return NotFound("BreadPoint Not Found");

            return Ok(breadPoint);
        }




        [HttpPost("Add", Name = "AddNewBreadPoint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<BreadPointDTO> AddNewBreadPoint(BreadPointDTO breadPointDTO)
        {
            if (breadPointDTO == null)
                return BadRequest("There is no Data Come");
            if (string.IsNullOrEmpty(breadPointDTO.Name))
                return BadRequest("Name Cant be empty");
            if (breadPointDTO.Name.Length < 2)
                return BadRequest("Name Cant be less than 2 char");
            if (string.IsNullOrEmpty(breadPointDTO.Address))
                return BadRequest("Address Cant be empty");
            if (breadPointDTO.AvailablePortions.HasValue && breadPointDTO.AvailablePortions.Value < 0)
                return BadRequest("AvailablePortions must be greater than or equal 0.");
            if (breadPointDTO.WalletBalance.HasValue && breadPointDTO.WalletBalance.Value < 0)
                return BadRequest("WalletBalance must be greater than or equal 0.");
            if (!string.IsNullOrEmpty(breadPointDTO.PhoneNumber) && breadPointDTO.PhoneNumber.Length < 10)
                return BadRequest("Error in Phone, Number must be at least 10 digits.");
            breadPointDTO.WalletBalance= breadPointDTO.WalletBalance ?? 0;

            BreadPoints breadPoint = new BreadPoints(breadPointDTO, BreadPoints.enMode.Add);
            

            if (breadPoint.Save())
            {
                breadPointDTO.BreadPointID = breadPoint.BreadPointID;
                return CreatedAtRoute("GetBreadPointBy", new { BreadPointID = breadPoint.BreadPointID }, breadPointDTO);
            }
            return BadRequest("Failed to Add BreadPoint.");
        }




        [HttpDelete("{BreadPointID}", Name = "DeleteBreadPoint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteBreadPoint(int BreadPointID, bool HardDelete = false)
        {
            if (BreadPointID < 1)
                return BadRequest("BreadPoint ID Can't Be Less than 1");

            BreadPoints? breadPoint = BreadPoints.Find(BreadPointID);
            if (breadPoint == null)
                return NotFound("BreadPoint Not found");

            if (breadPoint.Delete(HardDelete))
                return Ok("BreadPoint Deleted Successfully");

            return BadRequest("Some error Occured.");
        }




        [HttpPut("Update", Name = "UpdateBreadPoint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<BreadPointDTO> UpdateBreadPoint(BreadPointDTO breadPointDTO)
        {
            if (!breadPointDTO.BreadPointID.HasValue)
                return BadRequest("BreadPointID is required.");

            BreadPoints? breadPoint = BreadPoints.Find(breadPointDTO.BreadPointID.Value);
            if (breadPoint == null)
                return NotFound($"BreadPoint with id {breadPointDTO.BreadPointID} not found.");

            breadPoint.AvailablePortions = breadPointDTO.AvailablePortions ?? breadPoint.AvailablePortions;
            breadPoint.WalletBalance = breadPointDTO.WalletBalance ?? breadPoint.WalletBalance;
            breadPoint.Latitude = breadPointDTO.Latitude ?? breadPoint.Latitude;
            breadPoint.Longitude = breadPointDTO.Longitude ?? breadPoint.Longitude;
            breadPoint.IsActive = breadPointDTO.IsActive ?? breadPoint.IsActive;
            breadPoint.Name = string.IsNullOrEmpty(breadPointDTO.Name) ? breadPoint.Name : breadPointDTO.Name;
            breadPoint.Address = string.IsNullOrEmpty(breadPointDTO.Address)? breadPoint.Address: breadPointDTO.Address;
            breadPoint.PhoneNumber = string.IsNullOrEmpty(breadPointDTO.PhoneNumber)? breadPoint.PhoneNumber: breadPointDTO.PhoneNumber;


            if (breadPoint.Save())
            {
                return CreatedAtRoute("GetBreadPointBy", new { BreadPointID = breadPoint.BreadPointID }, breadPoint);
            }
            return BadRequest("Failed to Update BreadPoint.");
        }
    }
}