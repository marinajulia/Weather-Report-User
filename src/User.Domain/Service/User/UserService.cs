using AutoMapper;
using User.Domain.Service.User.Dto;
using User.Domain.Service.User.Entities;
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
        private readonly IUserRepository _userRepository;
        private readonly ISecurityService _securityService;
        private readonly ITokenManager _tokenManager;
        private readonly IGenerator _generator;

        public UserService(IUserRepository userRepository, ISecurityService securityService, ITokenManager tokenManager, IGenerator generator)
        {
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
                var isEquals = await _securityService.ComparePassword(userRequest.Password, userRequest.ConfirmPassword);
                if (!isEquals.Data)
                    return Response.Unprocessable(Report.Create("Passwords do not match"));

                var exists = await _userRepository.ExistsByEmailAsync(userRequest.Email);
                if (exists)
                    return Response.Unprocessable(Report.Create("User exists"));

                var passwordEncripted = await _securityService.EncryptPassword(userRequest.Password);
                userRequest.Password = passwordEncripted.Data;

                var userEntity = _mapper.Map<UserEntity>(userRequest);

                var response = new Response();
                var validation = new UserValidation();
                var errors = validation.Validate(userEntity).GetErrors();

                if (errors.Report.Count > 0)
                    return errors;

                userEntity.Id = _generator.Generate();
                var userPost = await _userRepository.Register(userEntity);

                return response;
            }
            catch (Exception ex)
            {
                var response = Report.Create(ex.Message);
                return Response.Unprocessable(response);
            }

        }

        public async Task<Response<List<UserDto>>> Get()
        {
            try
            {
                var userEntities = _userRepository.Get();
                var response = new Response<List<UserDto>>();

                if (userEntities == null)
                {
                    response.Report.Add(Report.Create($"Could not find result"));
                    return response;
                }

                var userMap = _mapper.Map<List<UserDto>>(userEntities);
                response.Data = userMap;

                if (response.Report.Any())
                    return Response.Unprocessable<List<UserDto>>(response.Report);

                return Response.OK(userMap);

            }
            catch (Exception ex)
            {
                var responseReport = Report.Create(ex.Message);

                return Response.Unprocessable<List<UserDto>>(new List<Report>() { responseReport });
            }
        }

        public async Task<Response<UserDto>> GetById(string id)
        {
            var response = new Response<UserDto>();
            UserEntity user;

            if (string.IsNullOrEmpty(id))
            {
                response.Report.Add(Report.Create("Empty field"));
                return response;
            }
            try
            {
                user = _userRepository.GetById(id);

                if (user == null)
                {
                    response.Report.Add(Report.Create($"User {id} not exists!"));
                    return response;
                }

                var userMap = _mapper.Map<UserDto>(user);
                response.Data = userMap;

                if (response.Report.Any())
                    return Response.Unprocessable<UserDto>(response.Report);

                return Response.OK(userMap);
            }
            catch (Exception ex)
            {
                var responseReport = Report.Create(ex.Message);

                return Response.Unprocessable<UserDto>(new List<Report>() { responseReport });
            }

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
