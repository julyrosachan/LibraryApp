using LibraryApp.Exceptions;
using LibraryApp.Models;

namespace LibraryApp.Services;

public class Library
{
    private readonly List<Book> _books = new();
    private readonly List<User> _users = new();

    public void AddBook(Book book)
    {
        _books.Add(book);
        Console.WriteLine($"Book added: {book.Title}");
    }

    public void RemoveBook(Guid bookId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId)
            ?? throw new BookNotFoundException(bookId);

        if (!book.IsAvailable)
            throw new InvalidOperationException("Cannot remove a book that is currently borrowed!");

        _books.Remove(book);
        Console.WriteLine($"Book removed: {book.Title}");
    }

    public void RegisterUser(User user)
    {
        _users.Add(user);
        Console.WriteLine($"User registered: {user.Name}");
    }

    public void BorrowBook(Guid userId, Guid bookId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId)
            ?? throw new UserNotFoundException(userId);

        var book = _books.FirstOrDefault(b => b.Id == bookId)
            ?? throw new BookNotFoundException(bookId);

        if (!book.IsAvailable)
            throw new InvalidOperationException($"Book is already borrowed: {book.Title}");

        book.IsAvailable = false;
        user.BorrowedBooks.Add(book);

        Console.WriteLine($"{user.Name} borrowed {book.Title}.");
    }

    public void ReturnBook(Guid userId, Guid bookId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId)
            ?? throw new UserNotFoundException(userId);

        var book = user.BorrowedBooks.FirstOrDefault(b => b.Id == bookId)
            ?? throw new InvalidOperationException("This user does not have this book!");

        book.IsAvailable = true;
        user.BorrowedBooks.Remove(book);

        Console.WriteLine($"{user.Name} returned {book.Title}.");
    }

    public List<Book> SearchBooks(string keyword)
    {
        keyword = keyword.ToLower();
        return _books
            .Where(b => b.Title.ToLower().Contains(keyword) ||
                        b.Author.ToLower().Contains(keyword))
            .ToList();
    }

    public void ListAllBooks()
    {
        if (!_books.Any())
        {
            Console.WriteLine("No books in the library.");
            return;
        }

        Console.WriteLine("\nAll books:");
        Console.WriteLine(new string('-', 100));
        foreach (var book in _books)
            Console.WriteLine(book);
        Console.WriteLine(new string('-', 100));
    }

    public void ListBorrowedBooks()
    {
        var borrowed = _books.Where(b => !b.IsAvailable).ToList();

        if (!borrowed.Any())
        {
            Console.WriteLine("No books are currently borrowed.");
            return;
        }

        Console.WriteLine("\nBorrowed books:");
        Console.WriteLine(new string('-', 100));
        foreach (var book in borrowed)
            Console.WriteLine(book);
        Console.WriteLine(new string('-', 100));
    }

    public void ListAllUsers()
    {
        if (!_users.Any())
        {
            Console.WriteLine("No users registered.");
            return;
        }

        Console.WriteLine("\nAll users:");
        Console.WriteLine(new string('-', 100));
        foreach (var user in _users)
            Console.WriteLine(user);
        Console.WriteLine(new string('-', 100));
    }

    public List<User> GetAllUsers()
    {
        return _users.ToList();
    }

    public List<Book> GetAvailableBooks()
    {
        return _books.Where(b => b.IsAvailable).ToList();
    }
}