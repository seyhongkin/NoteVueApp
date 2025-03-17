using NoteVueApp.Server.Entities;
using NoteVueApp.Server.Repositories;

namespace NoteVueApp.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<User>> GetAllUsers() => _userRepository.GetAllUsers();
        public Task<User> GetUserById(int id) => _userRepository.GetUserById(id);
        public Task AddUser(User user) => _userRepository.AddUser(user);
        public Task UpdateUser(User user) => _userRepository.UpdateUser(user);
        public Task DeleteUser(int id) => _userRepository.DeleteUser(id);
    }
}
