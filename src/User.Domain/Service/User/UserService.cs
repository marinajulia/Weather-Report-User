using AutoMapper;
using User.Domain.Service.User.Dto;
using User.Domain.Service.User.Entities;
using User.SharedKernel.Utils.Enums;
using User.SharedKernel.Utils.Notifications;
using User.Domain.Mapper;
using User.Domain.Common.Security;
using User.Domain.Common.Validations.Base;
using User.Domain.Common.ResponseAuth;
using User.Domain.Common.Generators;

namespace User.Domain.Service.User
{
    public class UserService : IUserService
    {
        IMapper _mapper = AutoMapperProfile.Initialize();
        private readonly INotification _notification;
        private readonly IUserRepository _userRepository;
        private readonly ISecurityService _securityService;
        private readonly ITokenManager _tokenManager;
        private readonly IGenerator _generator;

        public UserService(INotification notification, IUserRepository userRepository, ISecurityService securityService, ITokenManager tokenManager, IGenerator generator)
        {
            _notification = notification;
            _userRepository = userRepository;
            _securityService = securityService;
            _tokenManager = tokenManager;
            _generator = generator;
        }

        private async Task<Response<bool>> AutheticationAsync(string password, UserEntity user)
        {
            return await _securityService.VerifyPassword(password, user);
        }

        private async Task<Response<UserEntity>> GetByLoginAsync(string email)
        {
            var response = new Response<UserEntity>();

            var exists = await _userRepository.ExistsByEmailAsync(email);

            if (!exists)
            {
                //colocar enum?
                response.Report.Add(Report.Create($"Email {email} not exists!"));
                return response;
            }

            var data = await _userRepository.GetByEmailAsync(email);
            response.Data = data;
            return response;
        }

        public async Task<Response> PostRegister(CreateUserRequest userRequest)
        {
            try
            {
                //melhorar isso:
                //if (string.IsNullOrEmpty(userRequest.Name))
                //    return await _notification.AddWithReturn<Task<CreateUserRequest>>(ConfigureEnum.GetEnumDescription(UserEnum.FieldNameEmpty));

                //if (string.IsNullOrEmpty(userRequest.Email))
                //    return await _notification.AddWithReturn<Task<CreateUserRequest>>(ConfigureEnum.GetEnumDescription(UserEnum.FieldEmailEmpty));

                //if (string.IsNullOrEmpty(userRequest.Password))
                //    return await _notification.AddWithReturn<Task<CreateUserRequest>>(ConfigureEnum.GetEnumDescription(UserEnum.FieldPasswordEmpty));

                //if (userRequest.IdCity < 0)
                //    return await _notification.AddWithReturn<Task<CreateUserRequest>>(ConfigureEnum.GetEnumDescription(UserEnum.FieldCityEmpty));

                //colocar no enum
                var isEquals = await _securityService.ComparePassword(userRequest.Password, userRequest.ConfirmPassword);
                //ta quebrando quando entra aqui
                //if (!isEquals.Data)
                //    return await _notification.AddWithReturn<Task<UserDto>>("colocar no enum que as senhas não coincidem");

                var passwordEncripted = await _securityService.EncryptPassword(userRequest.Password);

                userRequest.Password = passwordEncripted.Data;
                //conferir de já está cadastrado
                var userEntity = _mapper.Map<UserEntity>(userRequest);

                var response = new Response();
                var validation = new UserValidation();
                var errors = validation.Validate(userEntity).GetErrors();

                if (errors.Report.Count > 0)
                    return errors;
                userEntity.Id = _generator.Generate();
                var userPost = await _userRepository.PostRegister(userEntity);

                return response;
            }
            catch (Exception ex)
            {
                var response = Report.Create(ex.Message);

                //return Response.Unprocessable(response);
                //return await _notification.AddWithReturn<Task<CreateUserRequest>>("deu bom n"); ;
                return Response.Unprocessable(response);
            }

        }

        public IEnumerable<CreateUserRequest> Get()
        {
            var userEntities = _userRepository.Get();
            List<CreateUserRequest> userDtos = new List<CreateUserRequest>();

            if (userEntities != null)
            {
                foreach (var entity in userEntities)
                {
                    var userDto = _mapper.Map<CreateUserRequest>(entity);
                    userDtos.Add(userDto);
                }

                return userDtos;
            }

            return null;
        }

        public CreateUserRequest GetById(int id)
        {
            var usuario = _userRepository.GetById(id);

            if (usuario == null)
                return _notification.AddWithReturn<CreateUserRequest>(ConfigureEnum.GetEnumDescription(UserEnum.CouldNotFind));

            return _mapper.Map<CreateUserRequest>(usuario);

        }

        public CreateUserRequest PostLogin(UserEntity user)
        {
            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Email))
                return _notification.AddWithReturn<CreateUserRequest>(ConfigureEnum.GetEnumDescription(UserEnum.EmptyFields));

            var userData = _userRepository.GetUser(user.Email, user.Password);
            if (userData == null)
                return _notification.AddWithReturn<CreateUserRequest>(ConfigureEnum.GetEnumDescription(UserEnum.IncorrectUsernameOrPassword));

            return _mapper.Map<CreateUserRequest>(userData);
        }

        public bool PutChangeData(CreateUserRequest user)
        {
            throw new NotImplementedException();
        }

        public bool PutChangePassword(CreateUserRequest user)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<AuthResponse>> AuthAsync(UserEntity auth)
        {
            var user = await GetByLoginAsync(auth.Email);

            if (user.Report.Any())
                return Response.Unprocessable<AuthResponse>(user.Report);

            var isAuthenticated = await AutheticationAsync(auth.Password, user.Data);

            if (!isAuthenticated.Data)
                return Response.Unprocessable<AuthResponse>(new List<Report>() { Report.Create("Invalid password or login") });

            var token = await _tokenManager.GenerateTokenAsync(user.Data);

            return new Response<AuthResponse>(token);
        }
       
    }
}
