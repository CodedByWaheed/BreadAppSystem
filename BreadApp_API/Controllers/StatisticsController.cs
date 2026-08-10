using BreadApp_API.DTOs.Auth;
using BreadApp_BL;
using BreadApp_Struct.Common;
using BreadApp_Struct.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BreadApp_DL.BreadPointModel;

namespace BreadApp_API.Controllers
{
    [ApiController]
    [Route("api/Statistics")]
    public class StatisticsController : Controller
    {

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin", Name = "GetAdminStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<StatisticsModel.AdminStatisticsDTO?> GetAdminStatistics()
        {

            return Ok(Statistics.GetAdminStatistics());
        }

        
        [HttpGet("BreadPoint/{ID}", Name = "GetBreadPointStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<StatisticsModel.BreadPointStatisticsDTO?>> GetBreadPointStatistics(int? ID, [FromServices] IAuthorizationService authorizationService)
        {
            if (ID < 1 || !ID.HasValue)
                return BadRequest("Bread Point ID can't be null or less than 1 ");


            BreadPoints? BreadPoint = BreadPoints.Find(BreadPointID: ID);

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

            return Ok(Statistics.GetBreadPointStatistics(ID.Value));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Logs", Name = "GetLogsStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<StatisticsModel.LogsStatisticsDTO?> GetLogsStatistics()
        {
            return Ok(Statistics.GetLogsStatistics());
        }



    }
}
