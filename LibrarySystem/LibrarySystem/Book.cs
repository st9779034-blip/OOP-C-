using System;
namespace LibrarySystem
{
    public class Book
    {
        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public bool IsAvailable { get { return Borrower == null; } }
        public User Borrower { get; private set; }

        public Book(int id, string title, string author)
        {
            Id = id;
            Title = title;
            Author = author;
        }

        internal bool Rent(User user)
        {
            if (!IsAvailable || user == null) return false;
            Borrower = user;
            return true;
        }

        internal bool Return(User user)
        {
            if (user == null || Borrower != user) return false;
            Borrower = null;
            return true;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"ID: {Id} | {Title} | {Author} | " +
                (IsAvailable ? "Доступна" : $"В оренді: {Borrower.Name}"));
        }
    }
}
