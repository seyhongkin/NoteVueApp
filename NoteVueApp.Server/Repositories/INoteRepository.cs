using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Entities;

namespace NoteVueApp.Server.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<NoteResourceDTO>> GetAllNotes(Guid userId);
        Task AddNote(NoteDTO noteDTO, Guid userId);
        Task<Note?> FindNoteById(Guid noteId, Guid userId);
        Task UpdateNote(Guid noteId, NoteDTO noteDTO, Guid userId);

        Task DeleteNote(Guid noteId, Guid userId);
    }
}
