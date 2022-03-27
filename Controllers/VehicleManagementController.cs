using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TMonitBackend.Configuration;
using TMonitBackend.Models;
using TMonitBackend.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMonitBackend.Services;


namespace TMonitBackend.Models
{
    [Route("api/vehicles/")]
    [ApiController]
    public class VehicleManagementController : ControllerBase
    {
        DatabaseContext _dbctx;
        public VehicleManagementController(DatabaseContext dbctx) => this._dbctx = dbctx;
        
        [HttpPost("qrgenerate")]
        public async Task<IActionResult> GenerateQR([FromBody] string encryptedVehicleId){
            var idDecrypted = InlineCrypto.RSADecrypt(encryptedVehicleId);
            var vehicleExist = _dbctx.Vehicles.Where(x => x.Id == idDecrypted);
            if (vehicleExist == null) throw new Exception("Not a valid vehicle");
            var payload = idDecrypted + "|" + DateTime.UtcNow.AddMinutes(1).ToString("o");
            return Ok(new {
                success = true,
                qrData = InlineCrypto.RSAEncrypt(payload)
            });
        }

        [HttpPost("pair")]
        public async Task<IActionResult> PairVehicle([FromBody] string qrData){
            var userid = long.Parse(User.FindFirstValue("Id") ?? throw new Exception("Not login"));
            var user = _dbctx.Users.Where(x => x.Id == userid).FirstOrDefault();
            var qrDecrypted = InlineCrypto.RSADecrypt(qrData);
            var qrDataParts = qrDecrypted.Split('|');
            var vehicleId = qrDataParts[0];
            var expireTime = DateTime.Parse(qrDataParts[1]);

            if(expireTime<DateTime.UtcNow) return BadRequest(new {success = false, message = "QR expired"});
            var vehicleExist = _dbctx.Vehicles.Where(x => x.Id == vehicleId).FirstOrDefault();
            if (vehicleExist == null) throw new Exception("Not a valid vehicle");
            vehicleExist.user = user;
            if (vehicleExist.userId == userid) return BadRequest(new {success = false, message = "Already paired"});
            await _dbctx.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("info")]
        public async Task<IActionResult> VehicleInformation([FromBody] string? encryptedVehicleId)
        {
            var idDecrypted = InlineCrypto.RSADecrypt(encryptedVehicleId);
            var vehicleExist = await _dbctx.Vehicles.Where(x => x.Id == idDecrypted).FirstOrDefaultAsync();
            if (vehicleExist == null) throw new Exception("Not a valid vehicle");
            var user = await _dbctx.Users.Where(x => x.Id == vehicleExist.userId).FirstOrDefaultAsync();
            return Ok(new
            {
                Success = true,
                info = new{
                    name = vehicleExist.name,
                    model = vehicleExist.model,
                    brand = vehicleExist.brand,
                    mileage = vehicleExist.mileage,
                    userId = vehicleExist.userId,
                    userName = user?.UserName,
                    userAvatar = user?.image,
                }
            });
        }
    }
}