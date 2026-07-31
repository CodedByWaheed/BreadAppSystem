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
