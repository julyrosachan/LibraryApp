namespace LibraryApp.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid userId)
        : base($"User not found! ID: {userId}")
    {
    }

    public UserNotFoundException(string message) : base(message) { }
}