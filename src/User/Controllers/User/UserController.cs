using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using User.Domain.Mapper;
using User.Domain.Service.User.Dto;
using User.Domain.Service.User.Entities;
using User.Domain.Service.User;
using User.Domain.Token;
using User.SharedKernel.Utils.Notifications;

namespace User.Api.Controllers.User
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly INotification _notification;
        private readonly IUserService _userService;
        IMapper _mapper = AutoMapperProfile.Initialize();

        public UserController(INotification notification, IUserService userService)
        {
            _notification = notification;
            _userService = userService;
        }

        [HttpPost]
        public async Task<bool> Post(UserDto user)
        {
            var response = await _userService.PostRegister(user);

            if (response == null) return false;
            return true;
            //if (response == null)
            //    return await BadRequest(_notification.GetNotifications());

            //return Ok(response);
        }

        [HttpGet]
        //[Authorize]
        public IActionResult Get()
        {
            var response = _userService.Get();
            if (response == null)
                return BadRequest(_notification.GetNotifications());

            return Ok(response);
        }

        [HttpGet]
        [Route("id")]
        //[Authorize]
        public IActionResult GetById(int id)
        {
            var response = _userService.GetById(id);
            if (response == null)
                return BadRequest(_notification.GetNotifications());

            return Ok(response);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult PostLogin(UserLoginDto model)
        {
            var user = _userService.PostLogin(model);
            if (user == null)
                return BadRequest(_notification.GetNotifications());

            var userEntity = _mapper.Map<UserEntity>(user);
            var token = TokenService.GenerateToken(userEntity);

            return Ok(token);
        }
    }
}
