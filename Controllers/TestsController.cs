using Microsoft.AspNetCore.Mvc;
using TMonitBackend.Models;
using TMonitBackend.Services;

namespace TMonitBackend.Controllers
{
    [Route("tests/")]
    [ApiController]
    public class TestAgentController : Controller
    {
        DatabaseContext _dbctx;

        public TestAgentController(DatabaseContext dbctx) => this._dbctx = dbctx;

        [HttpGet("hello")]
        public string Index()
        {
            return "Welcome to the test page.";
        }

        [HttpPost("rsa/encrypt")]
        public async Task<IActionResult> Encrypt([FromBody] string data)
        {
            var encrypted = InlineCrypto.RSAEncrypt(data);
            return Ok(new
            {
                success = true,
                data = encrypted
            });
        }

        [HttpPost("rsa/decrypt")]
        public async Task<IActionResult> Decrypt([FromBody] string data)
        {
            var decrypted = InlineCrypto.RSADecrypt(data);
            return Ok(new
            {
                success = true,
                data = decrypted
            });
        }
        
        [HttpPost("vehicle/register")]
        public async Task<IActionResult> RegisterNewVehicle([FromBody] long? forWhichUserId)
        {
            var newVehicleId = Guid.NewGuid().ToString();
            _dbctx.Vehicles.Add(new Vehicle
            {
                Id = newVehicleId,
                userId = forWhichUserId,
                brand = "Testers",
                model = "For Tests Only"
            });
            await _dbctx.SaveChangesAsync();
            return Ok(new
            {
                Id = newVehicleId
            });
        }
    }
}