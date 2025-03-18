using System.Data;
using Dapper;
using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Entities;

namespace NoteVueApp.Server.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly IDbConnection _dbConnection;
        public NoteRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddNote(NoteDTO noteDTO, Guid userId)
        {
            string sql = "INSERT INTO Notes (Title, Content, CreatedBy) VALUES (@Title, @Content, @CreatedBy)";

            var parameters = new
            {
                noteDTO.Title,
                noteDTO.Content,
                CreatedBy = userId
            };

            await _dbConnection.ExecuteAsync(sql, parameters);
        }

        public async Task DeleteNote(Guid noteId, Guid userId)
        {
            string sql = "DELETE FROM Notes WHERE Id = @NoteId AND CreatedBy = @CreatedBy";
            var parameters = new
            {
                NoteId = noteId,
                CreatedBy = userId
            };
            await _dbConnection.ExecuteAsync(sql, parameters);
        }

        public async Task<Note?> FindNoteById(Guid noteId, Guid userId)
        {
            string sql = "SELECT TOP 1 * FROM Notes WHERE Id = @NoteId AND CreatedBy = @CreatedBy";
            var note = await _dbConnection.QuerySingleOrDefaultAsync<Note>(sql, new { NoteId = noteId, CreatedBy = userId });
            if (note == null) 
            {
                return null;
            }

            return note;
        }

        public async Task<IEnumerable<NoteResourceDTO>> GetAllNotes(Guid userId)
        {
            string sql = "SELECT * FROM Notes WHERE CreatedBy = @CreatedBy";
            return await _dbConnection.QueryAsync<NoteResourceDTO>(sql, new { CreatedBy = userId });
        }

        public async Task UpdateNote(Guid noteId, NoteDTO noteDTO, Guid userId)
        {
            string sql = "UPDATE Notes Set Title = @Title, Content = @Content, UpdatedAt = GETDATE() WHERE Id = @NoteId AND CreatedBy = @CreatedBy";
            var parameters = new
            {
                noteDTO.Title,
                noteDTO.Content,
                NoteId = noteId,
                CreatedBy = userId
            };
            await _dbConnection.ExecuteAsync(sql, parameters);
        }
    }
}
