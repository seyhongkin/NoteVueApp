using NoteVueApp.Server.Entities;

namespace NoteVueApp.Server.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task AddUser(User user);
        Task UpdateUser(User product);
        Task DeleteUser(int id);
    }
}
