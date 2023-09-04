using Microsoft.AspNetCore.Mvc;
using Sushi_House.Models;

namespace Sushi_House.Services
{
    public interface IAuthService
    {
        string Login(User model);
        List<User> Get();
        void Post(User u);
        void Delete(int id);
        void Put(int id, UserDTO dto);
    }
}
