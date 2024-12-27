
namespace Synaptic.Analysis;

public static class TokenExtensions
{
    public static string Word(this Token token)
    {
        return new string(token.Span);
    }
}