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

    [Route("api/events/")]
    [ApiController]
    public class UserEventController : ControllerBase
    {
        //todo
        [HttpGet()]
        public async Task<IActionResult> GetEvents([FromBody] int startFromIndex = 0)
        {
            return Ok(new
            {
                Success = true,
                // Events = new List<UserEvent>()
                //todo
            });
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewEvent([FromBody] UserBehaviorRec newEvent)
        {
            //todo
            return Ok(new
            {
                Success = true,
                // Event = newEvent
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            //todo
            return Ok(new
            {
                Success = true,
                // Event = newEvent
            });
        }

        [HttpPost("emergency/")]
        public async Task<IActionResult> Emergency([FromBody] string emergencyEvent)
        {
            //todo
            return Ok(new
            {
                Success = true,
                // Events = new List<UserEvent>()
                //todo
            });
        }
    }
}