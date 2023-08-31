using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sushi_House.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Sushi_House.Extensions;
using Microsoft.AspNetCore.Authentication;
using Sushi_House.Services;

namespace Sushi_House.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IAuthService _authenticationService;
        // Конструктор контроллера, внедрение зависимостей через DI
        public UserController( IMapper mapper, IAuthService authenticationService)
        {
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        // Получение списка всех пользователей
        [HttpGet]
        public List<User> Get()
        {
            return _login.Users.ToList();
        }

        // Регистрация пользователя
        [HttpPost("Registration")]
        public IActionResult Post(User u)
        {
            if (u == null || string.IsNullOrWhiteSpace(u.UserPassword))
            {
                return BadRequest("Invalid user data");
            }

            // Хеширование пароля перед сохранением в базе данных
            string hashed = u.UserPassword.HashPassword();
            u.UserStatId = 3;
            u.UserPassword = hashed;
            _login.Users.Add(u);
            _login.SaveChanges();
            return Ok(u);
        }

        // Хеширование пароля с помощью алгоритма SHA-256
        

        // Удаление пользователя по ID
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserStatusLimit")]
        public void Delete(int id)
        {
            if (id <= 0 || _login.Users.FirstOrDefault(x => x.UserId == id) == null)
            {
                BadRequest("Invalid User ID");
            }
            _login.Users.Remove(_login.Users.SingleOrDefault(x => x.UserId == id));
            _login.SaveChanges();
        }

        // Вход пользователя с выдачей JWT-токена
        [HttpPost("login")]
        public IActionResult Login([FromBody] User model)
        {
            return Ok(_authenticationService.Login(model));

        }

        // Аутентификация пользователя
       

        // Проверка хеша введенного пароля на соответствие хешу в базе данных
        //private bool VerifyPassword(string hashedPassword, string providedPassword)
        //{
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] providedPasswordBytes = Encoding.UTF8.GetBytes(providedPassword);
        //        byte[] hashBytes = sha256.ComputeHash(providedPasswordBytes);

        //        StringBuilder stringBuilder = new StringBuilder();
        //        for (int i = 0; i < hashBytes.Length; i++)
        //        {
        //            stringBuilder.Append(hashBytes[i].ToString("x2"));
        //        }

        //        string hashedProvidedPassword = stringBuilder.ToString();

        //        return hashedPassword == hashedProvidedPassword;
        //    }
        ////}

        // Генерация JWT-токена
      
        [HttpPut("{id}")]
        public IActionResult Put(int id, UserDTO dto)
        {
            var oldUser = _login.Users.SingleOrDefault(x => x.UserId == id);
            if (oldUser == null)
            {
                return NotFound();
            }

            _mapper.Map(dto, oldUser); // Применение маппинга

            try
            {
                _login.SaveChanges();
                return Ok(oldUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving data: {ex.Message}");
            }
        }

    }
}