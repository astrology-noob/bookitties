using Bookitties.Data;
using System.Configuration;

using (var context = new AppDBContext(ConfigurationManager.ConnectionStrings["AppDBContext"].ConnectionString))
{
    BookService bs = new BookService(context);
    List<Book> books = await bs.GetBooksByPropertyAsync(context.Books.ToList(), PropertyEnum.Author, "Fy");
    Console.WriteLine(books[0]);
}