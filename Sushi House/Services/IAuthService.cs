using Microsoft.AspNetCore.Mvc;
using Sushi_House.Models;

namespace Sushi_House.Services
{
    public interface IAuthService
    {
        string Login(User model);
    }
}
