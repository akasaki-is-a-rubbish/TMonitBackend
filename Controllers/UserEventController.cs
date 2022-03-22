using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TMonitBackend.Configuration;
using TMonitBackend.Models;
using TMonitBackend.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace TMonitBackend.Controllers
{

    [Route("api/")]
    [ApiController]
    public class UserEventController : ControllerBase
    {
        //todo
        [HttpGet("events/")]
        public async Task<IActionResult> GetEvents([FromQuery] int year, [FromQuery] int month, [FromQuery] int day)
        {
            return Ok(new
            {
                Success = true,
                // Events = new List<UserEvent>()
                //todo
            });
        }
    }
}