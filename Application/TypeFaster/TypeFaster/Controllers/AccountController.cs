using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TypeFaster.Core.Interfaces;
using TypeFaster.Hubs;

namespace TypeFaster.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IUserService _userService;

        public AccountController(IHubContext<ChatHub>  hubContext, IUserService userService)
        {
            _hubContext = hubContext;
            _userService = userService;
        }

        [HttpGet("Login")]
        public IActionResult Login(string username)
        {
            string userId = null;
            Request.Cookies.TryGetValue("userid", out userId);
            
            var user = _userService.GetOrCreateUser(username);


            var cookie = new CookieOptions()
            {
                SameSite = SameSiteMode.None,
                Domain = "localhost"
                
            };
            
            
            Response.Cookies.Append("userid", user.Id.ToString(),cookie);
            return Ok();
        }
    }
}