using NoteVueApp.Server.DTOs;

namespace NoteVueApp.Server.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(Guid userId, string username);
        string ExtractTokenFromHeader(IHeaderDictionary headers);
        DecodedTokenDTO DecodeToken(string token);
    }
}
