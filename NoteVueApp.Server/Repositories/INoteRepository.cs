using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Entities;

namespace NoteVueApp.Server.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<NoteResourceDTO>> GetAllNotes(Guid userId);
        Task<IEnumerable<Note>> GetAllNotesByFilter(string? search, bool? isSortAsc, Guid userId);
        Task AddNote(NoteDTO noteDTO, Guid userId);
        Task<Note?> FindNoteById(Guid noteId, Guid userId);
        Task<bool> UpdateNote(Guid noteId, NoteDTO noteDTO, Guid userId);

        Task<bool> DeleteNote(Guid noteId, Guid userId);
    }
}
