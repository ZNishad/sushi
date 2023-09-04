using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly SushiContext _login;
        public AuthenticationManager(IOptions<JwtSettings> jwtSettings, SushiContext login, IMapper mapper)
        {
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
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

            if (user == null)
            {
                throw new ArgumentException("Invalid email or password.");
            }

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

        public List<User> Get()
        {
            return _login.Users.ToList();
        }

        public void Post(User u)
        {
            if (u == null || string.IsNullOrWhiteSpace(u.UserPassword))
            {
                throw new ArgumentException("Invalid user data");
            }

            // Хеширование пароля перед сохранением в базе данных
            string hashed = u.UserPassword.HashPassword();
            u.UserStatId = 3;
            u.UserPassword = hashed;
            _login.Users.Add(u);
            _login.SaveChanges();
        }

        public void Delete(int id)
        {
            if (id <= 0 || _login.Users.FirstOrDefault(x => x.UserId == id) == null)
            {
                throw new ArgumentException("Invalid User ID");
            }
            _login.Users.Remove(_login.Users.SingleOrDefault(x => x.UserId == id));
            _login.SaveChanges();
        }

        public void Put(int userId, UserDTO dto)
        {
            var oldUser = _login.Users.SingleOrDefault(x => x.UserId == userId);
            if (oldUser == null)
            {
                throw new ArgumentException("User not found");
            }

            _mapper.Map(dto, oldUser);

            try
            {
                _login.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while saving data: {ex.Message}");
            }
        }
    }
}
