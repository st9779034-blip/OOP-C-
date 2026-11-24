using System;
using System.Collections.Generic;
namespace LibrarySystem
{
    public class User
    {
        public int Id { get; }
        public string Name { get; }
        private readonly List<Book> rentedBooks = new List<Book>();
        public IReadOnlyList<Book> RentedBooks { get { return rentedBooks.AsReadOnly(); } }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }

        internal bool RentBook(Book book)
        {
            if (!book.Rent(this)) return false;
            rentedBooks.Add(book);
            return true;
        }

        internal bool ReturnBook(Book book)
        {
            if (!rentedBooks.Contains(book) || !book.Return(this)) return false;
            rentedBooks.Remove(book);
            return true;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"ID: {Id} | Ім'я: {Name}");
            if (rentedBooks.Count == 0)
                Console.WriteLine("  Немає орендованих книг.");
            foreach (Book book in rentedBooks)
                Console.WriteLine("  - " + book.Title);
        }
    }
}
