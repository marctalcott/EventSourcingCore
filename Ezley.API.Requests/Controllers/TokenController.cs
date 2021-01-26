using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ES.API.Requests.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ES.API.Requests.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Index([FromBody] LoginModel login)
        {
            var uri = _configuration["Auth0:LoginUri"];
            var loginModel = new Auth0LoginModel(_configuration["Auth0:ClientId"],
                _configuration["Auth0:ClientSecret"],
                _configuration["Auth0:Audience"],
                login.UserName,
                login.Password);

            var json = JsonConvert.SerializeObject(loginModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync(uri, data);
            string respContent = await response.Content.ReadAsStringAsync();

            return Ok(respContent);
        }
    }
}