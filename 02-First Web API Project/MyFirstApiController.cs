using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstRESTProject.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/MyFirstApi")]
    [ApiController]
    public class MyFirstApiController : ControllerBase
    {
        [HttpGet("MyName", Name = "MyName")]
        public string GetMyName()
        {
            return "My Name Is Ahmed Abd-Elaal";
        }

        [HttpGet("YourName",Name = "YourName")]
        public string GetYouName()
        {
            return "Your Name Is Mohammed Ali";
        }

        [HttpGet("Sum/{num1}/{num2}")]
        public int Sum2Numbers(int num1 ,int num2)
        {
            return num1 + num2;
        }

        [HttpGet("Multi/{num1}/{num2}")]
        public int Multi2Numbers(int num1, int num2)
        {
            return num1 * num2;
        }
    }
}
