using BreadApp_API.DTOs.Auth;
using BreadApp_BL;
using BreadApp_Struct.AuthModel;
using BreadApp_Struct.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static BreadApp_DL.BreadPointModel;

namespace BreadApp_API.Controllers
{
    [ApiController]
    [Route("api/Logs")]
    public class LogsController : Controller
    {

        [Authorize(Roles = "Admin")]
        [HttpGet("AllAccess", Name = "GetAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<List<AccessModel.AccessObjDTO>>> GetAccessData(
             DateTime? DateFrom = null, DateTime? DateTo = null  ,int?UserID = null
            ,string? HttpMethod = null, string? EndPoint = null
            ,string? IPAddress = null,Guid? RequestedID = null,int? pageNumber = 1, int? PageSize = 20)
        {
            AccessModel.AccessDTO dto = new AccessModel.AccessDTO()
            {
                UserID = UserID,
                DateFrom = DateFrom ?? DateTime.Today,
                DateTo = DateTo?? DateTime.Today.AddDays(1),
                HttpMethod = HttpMethod, 
                EndPoint = EndPoint,
                IPAddress = IPAddress,
                PageNumber = pageNumber,
                PageRow = PageSize,
                RequestedID = RequestedID
            };

            var data = Access.GetAccessData(dto);
            if (data == null)
                return NotFound("Data Not Found");
            return Ok(data);
        }




        [Authorize(Roles = "Admin")]
        [HttpGet("AllAuth", Name = "GetAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<List<AuthModel.AuthObjDTO>>> GetAuthData(int? UserID = null
            , DateTime? DateFrom = null, DateTime? DateTo = null, bool? Success = null
            , string? IPAddress = null, Guid? RequestedID = null, int? pageNumber = 1, int? PageSize = 20)
        {
            AuthModel.AuthUserDTO dto = new AuthModel.AuthUserDTO()
            {
                UserID = UserID,
                DateFrom = DateFrom ?? DateTime.Today,
                DateTo = DateTo ?? DateTime.Today.AddDays(1),
                IPAddress = IPAddress,
                PageNumber =pageNumber,
                PageRow = PageSize,
                RequestedId = RequestedID,
                Success = Success
            };

            var data = Auth.GetAuthData(dto);
            if (data == null)
                return NotFound("Data Not Found");
            return Ok(data);
        }




        [Authorize(Roles = "Admin")]
        [HttpGet("AllAudit", Name = "GetAudit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<List<AuditModel.AuditObjDTO>>> GetAuditData(int? UserID = null
            , DateTime? DateFrom = null, DateTime? DateTo = null , string? Action = null , int? RecordID = null
            ,string? Role = null, string? IPAddress = null, int? pageNumber = 1, int? PageSize = 20,
            string? TableName = null)
        {
            AuditModel.AuditDTO dto = new AuditModel.AuditDTO()
            {
                UserID = UserID,
                DateFrom = DateFrom ?? DateTime.Today,
                DateTo = DateTo ?? DateTime.Today.AddDays(1),
                PageNumber = pageNumber,
                PageRow = PageSize,
                Action = Action,
                IpAddress = IPAddress,
                RecordID = RecordID,
                Role = Role,
                TableName = TableName
            };

            var data = Audit.GetAuditData(dto);
            if (data == null)
                return NotFound("Data Not Found");
            return Ok(data);
        }

    }
}
