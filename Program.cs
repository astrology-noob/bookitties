using Bookitties.Data;
using Bookitties.UI;
using System.Configuration;

using (var context = new AppDBContext(ConfigurationManager.ConnectionStrings["AppDBContext"].ConnectionString))
{
    BookService bs = new BookService(context);

    var arguments = await ArgumentsParser.Parse(args);
    await arguments.Command.Action.Invoke(arguments.Flags, bs);
}