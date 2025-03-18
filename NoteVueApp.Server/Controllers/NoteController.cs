using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteVueApp.Server.DTOs;
using NoteVueApp.Server.Services;

namespace NoteVueApp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;
        private readonly IJwtTokenService _jwtTokenService;
        public NoteController(INoteService noteService, IJwtTokenService jwtTokenService)
        {
            _noteService = noteService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> AddNote([FromBody] NoteDTO noteDTO)
        {
            string token = _jwtTokenService.ExtractTokenFromHeader(Request.Headers);
            var result = await _noteService.AddNote(noteDTO, token);

            return Ok(result);
        }

        [HttpGet("{noteId}")]
        public async Task<IActionResult> FindNoteById([FromRoute] Guid noteId)
        {
            string token = _jwtTokenService.ExtractTokenFromHeader(Request.Headers);
            var note = await _noteService.FindNoteById(noteId, token);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string token = _jwtTokenService.ExtractTokenFromHeader(Request.Headers);
            var users = await _noteService.GetAllNotes(token);
            return Ok(users);
        }

        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid noteId, [FromBody] NoteDTO noteDTO)
        {
            string token = _jwtTokenService.ExtractTokenFromHeader(Request.Headers);
            NoteDTO updated = await _noteService.UpdateNote(noteId, noteDTO, token);

            return Ok(updated);
        }

        [HttpDelete("{noteId}")]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid noteId)
        {
            string token = _jwtTokenService.ExtractTokenFromHeader(Request.Headers);
            var result = await _noteService.DeleteNote(noteId, token);

            return Ok(result);
        }
    }
}
