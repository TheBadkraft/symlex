
namespace Symlex.Grammar;

public static class GrammarExtensions
{
    public static string Word(this Token token)
    {
        return new string(token.Span);
    }
}