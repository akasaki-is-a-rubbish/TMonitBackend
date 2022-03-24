using TMonitBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_ts.Controllers
{
    [Route("api/images/")]
    [ApiController]
    public class ImageController : Controller
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageById([FromRoute] string id, [FromServices] DatabaseContext dbctx)
        {
            var image = await dbctx.Images.Where(x => x.id == id).SingleOrDefaultAsync();
            if (image == null) return NotFound();
            return new FileStreamResult(new MemoryStream(image.data, false), "image/jpeg");
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImageById([FromRoute] string id, [FromServices] DatabaseContext dbctx)
        {
            var image = await dbctx.Images.Where(x => x.id == id).SingleOrDefaultAsync();
            if (image == null) return NotFound();
            dbctx.Images.Remove(image);
            return NoContent();
        }
    }
}