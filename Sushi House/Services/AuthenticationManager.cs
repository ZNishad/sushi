using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sushi_House.Extensions;
using Sushi_House.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sushi_House.Services
{
    public class AuthenticationManager : IAuthService
    {

        private readonly JwtSettings _jwtSettings;
        private readonly SushiContext _login;
        public AuthenticationManager(IOptions<JwtSettings> jwtSettings, SushiContext login)
        {
            _jwtSettings = jwtSettings.Value;
            _login = login;
        }
        private User AuthenticateUser(string email, string password)
        {

            var user = _login.Users.FirstOrDefault(u => u.UserMail == email);
            if (user != null && (password.HashPassword() == user.UserPassword))
            {
                return user;
            }
            return null;
        }
        public string Login(User model)
        {
            var user = AuthenticateUser(model.UserMail, model.UserPassword);

            //if (user == null)
            //{
            //    return UnauthorizedAccessException("Invalid email or password.");
            //}

            var token = GenerateJwtToken(user.UserId, user.UserName, (int)user.UserStatId);
            return token;
        }
        private string GenerateJwtToken(int UserId, string UserName, int UserStatId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim(ClaimTypes.Sid, UserId.ToString()),
                    new Claim("UserStatId", UserStatId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
