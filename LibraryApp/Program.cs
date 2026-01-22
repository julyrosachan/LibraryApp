using LibraryApp.Exceptions;
using LibraryApp.Models;
using LibraryApp.Services;

namespace LibraryApp;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        var library = new Library();

        // Sample data
        library.AddBook(new Book("1984", "George Orwell", 1949));
        library.AddBook(new Book("Dune", "Frank Herbert", 1965));
        library.AddBook(new Book("Les Misérables", "Victor Hugo", 1862));

        library.RegisterUser(new User("julyrosachan"));
        library.RegisterUser(new User("salvatoreayan"));

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Library System ===\n");
            Console.WriteLine("1. Add book");
            Console.WriteLine("2. Register user");
            Console.WriteLine("3. Borrow book");
            Console.WriteLine("4. Return book");
            Console.WriteLine("5. List all books");
            Console.WriteLine("6. Search books");
            Console.WriteLine("7. List borrowed books");
            Console.WriteLine("8. List all users");
            Console.WriteLine("0. Exit");
            Console.Write("\nChoice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Please enter a valid number!");
                Console.ReadKey();
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        AddBookMenu(library);
                        break;
                    case 2:
                        RegisterUserMenu(library);
                        break;
                    case 3:
                        BorrowBookMenu(library);
                        break;
                    case 4:
                        ReturnBookMenu(library);
                        break;
                    case 5:
                        library.ListAllBooks();
                        break;
                    case 6:
                        SearchBooksMenu(library);
                        break;
                    case 7:
                        library.ListBorrowedBooks();
                        break;
                    case 8:
                        library.ListAllUsers();
                        break;
                    case 0:
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
            catch (BookNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    static void AddBookMenu(Library library)
    {
        Console.Write("Book title: ");
        string title = Console.ReadLine() ?? "";
        Console.Write("Author: ");
        string author = Console.ReadLine() ?? "";
        Console.Write("Year: ");
        if (int.TryParse(Console.ReadLine(), out int year))
        {
            library.AddBook(new Book(title, author, year));
        }
        else
        {
            Console.WriteLine("Invalid year format!");
        }
    }

    static void RegisterUserMenu(Library library)
    {
        Console.Write("User name: ");
        string name = Console.ReadLine() ?? "";
        if (!string.IsNullOrWhiteSpace(name))
        {
            library.RegisterUser(new User(name));
        }
        else
        {
            Console.WriteLine("Name cannot be empty!");
        }
    }

    static void BorrowBookMenu(Library library)
    {
        var users = library.GetAllUsers();
        if (!users.Any())
        {
            Console.WriteLine("No users registered.");
            return;
        }

        Console.WriteLine("Select user (enter number):");
        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {users[i]}");
        }
        if (!int.TryParse(Console.ReadLine(), out int userIndex) || userIndex < 1 || userIndex > users.Count)
        {
            Console.WriteLine("Invalid selection!");
            return;
        }
        Guid userId = users[userIndex - 1].Id;

        var availableBooks = library.GetAvailableBooks();
        if (!availableBooks.Any())
        {
            Console.WriteLine("No available books.");
            return;
        }

        Console.WriteLine("Select book (enter number):");
        for (int i = 0; i < availableBooks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {availableBooks[i]}");
        }
        if (!int.TryParse(Console.ReadLine(), out int bookIndex) || bookIndex < 1 || bookIndex > availableBooks.Count)
        {
            Console.WriteLine("Invalid selection!");
            return;
        }
        Guid bookId = availableBooks[bookIndex - 1].Id;

        library.BorrowBook(userId, bookId);
    }

    static void ReturnBookMenu(Library library)
    {
        var users = library.GetAllUsers();
        if (!users.Any())
        {
            Console.WriteLine("No users registered.");
            return;
        }

        Console.WriteLine("Select user (enter number):");
        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {users[i]}");
        }
        if (!int.TryParse(Console.ReadLine(), out int userIndex) || userIndex < 1 || userIndex > users.Count)
        {
            Console.WriteLine("Invalid selection!");
            return;
        }
        Guid userId = users[userIndex - 1].Id;

        var borrowedBooks = users[userIndex - 1].BorrowedBooks;
        if (!borrowedBooks.Any())
        {
            Console.WriteLine("This user has no borrowed books.");
            return;
        }

        Console.WriteLine("Select book to return (enter number):");
        for (int i = 0; i < borrowedBooks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {borrowedBooks[i]}");
        }
        if (!int.TryParse(Console.ReadLine(), out int bookIndex) || bookIndex < 1 || bookIndex > borrowedBooks.Count)
        {
            Console.WriteLine("Invalid selection!");
            return;
        }
        Guid bookId = borrowedBooks[bookIndex - 1].Id;

        library.ReturnBook(userId, bookId);
    }

    static void SearchBooksMenu(Library library)
    {
        Console.Write("Search keyword (title or author): ");
        string keyword = Console.ReadLine() ?? "";

        var results = library.SearchBooks(keyword);

        if (!results.Any())
        {
            Console.WriteLine("No results found.");
            return;
        }

        Console.WriteLine($"\nFound books ({results.Count}):");
        Console.WriteLine(new string('-', 100));
        foreach (var book in results)
            Console.WriteLine(book);
        Console.WriteLine(new string('-', 100));
    }
}