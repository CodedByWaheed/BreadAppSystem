using BreadApp_BL;
using BreadApp_DL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using static BreadApp_DL.UserModel;

namespace BreadApp_API.Controllers
{
    [Authorize]
    [Route("api/Users")]
    [ApiController]
    public class UsersControllers : ControllerBase
    {

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<UserModel.UserInfoDTO>> GetAllUsers(bool? IsActive = true ,int PageNumber = 1, int PageSize = 10)
        {
            return Ok(Users.GetAllUsers(IsActive ,PageNumber, PageSize));
        }
        
        


        [HttpGet("GetBy", Name = "GetUserBy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDTO>> GetUserBy(int? UserID, Guid? PublicID, string? NationalNumber, string? Phone, bool? IsActive, [FromServices] IAuthorizationService authorizationService)
        {
            if (UserID.HasValue && UserID < 1)
                return BadRequest("Invalid User id.");

            if (!string.IsNullOrEmpty(NationalNumber) && NationalNumber.Length < 9)
                return BadRequest("National Number Can't be less than 9 number.");

            if (!string.IsNullOrEmpty(Phone) && (Phone.Length < 10 || Phone.Length > 10))
                return BadRequest("Error in Phone ,Number must be 10 number.");

            UserDTO? user = null;
            if (UserID.HasValue)
                 user = Users.GetAllUsers().FirstOrDefault(s => s.UserID == UserID);
            else if(PublicID.HasValue)
                user = Users.GetAllUsers().FirstOrDefault(s => s.PublicID == PublicID);
            else if (!string.IsNullOrEmpty(NationalNumber))
                user = Users.GetAllUsers().FirstOrDefault(s => string.Equals( s.NationalNumber , NationalNumber));



            if (user == null)
                return NotFound("User not found.");

            var authResult = await authorizationService.AuthorizeAsync(
                User ,
                user.UserID,
                "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403

            return Ok(user);
        }
        //public ActionResult<IEnumerable<UserModel.UserDTO>> GetUserBy(int? UserID, Guid? PublicID, string? NationalNumber, string? Phone, bool? IsActive)
        //{
        //    if (UserID.HasValue && UserID < 1)
        //        return BadRequest("User ID Cannot be less than 1");
        //    if (!string.IsNullOrEmpty(NationalNumber) && NationalNumber.Length < 9)
        //        return BadRequest("National Number Can't be less than 9 number.");
        //    if (!string.IsNullOrEmpty(Phone) && (Phone.Length < 10 || Phone.Length > 10))
        //        return BadRequest("Error in Phone ,Number must be 10 number.");
        //    return Ok(Users.GetUserBy(UserID ?? null, PublicID ?? null, NationalNumber ?? null, Phone ?? null, IsActive ?? null));
        //}
       
        
        
        
        [HttpPost("Add", Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<UserModel.UserInfoDTO> AddNewUser(UserModel.UserDTO userDTO)
        {

            

            if (userDTO == null)
                return BadRequest("There is no Data Come");
            if (string.IsNullOrEmpty(userDTO.NationalNumber))
                return BadRequest("National Number Cant be emapty");
            if (userDTO.NationalNumber.Length < 9)
                return BadRequest("National Number Cant be less than 9 char");
            if (string.IsNullOrEmpty(userDTO.FirstName) && string.IsNullOrEmpty(userDTO.LastName))
                return BadRequest("First and Last Name cant be empty");
            if (string.IsNullOrEmpty(userDTO.Phone))
                return BadRequest("Phone cant be empty");
            if (string.IsNullOrEmpty(userDTO.PasswordHash))
                return BadRequest("Password cant be empty");
            if (!userDTO.DateOfBirth.HasValue)
                return BadRequest("DateOfBirth cant be empty");
            if (userDTO.FamilyNumber.HasValue && userDTO.FamilyNumber.Value < 0)
                return BadRequest("Family number must be greater than 0.");
            if (string.IsNullOrEmpty(userDTO.WifeHusbNational))
                userDTO.WifeHusbNational = null;

            userDTO.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.PasswordHash);

            Users User = new Users(userDTO, Users.enMode.Add);

            if (User.Save())
            {
                userDTO.UserID = User.UserID;
                return CreatedAtRoute("GetUserBy", new { UserID = User.UserID }, userDTO);
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
            
            if (user.DeleteUser(HardDelete))
                return Ok("User Deleted Successfully");

           return BadRequest("Some error Occured .");

        }




        [HttpPut("Update", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDTO>> UpdateUser(UserModel.UserDTO UserDTO, [FromServices] IAuthorizationService authorizationService)
        {
            Users user = Users.Find(UserDTO.UserID);
            if(user == null)
                return NotFound($"User with id {UserDTO.UserID} not found.");

            var authResult = await authorizationService.AuthorizeAsync(
               User,
               user.UserID,
               "UserOwnerOrAdmin");

            if (!authResult.Succeeded)
                return Forbid(); // 403


            user.FirstName = string.IsNullOrEmpty(UserDTO.FirstName) ? user.FirstName : UserDTO.FirstName;
            user.SecondName = string.IsNullOrEmpty(UserDTO.SecondName) ? user.SecondName : UserDTO.SecondName;
            user.LastName = string.IsNullOrEmpty(UserDTO.LastName) ? user.LastName : UserDTO.LastName;
            user.DateOfBirth = UserDTO.DateOfBirth ?? user.DateOfBirth;
            user.MaritalStatus = UserDTO.MaritalStatus ?? user.MaritalStatus;
            user.FamilyNumber = UserDTO.FamilyNumber ?? user.FamilyNumber;
            user.Phone = string.IsNullOrEmpty(UserDTO.Phone) ? user.Phone : UserDTO.Phone;
            user.PasswordHash = string.IsNullOrEmpty(UserDTO.PasswordHash)? user.PasswordHash : UserDTO.PasswordHash;
            user.WifeHusbNational = string.IsNullOrEmpty(UserDTO.WifeHusbNational) ? user.WifeHusbNational : UserDTO.WifeHusbNational;
            user.IsActive = UserDTO.IsActive ?? user.IsActive;


            if (user.Save())
            {
                return CreatedAtRoute("GetUserBy", new { UserID = user.UserID }, UserDTO);
            }
            return BadRequest("Falied to Update User.");

        }


        //public ActionResult<IEnumerable< UserDTO>> UpdateUser(UserModel.UserDTO UserDTO)
        //{

        //    Users User = Users.Find(UserDTO.UserID);
        //    if(User == null)
        //        return NotFound($"User with id {UserDTO.UserID} not found.");

        //    User.FirstName = string.IsNullOrEmpty(UserDTO.FirstName) ? User.FirstName : UserDTO.FirstName;
        //    User.SecondName = string.IsNullOrEmpty(UserDTO.SecondName) ? User.SecondName : UserDTO.SecondName;
        //    User.LastName = string.IsNullOrEmpty(UserDTO.LastName) ? User.LastName : UserDTO.LastName;
        //    User.DateOfBirth = UserDTO.DateOfBirth ?? User.DateOfBirth;
        //    User.MaritalStatus = UserDTO.MaritalStatus ?? User.MaritalStatus;
        //    User.FamilyNumber = UserDTO.FamilyNumber ?? User.FamilyNumber;
        //    User.Phone = string.IsNullOrEmpty(UserDTO.Phone) ? User.Phone : UserDTO.Phone;
        //    User.PasswordHash = string.IsNullOrEmpty(UserDTO.PasswordHash) ? User.PasswordHash : UserDTO.PasswordHash;
        //    User.WifeHusbNational = string.IsNullOrEmpty(UserDTO.WifeHusbNational) ? User.WifeHusbNational : UserDTO.WifeHusbNational;
        //    User.IsActive = UserDTO.IsActive ?? User.IsActive;


        //    if (User.Save())
        //    {
        //        return CreatedAtRoute("GetUserBy", new { UserID = User.UserID }, UserDTO);
        //    }
        //    return BadRequest("Falied to Update User.");

        //}



    }


    
}
