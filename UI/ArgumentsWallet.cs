namespace Bookitties.UI
{
    public sealed record ArgumentsWallet(Command Command, ICollection<FlagAttribute> Flags);
}
