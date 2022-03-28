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
    [Route("api/user/")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JWTSettings _jwtConfig;
        private DatabaseContext _dbctx;

        public UserManagementController(
                UserManager<User> userManager,
                IOptionsMonitor<JWTSettings> jWTSettings,
                DatabaseContext dbctx)
        {
            _userManager = userManager;
            _jwtConfig = jWTSettings.CurrentValue;
            _dbctx = dbctx;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistration user)
        {
            // 检查传入请求是否有效
            if (ModelState.IsValid)
            {
                // 检查使用相同电子邮箱的用户是否存在
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string>()
                    {
                        "Email already in use"
                    },
                        Success = false
                    });
                }

                var newUser = new User() { Email = user.Email, UserName = user.Username };
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(newUser);

                    return Ok(new
                    {
                        Success = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }
            }

            return BadRequest(new
            {
                Errors = new List<string>()
            {
                "Invalid payload"
            },
                Success = false
            });
        }

        private string GenerateJwtToken(User user)
        {
            //现在，是时候定义 jwt token 了，它将负责创建我们的 tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // 从 appsettings 中获得我们的 secret 
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            // 定义我们的 token descriptor
            // 我们需要使用 claims （token 中的属性）给出关于 token 的信息，它们属于特定的用户，
            // 因此，可以包含用户的 Id、名字、邮箱等。
            // 好消息是，这些信息由我们的服务器和 Identity framework 生成，它们是有效且可信的。
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    // Jti 用于刷新 token，我们将在下一篇中讲到
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                // token 的过期时间需要缩短，并利用 refresh token 来保持用户的登录状态，
                // 不过由于这只是一个演示应用，我们可以对其进行延长以适应我们当前的需求
                Expires = DateTime.UtcNow.AddHours(6),
                // 这里我们添加了加密算法信息，用于加密我们的 token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                // 检查使用相同电子邮箱的用户是否存在
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    // 出于安全原因，我们不想透露太多关于请求失败的信息
                    return BadRequest(new
                    {
                        Errors = new List<string>()
                {
                    "Invalid login request"
                },
                        Success = false
                    });
                }

                // 现在我们需要检查用户是否输入了正确的密码
                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (!isCorrect)
                {
                    // 出于安全原因，我们不想透露太多关于请求失败的信息
                    return BadRequest(new
                    {
                        Errors = new List<string>()
                {
                    "Invalid login request"
                },
                        Success = false
                    });
                }

                var jwtToken = GenerateJwtToken(existingUser);

                return Ok(new
                {
                    Success = true,
                    Token = jwtToken
                });
            }

            return BadRequest(new
            {
                Errors = new List<string>() {
                    "Invalid payload"
                },
                Success = false
            });
        }

        [HttpPut("{id}/updateAvatar")]
        public async Task<IActionResult> UpdateUserAvatar([FromRoute] long id, [FromForm] IFormFile formFile)
        {
            var userid = long.Parse(User.FindFirstValue("Id") ?? throw new Exception("Not login"));
            var user = await _dbctx.Users.Where(x => x.Id == userid)
                //todo
                // .Include(x => x.image)
                .FirstOrDefaultAsync();
            byte[] data;
            using (var ms = new MemoryStream())
            {
                await formFile.CopyToAsync(ms);
                data = ms.ToArray();
            }
            var image = new CommonImage()
            {
                id = Guid.NewGuid().ToString("D"),
                data = data
            };
            if (user.imageId != null)
                _dbctx.Images.Remove(new CommonImage() { id = user.imageId });
            user.image = image;
            await _dbctx.SaveChangesAsync();
            return new JsonResult(new
            {
                success = true,
                url = "/api/images/" + image.id
            });
        }

        [HttpPost("{id}/setEmergencyContract")]
        public async Task<IActionResult> SetEmergencyContract([FromRoute] long id, [FromBody] string emergencyContract)
        {
            var userid = long.Parse(User.FindFirstValue("Id") ?? throw new Exception("Not login"));
            var user = _dbctx.Users.Where(x => x.Id == id).FirstOrDefault();
            user.EmergencyContract = emergencyContract;
            await _dbctx.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("currentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userid = long.Parse(User.FindFirstValue("Id") ?? throw new Exception("Not login"));
            var user = _dbctx.Users.Where(x => x.Id == userid).FirstOrDefault();
            var avatarUrl = user.image == null ? "https://github.githubassets.com/images/modules/logos_page/Octocat.png" : "/api/images/" + User.FindFirstValue("imageId");
            return new JsonResult(new
            {
                id = User.FindFirstValue("id"),
                username = User.FindFirstValue(ClaimTypes.Name),
                email = User.FindFirstValue(ClaimTypes.Email),
                avatar = avatarUrl,
                emergencycontract = user.EmergencyContract,
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