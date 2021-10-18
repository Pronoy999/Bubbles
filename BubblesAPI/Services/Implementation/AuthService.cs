using AutoMapper;
using BubblesAPI.Authentication;
using BubblesAPI.Database.Repository;
using BubblesAPI.DTOs;
using Microsoft.Extensions.Configuration;

namespace BubblesAPI.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IBubblesRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IAuthentication _authentication;
        private IMapper _mapper;

        public AuthService(IBubblesRepository repository, IAuthentication authentication, IConfiguration configuration,
            IMapper mapper)
        {
            _repository = repository;
            _authentication = authentication;
            _configuration = configuration;
            _mapper = mapper;
        }

        public LoginResponse Login(LoginRequest request)
        {
            var user = _repository.ValidateCredentials(request);
            var token = _authentication.GetToken(user.UserId, _configuration.GetSection("Jwt").GetValue<string>("Key"),
                _configuration.GetSection("Jwt").GetValue<string>("Issuer"));
            var response = _mapper.Map<LoginResponse>(user);
            response.Token = token;
            return response;
        }
    }
}