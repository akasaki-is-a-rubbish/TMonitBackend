using Microsoft.AspNetCore.Mvc;

namespace TMonitBackend.Controllers{
    [Route("tests/")]
    [ApiController]
    public class TestAgentController : Controller{
        public string Index()
        {
            return "Welcome to the test page.";
        }
    }
}