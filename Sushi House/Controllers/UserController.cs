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


        private readonly IAuthService _authenticationService;
        // Конструктор контроллера, внедрение зависимостей через DI
        public UserController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // Получение списка всех пользователей
        [HttpGet]
        public List<User> Get()
        {
            return _authenticationService.Get();
        }

        // Регистрация пользователя
        [HttpPost("Registration")]
        public IActionResult Post(User u)
        {
            try
            {
                _authenticationService.Post(u);
                return Ok(u);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // Хеширование пароля с помощью алгоритма SHA-256


        // Удаление пользователя по ID
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult Delete(int id)
        {
            try
            {
                _authenticationService.Delete(id);
                return Ok("User deleted successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Вход пользователя с выдачей JWT-токена
        [HttpPost("login")]
        public IActionResult Login([FromBody] User model)
        {
            try
            {
            return Ok(_authenticationService.Login(model));
                
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, UserDTO dto)
        {
            try
            {
                _authenticationService.Put(id, dto);
                return Ok("User updated successfully");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}