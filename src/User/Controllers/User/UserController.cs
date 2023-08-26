using Microsoft.AspNetCore.Mvc;
using User.Domain.Service.User.Dto;
using User.Domain.Service.User;
using Microsoft.AspNetCore.Authorization;

namespace User.Api.Controllers.User
{
    [ApiController]
    [Route("api/user")]
    [Authorize]

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateUserRequest user)
        {
            var response = await _userService.Create(user);

            if (response.Report.Any())
                return UnprocessableEntity(response.Report);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var response = await _userService.Get();

            if (response.Report.Any())
                return UnprocessableEntity(response.Report);

            return Ok(response);
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult> GetById([FromQuery] string id)
        {
            var response = await _userService.GetById(id);

            if (response.Report.Any())
                return UnprocessableEntity(response.Report);

            return Ok(response);
        }

        [HttpPost("auth")]
        [AllowAnonymous]
        public async Task<ActionResult> Auth([FromBody] UserLogin request)
        {
            var response = await _userService.AuthAsync(request);

            if (response.Report.Any())
                return UnprocessableEntity(response.Report);

            return Ok(response);
        }
    }
}
