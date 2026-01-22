namespace LibraryApp.Models;

public class User
{
    public Guid Id { get; }
    public string Name { get; set; }
    public List<Book> BorrowedBooks { get; } = new();

    public User(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public override string ToString()
    {
        return $"{Name} ({Id.ToString()[..8]}) → {BorrowedBooks.Count} books";
    }
}