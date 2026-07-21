using BreadApp_BL;
using BreadApp_DL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using static BreadApp_DL.UserModel;

namespace BreadApp_API.Controllers
{
    [Authorize]
    [Route("api/Users")]
    [ApiController]
    public class UsersControllers : ControllerBase
    {

        [HttpGet("GetAll", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserModel.UserInfoDTO>>> GetAllUsers([FromServices] IAuthorizationService authorizationService,int? BreadPointID = null, bool? IsActive = true, int PageNumber = 1, int PageSize = 10 )
        {
            if (BreadPointID.HasValue && BreadPointID < 1)
                return BadRequest("Invalid Bread Point ID.");

            var BreadPoint = BreadPoints.GetBreadPointBy(BreadPointID: BreadPointID);
            if(BreadPoint == null)
                return NotFound("Bread Point not found.");

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                BreadPoint.UserID,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            return Ok(Users.GetAllUsers(BreadPointID ,IsActive ,PageNumber, PageSize ));
        }



        
        [HttpGet("GetBy", Name = "GetUserBy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> GetUserBy(int? UserID, Guid? PublicID, string? NationalNumber, string? Phone, [FromServices] IAuthorizationService authorizationService)
        {
            if (UserID.HasValue && UserID < 1)
                return BadRequest("Invalid User id.");

            if (!string.IsNullOrEmpty(NationalNumber) && NationalNumber.Length < 9)
                return BadRequest("National Number Can't be less than 9 number.");

            if (!string.IsNullOrEmpty(Phone) && (Phone.Length < 10 || Phone.Length > 10))
                return BadRequest("Error in Phone ,Number must be 10 number.");

            Users? user = new Users(Users.GetUserBy(UserID, PublicID, NationalNumber, Phone) , Users.enMode.Update);


            if (user == null)
                return NotFound("User not found.");

            var authResult = await authorizationService.AuthorizeAsync(
                User,
                user.UserID,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403


            return Ok(user.ToInfoDTO());
        }
        


        [AllowAnonymous]
        [HttpPost("Add", Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<UserModel.UserInfoDTO> AddNewUser(UserModel.UserDataDTO userDataDTO)
        {

            

            if (userDataDTO == null)
                return BadRequest("There is no Data Came");

            if (string.IsNullOrEmpty(userDataDTO.NationalNumber))
                return BadRequest("National Number Cant be emapty");
            if (userDataDTO.NationalNumber.Length < 9)
                return BadRequest("National Number Cant be less than 9 char");
            if (string.IsNullOrEmpty(userDataDTO.FirstName) &&string.IsNullOrEmpty(userDataDTO.SecondName)&& string.IsNullOrEmpty(userDataDTO.LastName))
                return BadRequest("First, second and Last Name cant be empty");
            if (string.IsNullOrEmpty(userDataDTO.Phone))
                return BadRequest("Phone cant be empty");
            if (string.IsNullOrEmpty(userDataDTO.Password))
                return BadRequest("Password cant be empty");
            if (userDataDTO.FamilyNumber.HasValue && userDataDTO.FamilyNumber.Value < 0)
                return BadRequest("Family number must be greater than 0.");
           

            userDataDTO.Password = BCrypt.Net.BCrypt.HashPassword(userDataDTO.Password);
            
            Users User = new Users(userDataDTO, Users.enMode.Add);

            if (User.Save())
            {
                return CreatedAtRoute("GetUserBy", new { UserID = User.UserID }, User.ToInfoDTO());
            }
            return BadRequest("Falied to Add User.");
            
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("{UserID}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteUser(int UserID , bool HardDelete = false)
        {
            if (UserID < 0)
                return BadRequest("User ID Can't Be Less than 0");

            Users user = Users.Find(UserID);
            if (user == null) 
                return BadRequest("User Not found");
            
            if (user.DeleteUser(false))
                return Ok("User Deleted Successfully");

           return BadRequest("Some error Occured .");

        }




        [HttpPut("Update", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserInfoDTO>> UpdateUser(UserModel.UserDataDTO UserDTO, [FromServices] IAuthorizationService authorizationService)
        {
            if (UserDTO.NationalNumber == null)
                return BadRequest("National Number Cant be null");

            Users user = new Users(Users.GetUserBy(NationalNumber:UserDTO.NationalNumber)!);

            if(user == null)
                return NotFound($"User not found.");

            var authResult = await authorizationService.AuthorizeAsync(
               User,
               user.UserID,
               "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            
            user.FirstName = string.IsNullOrEmpty(UserDTO.FirstName) ? user.FirstName : UserDTO.FirstName;
            user.SecondName = string.IsNullOrEmpty(UserDTO.SecondName) ? user.SecondName : UserDTO.SecondName;
            user.LastName = string.IsNullOrEmpty(UserDTO.LastName) ? user.LastName : UserDTO.LastName;
            user.DateOfBirth = UserDTO.DateOfBirth.HasValue ? UserDTO.DateOfBirth.Value : user.DateOfBirth;
            user.MaritalStatus = UserDTO.MaritalStatus.HasValue ? UserDTO.MaritalStatus.Value : user.MaritalStatus;
            user.FamilyNumber = UserDTO.FamilyNumber ?? user.FamilyNumber;
            user.Phone = string.IsNullOrEmpty(UserDTO.Phone) ? user.Phone : UserDTO.Phone;
            user.PasswordHash = string.IsNullOrEmpty(UserDTO.Password)? user.PasswordHash : BCrypt.Net.BCrypt.HashPassword(UserDTO.Password); ;
            user.WifeNational = string.IsNullOrEmpty(UserDTO.WifeNational) ? user.WifeNational : UserDTO.WifeNational;
            user.HusbNational = string.IsNullOrEmpty(UserDTO.HusbNational) ? user.HusbNational : UserDTO.HusbNational;


            if (user.Save())
            {
                return CreatedAtRoute("GetUserBy", new { UserID = user.UserID }, user.ToInfoDTO());
            }
            return BadRequest("Falied to Update User.");

        }


     


    }


    
}
