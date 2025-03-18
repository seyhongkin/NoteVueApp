using Microsoft.AspNetCore.Mvc;
using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Entities;
using NoteVueApp.Server.Exceptions;
using NoteVueApp.Server.Repositories;

namespace NoteVueApp.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public Task<IEnumerable<UserResourceDTO>> GetAllUsers() => _userRepository.GetAllUsers();
        public async Task<object> AddUser(UserDTO userDto)
        {
            // Validation
            bool isExistsByUsername = await _userRepository.ExistsByUsername(userDto.Username);
            bool isExistsByEmail = await _userRepository.ExistsByEmail(userDto.Email);

            if (isExistsByEmail)
            {
                throw new ResourceDuplicateException("This email is already taken.");
            }
            if (isExistsByUsername)
            {
                throw new ResourceDuplicateException("This username is already taken.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            userDto.Password = hashedPassword;

            await _userRepository.AddUser(userDto);

            return new { message = "New user register successfully" };
        }

        public async Task<object> Login(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByCredential(loginDTO);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = _jwtTokenService.GenerateToken(user.Id, user.Username);

            return new { Token = token, User = new UserResourceDTO(user.Username, user.Email) };
        }

    }
}
