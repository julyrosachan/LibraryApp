namespace LibraryApp.Models;

public class Book
{
    public Guid Id { get; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Book(string title, string author, int year)
    {
        Id = Guid.NewGuid();
        Title = title;
        Author = author;
        Year = year;
    }

    public override string ToString()
    {
        return $"[{Id.ToString()[..8]}]  {Title,-35} | {Author,-25} | {Year} | {(IsAvailable ? "Available" : "Borrowed")}";
    }
}