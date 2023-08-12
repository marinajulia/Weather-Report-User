﻿using AutoMapper;
using User.Domain.Service.User.Dto;
using User.Domain.Service.User.Entities;
using User.SharedKernel.Utils.Enums;
using User.SharedKernel.Utils.Notifications;
using User.Domain.Mapper;
using User.Domain.Common.Security;

namespace User.Domain.Service.User
{
    public class UserService : IUserService
    {
        IMapper _mapper = AutoMapperProfile.Initialize();
        private readonly INotification _notification;
        private readonly IUserRepository _userRepository;
        private readonly ISecurityService _securityService;

        public UserService(INotification notification, IUserRepository userRepository, ISecurityService securityService)
        {
            _notification = notification;
            _userRepository = userRepository;
            _securityService = securityService;
        }

        public async Task<UserDto> PostRegister(UserDto userDto)
        {
            //melhorar isso:
            if (string.IsNullOrEmpty(userDto.Name))
                return await _notification.AddWithReturn<Task<UserDto>>(ConfigureEnum.GetEnumDescription(UserEnum.FieldNameEmpty));

            if (string.IsNullOrEmpty(userDto.Email))
                return await _notification.AddWithReturn<Task<UserDto>>(ConfigureEnum.GetEnumDescription(UserEnum.FieldEmailEmpty));

            if (string.IsNullOrEmpty(userDto.Password))
                return await _notification.AddWithReturn<Task<UserDto>>(ConfigureEnum.GetEnumDescription(UserEnum.FieldPasswordEmpty));

            if (userDto.IdCity < 0)
                return await _notification.AddWithReturn<Task<UserDto>>(ConfigureEnum.GetEnumDescription(UserEnum.FieldCityEmpty));

            //colocar no enum
            var isEquals = await _securityService.ComparePassword(userDto.Password, userDto.ConfirmPassword);
            //ta quebrando quando entra aqui
            if (!isEquals.Data)
                return await _notification.AddWithReturn<Task<UserDto>>("colocar no enum que as senhas não coincidem");

            var passwordEncripted = await _securityService.EncryptPassword(userDto.Password);

            userDto.Password = passwordEncripted.Data;
            //conferir de já está cadastrado

            var userEntity = _mapper.Map<UserEntity>(userDto);

            var userPost = await _userRepository.PostRegister(userEntity);

            var userDtoModel = _mapper.Map<UserDto>(userPost);

            return userDtoModel;
        }

        public IEnumerable<UserDto> Get()
        {
            var userEntities = _userRepository.Get();
            List<UserDto> userDtos = new List<UserDto>();

            if (userEntities != null)
            {
                foreach (var entity in userEntities)
                {
                    var userDto = _mapper.Map<UserDto>(entity);
                    userDtos.Add(userDto);
                }

                return userDtos;
            }

            return null;
        }

        public UserDto GetById(int id)
        {
            var usuario = _userRepository.GetById(id);

            if (usuario == null)
                return _notification.AddWithReturn<UserDto>(ConfigureEnum.GetEnumDescription(UserEnum.CouldNotFind));

            return _mapper.Map<UserDto>(usuario);

        }

        public UserDto PostLogin(UserLoginDto user)
        {
            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email))
                return _notification.AddWithReturn<UserDto>(ConfigureEnum.GetEnumDescription(UserEnum.EmptyFields));

            var userData = _userRepository.GetUser(user.Email, user.Password);
            if (userData == null)
                return _notification.AddWithReturn<UserDto>(ConfigureEnum.GetEnumDescription(UserEnum.IncorrectUsernameOrPassword));

            return _mapper.Map<UserDto>(userData);
        }

        public bool PutChangeData(UserDto user)
        {
            throw new NotImplementedException();
        }

        public bool PutChangePassword(UserDto user)
        {
            throw new NotImplementedException();
        }
    }
}
