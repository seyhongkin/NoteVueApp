using Dapper;
using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Entities;
using System.Data;

namespace NoteVueApp.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;
        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<UserResourceDTO>> GetAllUsers()
        {
            string sql = "SELECT Username, Email FROM Users";
            return await _dbConnection.QueryAsync<UserResourceDTO>(sql);
        }

        public async Task AddUser(UserDTO userDTO)
        {
            string sql = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
            await _dbConnection.ExecuteAsync(sql, userDTO);
        }

        public async Task<User?> GetUserByCredential(LoginDTO loginDTO)
        {
            string sql = "SELECT * FROM Users WHERE Username = @Username";

            var user = await _dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { loginDTO.Username });

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
            {
                return null;
            }

            return user;
        }

        public async Task<bool> ExistsByUsername(string username)
        {
            string sql = "SELECT * FROM Users WHERE username = @Username";

            var user = await _dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });

            if (user != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            string sql = "SELECT * FROM Users WHERE email = @Email";

            var user = await _dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });

            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}
