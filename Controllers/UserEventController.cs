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

namespace TMonitBackend.Controllers
{

    [Route("api/events/")]
    [ApiController]
    public class UserEventController : ControllerBase
    {
        DatabaseContext _dbctx;
        public UserEventController(DatabaseContext dbctx) => this._dbctx = dbctx;

        [HttpGet]
        public async Task<IActionResult> ListEvents(/* [FromBody] int startFromIndex = 0 */)//todo
        {
            var userId = long.Parse(this.User.FindFirstValue("Id") ?? throw new Exception("Not login"));
            var todos = _dbctx.UserBehaviors.Where(x => x.userId == userId);
            return new JsonResult(
                todos.Select(x => new UserBehaviorRec
                {
                    id = x.Id,
                    description = x.description,
                    dateTime = x.dateTime,
                    image = x.imageId == null ? null : ("/api/images/" + x.imageId)
                })
            );
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewEvent([FromBody] UserBehaviorRec newEvent)
        {
            var userId = long.Parse(User.FindFirstValue("Id") ?? throw new Exception("Not login"));
            _dbctx.UserBehaviors.Add(new UserBehavior
            {
                userId = userId,
                dateTime = newEvent.dateTime==null?DateTime.Now:newEvent.dateTime,
                description = newEvent.description,
                dangerousLevel = newEvent.dangerousLevel
            });
            await _dbctx.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] string id)
        {
            var userid = long.Parse(User.FindFirstValue("Id") ?? throw new Exception("Not login"));
            var behaviorRec = await _dbctx.UserBehaviors.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (behaviorRec == null || behaviorRec.userId != userid) throw new Exception(); // todo make errorcode table
            _dbctx.UserBehaviors.Remove(behaviorRec);
            if (behaviorRec.imageId != null)
            {
                _dbctx.Attach(new CommonImage() {id = behaviorRec.imageId}).State = EntityState.Deleted;
            }
            await _dbctx.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}/image")]
        public async Task<IActionResult> PutTodoImage([FromRoute] string id)
        {
            var behaviorRec = await _dbctx.UserBehaviors
                .Where(x => x.Id == id)
                //todo
                // .Include(x => x.image)
                .FirstOrDefaultAsync();
            if (behaviorRec == null) return NotFound();
            byte[] data = await ReadRequestBodyAsBytes();
            var image = new CommonImage()  
            {
                id = Guid.NewGuid().ToString("D"),
                data = data
            };
            if (behaviorRec.imageId != null)
            {
                _dbctx.Images.Remove(new CommonImage() {id = behaviorRec.imageId});
                // _dbctx.Images.Remove(todo.image);
            }
            behaviorRec.image = image;
            await _dbctx.SaveChangesAsync();
            return new JsonResult(new
            {
                url = "/api/events/images/" + image.id
            });
        }

        [HttpDelete("{id}/image")]
        public async Task<IActionResult> DeleteTodoImage([FromRoute] string id)
        {
            var behaviorRec = await _dbctx.UserBehaviors.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (behaviorRec == null) return NotFound();
            if (behaviorRec.imageId != null)
            {
                _dbctx.Attach(new CommonImage() {id = behaviorRec.imageId}).State = EntityState.Deleted;
            }
            behaviorRec.imageId = null;
            await _dbctx.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("emergency/")]
        public async Task<IActionResult> Emergency([FromBody] string emergencyEvent)
        {
            //todo
            return Ok(new
            {
                Success = false,
                Text = "Method still under construction"
                //todo
            });
        }

        private async Task<byte[]> ReadRequestBodyAsBytes()
        {
            var fileStream = Request.Body;
            byte[] data;
            using (var ms = new MemoryStream())
            {
                await fileStream.CopyToAsync(ms);
                data = ms.ToArray();
            }

            return data;
        }
    }
}