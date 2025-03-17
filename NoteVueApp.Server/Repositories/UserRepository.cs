using Dapper;
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

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            string sql = "SELECT * FROM Users";
            return await _dbConnection.QueryAsync<User>(sql);
        }

        public async Task<User> GetUserById(int id)
        {
            string sql = "SELECT * FROM Users WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task AddUser(User user)
        {
            string sql = "INSERT INTO Users (Name, Email, Age) VALUES (@Name, @Email, @Age)";
            await _dbConnection.ExecuteAsync(sql, user);
        }

        public async Task UpdateUser(User user)
        {
            string sql = "UPDATE Users SET Name = @Name, Email = @Email, Age = @Age WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, user);
        }

        public async Task DeleteUser(int id)
        {
            string sql = "DELETE FROM Users WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
