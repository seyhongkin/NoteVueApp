﻿namespace NoteVueApp.Server.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(Guid userId, string username);
    }
}
