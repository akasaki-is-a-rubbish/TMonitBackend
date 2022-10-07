using Microsoft.AspNetCore.Mvc;
using TMonitBackend.Models;
using TMonitBackend.Services;
using TMonitBackend.Models.DTO;

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
                userId = forWhichUserId == -1 ? null : forWhichUserId,
                brand = "Testers",
                model = "For Tests Only"
            });
            await _dbctx.SaveChangesAsync();
            return Ok(new
            {
                Id = newVehicleId
            });
        }
        [HttpPost("course/new")]
        public async Task<IActionResult> CreateNewCourse([FromBody] CourseInfo course)
        {
            var newCourseId = Guid.NewGuid().ToString();
            _dbctx.Courses.Add(new CourseModel
            {
                Id = newCourseId,
                description = course.description,
                name = course.name
            });
            await _dbctx.SaveChangesAsync();
            return Ok(new
            {
                Id = newCourseId
            });
        }
    }
}