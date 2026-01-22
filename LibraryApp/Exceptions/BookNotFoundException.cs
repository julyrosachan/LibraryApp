namespace LibraryApp.Exceptions;

public class BookNotFoundException : Exception
{
    public BookNotFoundException(Guid bookId)
        : base($"Book not found! ID: {bookId}")
    {
    }

    public BookNotFoundException(string message) : base(message) { }
}