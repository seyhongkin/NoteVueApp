using Azure.Core;
using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Entities;
using NoteVueApp.Server.Repositories;

namespace NoteVueApp.Server.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public NoteService(INoteRepository noteRepository, IJwtTokenService jwtTokenService)
        {
            _noteRepository = noteRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<object> AddNote(NoteDTO noteDTO, string token)
        {
            DecodedTokenDTO decoded = _jwtTokenService.DecodeToken(token);
            await _noteRepository.AddNote(noteDTO, decoded.UserId);
            return new { message = "New note added" };
        }

        public async Task<Note?> FindNoteById(Guid noteId, string token)
        {
            DecodedTokenDTO decoded = _jwtTokenService.DecodeToken(token);
            var note = await _noteRepository.FindNoteById(noteId, decoded.UserId);
            if (note == null)
            {
                return null;
            }
            return note;
        }

        public async Task<IEnumerable<NoteResourceDTO>> GetAllNotes(string token)
        {
            DecodedTokenDTO dto = _jwtTokenService.DecodeToken(token);
            return await _noteRepository.GetAllNotes(dto.UserId);
        }

        public async Task<bool> DeleteNote(Guid noteId, string token)
        {
            DecodedTokenDTO dto = _jwtTokenService.DecodeToken(token);
            return await _noteRepository.DeleteNote(noteId, dto.UserId);
        }

        public async Task<bool> UpdateNote(Guid noteId, NoteDTO noteDTO, string token)
        {
            DecodedTokenDTO dto = _jwtTokenService.DecodeToken(token);
            return await _noteRepository.UpdateNote(noteId, noteDTO, dto.UserId);
        }

        public async Task<IEnumerable<Note>> GetAllNotesByFilter(string? search, bool? isSortAsc, string token)
        {
            DecodedTokenDTO dto = _jwtTokenService.DecodeToken(token);
            return await _noteRepository.GetAllNotesByFilter(search, isSortAsc, dto.UserId);
        }
    }
}
