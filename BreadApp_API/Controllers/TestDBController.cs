using BreadApp_DL;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

[ApiController]
[Route("api/dbtest")]
public class TestDBController : ControllerBase
{
  

    [HttpGet("connection")]
    public IActionResult TestConnection()
    {


        if (clsDBTest.TestCon()) 
            return Ok("DB Connected Successfully 🚀");
        return
            BadRequest("FuckOff");
    }
}