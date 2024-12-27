
using Synaptic.Analysis;
using Synaptic.Core;

namespace Synaptic.Services.Analysis;

/// <summary>
/// A service that tokenizes input statements.
/// </summary>
/// <remarks>
/// Consider managing multiple lexers for multi-threading
/// </remarks>
public class LexerService : ILexerService
{
    private InputLexer Lexer { get; init; } = new InputLexer();

    internal LexerService() { }

    /// <summary>
    /// Tokenizes the input statement.
    /// </summary>
    /// <param name="input">The input statement to tokenize</param>
    /// <returns>The tokens</returns>
    public IReadOnlyList<TResult> Tokenize<TResult>(ArraySegment<char> input)
    {
        return (IReadOnlyList<TResult>)Lexer.Tokenize(input);
    }

    public void OnRegistered(IServiceContainer serviceContainer)
    {
        //  nothing to do here ...
    }

    private class InputLexer
    {
        internal InputLexer() { }

        /// <summary>
        /// Tokenizes the input statement.
        /// </summary>
        /// <param name="input">The entire statement to be tokenized</param>
        /// <returns></returns>
        public unsafe IEnumerable<Token> Tokenize(ArraySegment<char> input)
        {
            List<Token> tokens = new();

            fixed (char* pText = input.Array)
            {
                char* p = pText + input.Offset;
                char* end = p + input.Count;

                while (p < end)
                {
                    //  skip white space
                    if (char.IsWhiteSpace(*p))
                    {
                        p++;
                        continue;
                    }

                    //  recognize "[=" as operator
                    if (*p == '[' && p + 1 < end && *(p + 1) == '=')
                    {
                        var op = new ArraySegment<char>(input.Array, (int)(p - pText), 2);
                        tokens.Add(new Token(op, TokenType.Operator));

                        p += op.Count;
                        continue;
                    }

                    //  recognize operators & symbols
                    if (IsSymbol(*p) || IsOperator(*p))
                    {
                        //  TODO: handle multi-character operators and symbols
                        tokens.Add(new Token(new ArraySegment<char>(input.Array, (int)(p - pText), 1), TokenType.Operator));

                        p++;
                        continue;
                    }

                    //  recognize identifiers or keywords
                    if (char.IsLetter(*p) || *p == '_')
                    {
                        var identifier = ReadIdentifier(input.Array, pText, p, end, out TokenType type);
                        tokens.Add(new Token(identifier, type));

                        p += identifier.Count;
                        continue;
                    }

                    //  recognize numbers
                    if (char.IsDigit(*p))
                    {
                        var number = ReadNumber(input.Array, pText, p, end);
                        tokens.Add(new Token(number, TokenType.Number));

                        p += number.Count;
                        continue;
                    }

                    //  fall through invalid character/sequence
                    var invalid = new ArraySegment<char>(input.Array, (int)(p - pText), 1);
                    tokens.Add(new Token(invalid, TokenType.Invalid));

                    p += invalid.Count;
                }

                return tokens;
            }
        }

        //  Symbols: [ ] ( ) { } : ,
        private static bool IsSymbol(char c) => c switch
        {
            '[' => true,
            ']' => true,
            '(' => true,
            ')' => true,
            '{' => true,
            '}' => true,
            ':' => true,
            ',' => true,
            _ => false
        };

        //  Operators: + - * / % = ! < > ^
        private static bool IsOperator(char c) => c switch
        {
            '+' => true,
            '-' => true,
            '*' => true,
            '/' => true,
            '%' => true,
            '=' => true,
            '!' => true,
            '<' => true,
            '>' => true,
            '^' => true,
            _ => false
        };

        //  Read an identifier or keyword
        private static unsafe ArraySegment<char> ReadIdentifier(char[] array, char* input, char* p, char* end, out TokenType type)
        {
            char* start = p;
            while (p < end && (char.IsLetterOrDigit(*p) || *p == '_'))
            {
                p++;
            }
            var segment = new ArraySegment<char>(array, (int)(start - input), (int)(p - start));
            // check if the identifier is valid: first character is a letter or underscore
            if (start == p || !char.IsLetter(*start) && *start != '_')
            {
                type = TokenType.Invalid;
            }
            // check if the identifier is a keyword
            else if (IsKeyword(segment))
            {
                type = TokenType.Keyword;
            }
            else
            {
                type = TokenType.Identifier;
            }

            return segment;
        }

        //  Read a number
        private static unsafe ArraySegment<char> ReadNumber(char[] array, char* input, char* p, char* end)
        {
            char* start = p;
            while (p < end && char.IsDigit(*p))
            {
                p++;
            }
            var segment = new ArraySegment<char>(array, (int)(start - input), (int)(p - start));

            return segment;
        }

        //  Check if the identifier is a keyword
        private static bool IsKeyword(ArraySegment<char> identifier)
        {
            // keywords = { "proc", "input", "body", "if", "then", "else", "var", "call", "with", "while", "do", "for", "in", "match", "case" };
            string[] keywords = { "proc", "input", "body", "if", "then", "else", "var", "call", "with", "while", "do", "for", "in", "match", "case" };
            string identifierString = new string(identifier.Array, identifier.Offset, identifier.Count);
            return Array.IndexOf(keywords, identifierString) != -1;
        }
    }
}
