using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Entities;

namespace NoteVueApp.Server.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserResourceDTO>> GetAllUsers();
        Task AddUser(UserDTO userDto);
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByCredential(LoginDTO loginDTO);
        Task<bool> ExistsByUsername(string username);
        Task<bool> ExistsByEmail(string email);
    }
}
