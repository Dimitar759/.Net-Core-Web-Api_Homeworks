using Homework2.Models;

namespace Homework2
{
    public class StaticDb
    {
        public static List<Book> Books = new List<Book>()
        {
            new Book()
            {
                Author = "Marcus Aurelius",
                Title = "Meditations",

            },

            new Book()
            {
                Author = "Epictetus",
                Title = "Enchiridion"

            },

            new Book()
            {
                Author = "Seneca",
                Title = "On the Shortness of Life ",

            },
        };
    }
}
