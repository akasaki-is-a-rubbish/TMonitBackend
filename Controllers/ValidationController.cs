using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TMonitBackend.Configuration;
using TMonitBackend.Models;
using TMonitBackend.Services;
using TMonitBackend.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace TMonitBackend.Controllers
{
    [Route("api/security/")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        DatabaseContext _dbctx;
        public ValidationController(DatabaseContext dbctx) => this._dbctx = dbctx;

        [HttpGet("publicrsa")]
        public async Task<IActionResult> getPublicRSAKey()
        {
            var publicKey = InlineCrypto.ExportPublicKey();
            return new JsonResult(new
            {
                success = true,
                key = new JsonResult(new
                {
                    D = publicKey.D,
                    DP = publicKey.DP,
                    DQ = publicKey.DQ,
                    Exponent = publicKey.Exponent,
                    InverseQ = publicKey.InverseQ,
                    Modulus = publicKey.Modulus,
                    P = publicKey.P,
                    Q = publicKey.Q
                })
            });
        }

    }
}