namespace NoteVueApp.Server.DTOs
{
    public class UserResourceDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public UserResourceDTO(string username, string email) {
            this.Username = username;
            this.Email = email;
        }
    }
}
