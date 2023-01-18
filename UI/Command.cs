using Bookitties.Data;

namespace Bookitties.UI
{
    public abstract class Command
    {
        public static Command Get { get; } = new GetCommand();
        public static Command AddNew { get; } = new AddNewCommand();

        public string Name { get; protected set; } = string.Empty;
        public Func<ICollection<FlagAttribute>, BookService, Task> Action { get; protected set; } = null!;

        public Command(string name)
        {
            Name = name;
        }

        private sealed class GetCommand : Command
        {
            public GetCommand() : base("get")
            {
                Action = GetBooks;
            }

            public async Task GetBooks(ICollection<FlagAttribute> flags, BookService bookService)
            {
                var books = await bookService.GetBooksFromDBAsync();

                if (flags.Count > 0)
                {
                    foreach (var flag in flags)
                    {
                        books = flag.Name switch
                        {
                            "title" => await bookService.GetBooksByPropertyAsync(
                                books: books,
                                option: PropertyEnum.Title,
                                desiredValue: flag.Value),
                            "author" => await bookService.GetBooksByPropertyAsync(
                                books: books,
                                option: PropertyEnum.Author,
                                desiredValue: flag.Value),
                            "date" => await bookService.GetBooksByPropertyAsync(
                                books: books,
                                option: PropertyEnum.Published,
                                desiredValue: DateTime.Parse(flag.Value)),
                            "order-by" => await bookService.GetBooksOrderedAsync(
                                books: books,
                                option: flag.Value switch
                                    {
                                        "title" => PropertyEnum.Title,
                                        "author" => PropertyEnum.Author,
                                        "date" => PropertyEnum.Published,
                                        _ => throw new Exception()
                                    }),
                            _ => throw new Exception()
                        };
                    }
                }

                if (books.Count == 0)
                {
                    Console.WriteLine("Books were not found :(");
                    return;
                }

                foreach (var book in books)
                {
                    if (book is not null)
                    {
                        Console.WriteLine(book);
                    }
                }
            }
        }

        private sealed class AddNewCommand : Command
        {
            public AddNewCommand() : base("add")
            {
                Action = AddBook;
            }

            public async Task AddBook(ICollection<FlagAttribute> flags, BookService bookService)
            {
                if (flags.Count != 3)
                {
                    throw new ArgumentException("Command 'add' requires 3 arguments (title, author, date).");
                }

                var flagsAsList = flags.ToList();
                await bookService.AddBookAsync(new Book(
                    flagsAsList[0].Value,
                    flagsAsList[1].Value,
                    DateTime.Parse(flagsAsList[2].Value)));
            }
        }
    }

}
