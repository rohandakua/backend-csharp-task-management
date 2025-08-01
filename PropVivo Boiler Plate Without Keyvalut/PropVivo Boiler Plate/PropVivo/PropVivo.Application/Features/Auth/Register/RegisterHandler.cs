using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Dto.Auth;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.User;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PropVivo.Application.Features.Auth.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, BaseResponse<LoginResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public RegisterHandler(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<BaseResponse<LoginResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<LoginResponse>();

            // Check if username already exists
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new BadRequestException("Username already exists");

            // Check if email already exists
            existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new BadRequestException("Email already exists");

            // Validate superior if provided
            if (!string.IsNullOrEmpty(request.SuperiorId))
            {
                var superior = await _userRepository.GetByIdAsync(request.SuperiorId);
                if (superior == null)
                    throw new BadRequestException("Superior not found");
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password), // TODO: Implement proper password hashing
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role,
                SuperiorId = request.SuperiorId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateAsync(user);

            var token = GenerateJwtToken(createdUser);

            var loginResponse = new LoginResponse
            {
                Token = token,
                UserId = createdUser.Id,
                Username = createdUser.Username,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Role = createdUser.Role,
                SuperiorId = createdUser.SuperiorId,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            response.Data = loginResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.Created;
            response.Message = "User registered successfully";

            return response;
        }

        private string HashPassword(string password)
        {
            // TODO: Implement proper password hashing (e.g., BCrypt)
            // For now, using simple hash (not secure for production)
            return password;
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