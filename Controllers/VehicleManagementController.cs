using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TMonitBackend.Configuration;
using TMonitBackend.Models;
using TMonitBackend.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace TMonitBackend.Models
{
    [Route("api/vehicles/")]
    [ApiController]
    public class VehicleManagementController : ControllerBase
    {
        DatabaseContext _dbctx;
        public VehicleManagementController(DatabaseContext dbctx) => this._dbctx = dbctx;

        [HttpPost("pair")]
        public async Task<IActionResult> PendPairVehicle([FromBody] long id)
        {
            // check if in query 
            //todo
            return Ok(new
            {
                Success = true,
                // Event = newEvent
            });
        }

        [HttpGet("info")]
        public async Task<IActionResult> VehicleInformation([FromBody] long id)
        {
            var userId = long.Parse(this.User.FindFirstValue("Id") ?? throw new Exception("Not login"));
            //todo
            // var vehicle = await _dbctx.Vehicles.Where(x => x.Id == id).FirstOrDefaultAsync();
            return Ok(new
            {
                Success = true,
                // Event = newEvent
            });
        }
    }
}