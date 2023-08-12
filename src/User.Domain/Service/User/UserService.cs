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

        public async Task<Response> Create(CreateUserRequest userRequest)
        {
            try
            {
                //colocar no enum
                //conferir de já está cadastrado

                var isEquals = await _securityService.ComparePassword(userRequest.Password, userRequest.ConfirmPassword);
                if (!isEquals.Data)
                    return Response.Unprocessable(Report.Create("Passwords do not match"));

                var passwordEncripted = await _securityService.EncryptPassword(userRequest.Password);
                userRequest.Password = passwordEncripted.Data;

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

        public async Task<Response<AuthResponse>> AuthAsync(UserLogin auth)
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
