using System;
using System.Collections.Generic;
namespace LibrarySystem
{
    public class Library
    {
        private readonly List<Book> books = new List<Book>();
        private readonly List<User> users = new List<User>();

        public void AddBook(Book book)
        {
            if (books.Exists(b => b.Id == book.Id))
                Console.WriteLine("Помилка: ID книги вже існує.");
            else books.Add(book);
        }

        public void AddUser(User user)
        {
            if (users.Exists(u => u.Id == user.Id))
                Console.WriteLine("Помилка: ID користувача вже існує.");
            else users.Add(user);
        }

        public void RentBook(int userId, int bookId)
        {
            User user = users.Find(u => u.Id == userId);
            Book book = books.Find(b => b.Id == bookId);
            if (user == null) Console.WriteLine("Помилка: користувача не знайдено.");
            else if (book == null) Console.WriteLine("Помилка: книгу не знайдено.");
            else if (!user.RentBook(book)) Console.WriteLine("Помилка: книга вже в оренді.");
            else Console.WriteLine($"Книгу «{book.Title}» видано користувачу {user.Name}.");
        }

        public void ReturnBook(int userId, int bookId)
        {
            User user = users.Find(u => u.Id == userId);
            Book book = books.Find(b => b.Id == bookId);
            if (user == null) Console.WriteLine("Помилка: користувача не знайдено.");
            else if (book == null) Console.WriteLine("Помилка: книгу не знайдено.");
            else if (!user.ReturnBook(book)) Console.WriteLine("Помилка: користувач не орендував цю книгу.");
            else Console.WriteLine($"Книгу «{book.Title}» повернено користувачем {user.Name}.");
        }

        public void ShowAllBooks()
        {
            Console.WriteLine("\nУсі книги:");
            foreach (Book book in books) book.ShowInfo();
        }

        public void ShowUsers()
        {
            Console.WriteLine("\nУсі користувачі:");
            foreach (User user in users) user.ShowInfo();
        }

        public void ShowAvailableBooks()
        {
            Console.WriteLine("\nДоступні книги:");
            bool found = false;
            foreach (Book book in books)
                if (book.IsAvailable) { book.ShowInfo(); found = true; }
            if (!found) Console.WriteLine("Немає доступних книг.");
        }

        public void ShowRentedBooks()
        {
            Console.WriteLine("\nКниги в оренді:");
            bool found = false;
            foreach (Book book in books)
                if (!book.IsAvailable) { book.ShowInfo(); found = true; }
            if (!found) Console.WriteLine("Немає книг в оренді.");
        }
    }
}
