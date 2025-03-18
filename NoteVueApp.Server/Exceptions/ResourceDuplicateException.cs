namespace NoteVueApp.Server.Exceptions
{
    public class ResourceDuplicateException: Exception
    {
        public ResourceDuplicateException(string message) : base(message) { }
    }
}
