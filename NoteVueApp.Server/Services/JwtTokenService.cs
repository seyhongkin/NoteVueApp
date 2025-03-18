using Microsoft.IdentityModel.Tokens;
using NoteVueApp.Server.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteVueApp.Server.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;
        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Guid userId, string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                ([
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, username),
                ]),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["JwtSettings:ExpirationMinutes"])),
                SigningCredentials = credentials,
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"]
            };

            var handle = new JwtSecurityTokenHandler();

            // Create the token
            var token = handle.CreateToken(tokenDescriptor);

            return handle.WriteToken(token);
        }

        public string ExtractTokenFromHeader(IHeaderDictionary headers)
        {
            if (headers.ContainsKey("Authorization"))
            {
                var authHeader = headers["Authorization"].ToString();

                // Check if the header starts with "Bearer "
                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    // Extract the token (after "Bearer ")
                    var token = authHeader.Substring(7); // Removes "Bearer " part
                    return token;
                }
                else
                {
                    throw new Exception("Authorization header is not in Bearer format.");
                }
            }
            else
            {
                throw new Exception("Authorization header not found.");
            }
        }

        public DecodedTokenDTO DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Parse the token
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Extract claims
            var claims = jwtToken.Claims.ToList();

            // Example of extracting individual claims
            var userIdClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);  // 'sub' claim for userId
            var usernameClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name); // 'name' claim for username

            // Return the decoded token as an object
            if (userIdClaim != null && usernameClaim != null)
            {
                return new DecodedTokenDTO
                {
                    UserId = Guid.Parse(userIdClaim.Value), // Assuming userId is stored as a GUID
                    Username = usernameClaim.Value
                };
            }
            else
            {
                throw new Exception("Required claims not found in token.");
            }
        }
    }

}
