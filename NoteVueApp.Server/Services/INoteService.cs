using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Entities;

namespace NoteVueApp.Server.Services
{
    public interface INoteService
    {
        Task<object> AddNote(NoteDTO noteDTO, string token);
        Task<Note?> FindNoteById(Guid noteId, string token);
        Task<IEnumerable<NoteResourceDTO>> GetAllNotes(string token);
        Task<NoteDTO> UpdateNote(Guid noteId, NoteDTO noteDTO, string token);

        Task<object> DeleteNote(Guid noteId, string token);
    }
}
