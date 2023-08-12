﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using User.Domain.Mapper;
using User.Domain.Service.User.Dto;
using User.Domain.Service.User;
using User.SharedKernel.Utils.Notifications;
using Microsoft.AspNetCore.Authorization;
using User.Domain.Service.User.Entities;
using User.Domain.Common.Validations.Base;

namespace User.Api.Controllers.User
{
    [ApiController]
    [Route("api/user")]
    [Authorize]

    public class UserController : Controller
    {
        private readonly INotification _notification;
        private readonly IUserService _userService;
        IMapper _mapper = AutoMapperProfile.Initialize();
        //tirar notification
        public UserController(INotification notification, IUserService userService)
        {
            _notification = notification;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateUserRequest user)
        {
            var response = await _userService.PostRegister(user);

            if (response.Report.Any())
                return UnprocessableEntity(response.Report);

            return Ok(response);
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

        [HttpPost("auth")]
        [AllowAnonymous]
        public async Task<ActionResult> Auth([FromBody] UserEntity request)
        {
            var response = await _userService.AuthAsync(request);

            if (response.Report.Any())
                return UnprocessableEntity(response.Report);

            return Ok(response);
        }
    }
}
