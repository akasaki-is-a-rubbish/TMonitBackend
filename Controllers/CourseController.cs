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
using Microsoft.EntityFrameworkCore;
using TMonitBackend.Services;
using Microsoft.AspNetCore.Mvc;
using TMonitBackend.Models;
using TMonitBackend.Services;

namespace TMonitBackend.Controllers
{
    [Route("api/course/")]
    [ApiController]
    public class CourseController : Controller
    {
        DatabaseContext _dbctx;
        public CourseController(DatabaseContext dbctx) => this._dbctx = dbctx;
        [HttpGet("info/{id}")]
        public async Task<IActionResult> CourseInformation([FromRoute] string? id)
        {
            var course = await _dbctx.Courses.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (course == null) throw new Exception("Not a valid course");
            var userid = long.Parse(User.FindFirstValue("Id") ?? throw new Exception("Not login"));
            var imageUrl = course.image == null ? "https://github.githubassets.com/images/modules/logos_page/Octocat.png" : "/api/images/" + User.FindFirstValue("imageId");

            return Ok(new
            {
                Success = true,
                info = new
                {
                    name = course.name,
                    description = course.description,
                    image = imageUrl
                }
            });
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllCourses()
        {
            var userid = long.Parse(User.FindFirstValue("Id") ?? throw new Exception("Not login"));
            var user = _dbctx.Users.Where(x => x.Id == userid).FirstOrDefault();
            var courses = _dbctx.Courses.AsEnumerable();
            return new JsonResult(new
            {
                vehicles = courses.Select(x => new CourseInfo
                {
                    Id = x.Id,
                    name = x.name,
                    description = x.description,
                })
            });
        }

        [HttpPost("join/{id}")]
        public async Task<IActionResult> JoinCourse([FromRoute] string id)
        {
            var userid = long.Parse(User.FindFirstValue("Id") ?? throw new Exception("Not login"));
            var user = _dbctx.Users.Where(x => x.Id == userid).FirstOrDefault();
            var course = _dbctx.Courses.Where(x => x.Id == id).FirstOrDefault();
            if (course == null)
                return BadRequest("Course id not found");
            course.partationers.Add(user);
            user.courses.Add(course);
            await _dbctx.SaveChangesAsync();
            return Ok();
        }
    }
}