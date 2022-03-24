using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TMonitBackend.Configuration;
using TMonitBackend.Models;
using TMonitBackend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TMonitBackend.Controllers{
    [ApiController]
    [Route("/api/query/")]
    public class QueryController:ControllerBase{
        ValidationQuery validationQuery;

    }
}