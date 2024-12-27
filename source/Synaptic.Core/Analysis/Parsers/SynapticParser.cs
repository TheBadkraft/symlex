
using System.Collections.ObjectModel;

using Synaptic.Core;

namespace Synaptic.Analysis.Parsers;

/// <summary>
/// Parses a list of tokens a specialized object
/// </summary>
/// <typeparam name="T">The type of object to parse the tokens into</typeparam>
public abstract class SynapticParser : Parser
{
    protected SynapticParser() { }

    /// <summary>
    /// Attempts to parse an identifier from the token list
    /// </summary>
    /// <param name="index">The current token list index</param>
    /// <param name="tokens">The token list</param>
    /// <param name="name">The identifier name</param>
    /// <returns>TRUE if an identifier was parsed; FALSE otherwise</returns>
    protected static bool TryParseIdentifier(ref int index, Data tokens, out string name)
    {
        //  try to parse identifier
        name = string.Empty;
        if (index < tokens.Count && tokens[index].Type == TokenType.Identifier)
        {
            name = tokens[index++].Word();
        }

        return !string.IsNullOrEmpty(name);
    }
    /// <summary>
    /// Attempts to parse parameters from the token list
    /// </summary>
    /// <param name="index">The current token list index</param>
    /// <param name="tokens">The token list</param>
    /// <param name="parameters">The collection of parameters</param>
    /// <returns>TRUE if parameters were parsed; FALSE otherwise</returns>
    protected static bool TryParseParameters(ref int index, Data tokens, out IReadOnlyCollection<Function.Parameter> parameters)
    {
        //  try to parse parameters
        var paramList = new Collection<Function.Parameter>();
        parameters = paramList; // Enumerable.Empty<Function.Parameter>().ToList();
        //  requires prefix `input`
        if (!TryConsumeToken(ref index, tokens, TokenType.Keyword, "input"))
        {
            return false;   //  missing 'input' prefix
        }
        //  paramList ...
        while (index < tokens.Count && tokens[index].Type != TokenType.Operator)    //  until either `)` or `:`
        {
            if (!TryParseIdentifier(ref index, tokens, out string name))
            {
                return false;   //  invalid parameter name
            }
            if (index < tokens.Count && tokens[index].Type == TokenType.Operator && tokens[index].Span[0] == ',')
            {
                index++;    //  consume the comma
            }
            paramList.Add(new(name, null));    //  assuming dynamic typing
        }
        // parameters = paramList;
        return parameters.Count >= 0;
    }
    /// <summary>
    /// Attempts to parse the function body from the token list
    /// </summary>
    /// <param name="index">The current token list index</param>
    /// <param name="tokens">The token list</param>
    /// <param name="body">The collection of tokens representing the body</param>
    /// <returns>TRUE if the body was parsed; FALSE otherwise</returns>
    protected static bool TryParseBody(ref int index, Data tokens, out IReadOnlyCollection<Token> body)
    {
        //  try to parse body
        var bodyTokens = new Collection<Token>();
        body = bodyTokens;    //  Enumerable.Empty<Token>().ToList();
        //  requires prefix `body`
        if (!TryConsumeToken(ref index, tokens, TokenType.Keyword, "body"))
        {
            return false;   //  missing 'body' prefix
        }
        //  requires `(` to start body
        if (!TryConsumeToken(ref index, tokens, TokenType.Operator, "("))
        {
            return false;   //  missing '('
        }
        //  bodyList ...
        int parensBalance = 1;  //  to keep track of nested parens
        while (index < tokens.Count)
        {
            if (tokens[index].Type == TokenType.Operator)
            {
                if (tokens[index].Span[0] == '(') parensBalance++;
                else if (tokens[index].Span[0] == ')') parensBalance--;
            }
            if (parensBalance == 0) break;
            bodyTokens.Add(tokens[index++]);
        }

        if (parensBalance != 0 || !TryConsumeToken(ref index, tokens, TokenType.Operator, ")"))
        {
            return false;   //  unbalanced parens
        }

        return bodyTokens.Count >= 0;
    }
    /// <summary>
    /// Attempts to consume a token from the token list
    /// </summary>
    /// <param name="index">The current token list index</param>
    /// <param name="tokens">The token list</param>
    /// <param name="expType">The expected token type</param>
    /// <param name="expInput">The expected token input</param>
    /// <returns>TRUE if the token was consumed; FALSE otherwise</returns>
    protected static bool TryConsumeToken(ref int index, Data tokens, TokenType expType, string expInput = null)
    {
        //  try to consume the token if type and expected input
        if (index >= tokens.Count || tokens[index].Type != expType)
        {
            return false;
        }
        if (!tokens[index].SequenceEqual(expInput))
        {
            return false;
        }
        index++;

        return true;
    }

}
