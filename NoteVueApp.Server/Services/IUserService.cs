using Microsoft.AspNetCore.Mvc;
using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Entities;

namespace NoteVueApp.Server.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResourceDTO>> GetAllUsers();
        Task<object> AddUser(UserDTO userDto);

        Task<object> Login(LoginDTO loginDTO);
    }
}
