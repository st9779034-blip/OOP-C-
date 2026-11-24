using System;
using System.Text;
namespace LibrarySystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Library library = new Library();
            // Початкове наповнення бібліотеки.
            library.AddBook(new Book(1, "Кобзар", "Тарас Шевченко"));
            library.AddBook(new Book(2, "Лісова пісня", "Леся Українка"));
            library.AddBook(new Book(3, "Захар Беркут", "Іван Франко"));
            library.AddUser(new User(1, "Іван"));
            library.AddUser(new User(2, "Олег"));

            library.ShowAllBooks();
            library.ShowUsers();
            library.ShowAvailableBooks();
            // Успішна видача та спроба повторної видачі.
            Console.WriteLine("\nВидача книг:");
            library.RentBook(1, 1);
            library.RentBook(2, 1);
            library.RentBook(1, 99);
            library.RentBook(99, 2);
            library.ShowRentedBooks();
            library.ShowUsers();
            library.ShowAvailableBooks();
            // Помилкове та успішне повернення.
            Console.WriteLine("\nПовернення книг:");
            library.ReturnBook(2, 1);
            library.ReturnBook(1, 1);
            library.ReturnBook(1, 1);
            library.ShowAvailableBooks();
            library.ShowRentedBooks();
            library.ShowUsers();
        }
    }
}
