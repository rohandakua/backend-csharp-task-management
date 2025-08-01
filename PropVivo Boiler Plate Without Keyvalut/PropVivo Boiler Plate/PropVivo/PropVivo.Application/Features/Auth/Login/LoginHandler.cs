using MediatR;
using Microsoft.Extensions.Configuration;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Dto.Auth;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.User;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace PropVivo.Application.Features.Auth.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, BaseResponse<LoginResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public LoginHandler(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<BaseResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<LoginResponse>();

            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
                throw new BadRequestException("Invalid username or password");

            if (!VerifyPassword(request.Password, user.PasswordHash))
                throw new BadRequestException("Invalid username or password");

            if (!user.IsActive)
                throw new BadRequestException("User account is deactivated");

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            var token = GenerateJwtToken(user);

            var loginResponse = new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                SuperiorId = user.SuperiorId,
                ExpiresAt = DateTime.UtcNow.AddHours(24) // Token expires in 24 hours
            };

            response.Data = loginResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Login successful";

            return response;
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            // TODO: Implement proper password verification
            // For now, using simple comparison (not secure for production)
            return password == passwordHash;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "default-key"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 