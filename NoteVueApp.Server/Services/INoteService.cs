using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Entities;

namespace NoteVueApp.Server.Services
{
    public interface INoteService
    {
        Task<object> AddNote(NoteDTO noteDTO, string token);
        Task<Note?> FindNoteById(Guid noteId, string token);
        Task<IEnumerable<NoteResourceDTO>> GetAllNotes(string token);
        Task<IEnumerable<Note>> GetAllNotesByFilter(string? search, bool? isSortAsc, string token);
        Task<bool> UpdateNote(Guid noteId, NoteDTO noteDTO, string token);

        Task<bool> DeleteNote(Guid noteId, string token);
    }
}
