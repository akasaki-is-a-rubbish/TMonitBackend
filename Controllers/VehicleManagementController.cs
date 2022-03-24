using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TMonitBackend.Configuration;
using TMonitBackend.Models;
using TMonitBackend.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TMonitBackend.Models{
    [Route("api/vehicles/")]
    [ApiController]
    public class VehicleManagementController : ControllerBase{
        
        [HttpPost("pair")]
        public async Task<IActionResult> PendPairVehicle([FromBody] long id){
            // check if in query 

            return Ok(new
            {
                Success = true,
                // Event = newEvent
            });
        }
        [HttpGet("info")]
        public async Task<IActionResult> VehicleInformation([FromBody] long id){
            //todo
            return Ok(new
            {
                Success = true,
                // Event = newEvent
            });
        }
    }
}