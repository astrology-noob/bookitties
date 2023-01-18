using Bookitties.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bookitties.UI
{
    public static class ArgumentsParser
    {
        public static async ValueTask<ArgumentsWallet> Parse(string[] args)
        {
            Command command = null!;
            var flags = new List<FlagAttribute>();

            var parseTask = Task.Run(() =>
            {
                if (args.Length == 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(args));
                }
                if (!new Regex(@"^[a-zA-Z]+$").IsMatch(args[0]))
                {
                    throw new CommandDoesNotExistException();
                }
                else
                {
                    command = args[0] switch
                    {
                        "get" => Command.Get,
                        "add" => Command.AddNew,
                        _ => throw new Exception()
                    };
                }

                var flagRegex = new Regex("--([a-zA-Z-]+)=(.+)");
                foreach (var arg in args[1..])
                {
                    var match = flagRegex.Match(arg);
                    if (!match.Success)
                    {
                        throw new ValidationException("Not valid argument syntax. Example: --author=Twen");
                    }
                    
                    flags.Add(new FlagAttribute(match.Groups[1].Value, match.Groups[2].Value));
                }
            });

            await parseTask;
            parseTask.Wait();

            return new ArgumentsWallet(command, flags);
        }
    }
}
