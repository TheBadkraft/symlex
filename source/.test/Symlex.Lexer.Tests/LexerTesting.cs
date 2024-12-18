
using Symlex.Analyzers;
using Symlex.Grammar;

namespace Symlex.Terminal.Tests;

[TestContainer(Ignore = false)]
public class LexerTesting
{
    private Lexer lexer;

    public static TestContext TestContext { get; set; }

    [Test]
    public void TokenizeEmptyString()
    {
        var tokens = lexer.Tokenize(new ArraySegment<char>());
        Assert.IsEmpty(tokens);
    }

    [Test]
    public void TokenizeWhiteSpace()
    {
        var tokens = lexer.Tokenize(new ArraySegment<char>(" \t\n\r".ToCharArray()));
        Assert.IsEmpty(tokens);
    }

    [Test]
    public void TokenizeSingleWord()
    {
        var tokens = lexer.Tokenize(new ArraySegment<char>("hello".ToCharArray()));
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual("hello", new string(tokens.First().Word()));
        Assert.AreEqual(TokenType.Identifier, tokens.First().Type);
    }

    [Test]
    public void TokenizeSingleNumber()
    {
        var tokens = lexer.Tokenize(new ArraySegment<char>("123".ToCharArray()));
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual("123", new string(tokens.First().Word()));
        Assert.AreEqual(TokenType.Number, tokens.First().Type);
    }

    [Test]
    public void TokenizeSingleOperator()
    {
        var tokens = lexer.Tokenize(new ArraySegment<char>("+".ToCharArray()));
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual("+", new string(tokens.First().Word()));
        Assert.AreEqual(TokenType.Operator, tokens.First().Type);
    }

    [Test]
    public void TokenizeKeyword()
    {
        var tokens = lexer.Tokenize(new ArraySegment<char>("if".ToCharArray()));
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual("if", new string(tokens.First().Word()));
        Assert.AreEqual(TokenType.Keyword, tokens.First().Type);
    }

    [Test]
    public void TokenizeExpression()
    {
        var input = "[=proc test(input): if x < 5 then: body x = 5]";
        var statement = new ArraySegment<char>(input.ToCharArray());
        var tokens = lexer.Tokenize(statement);

        Assert.AreEqual(19, tokens.Count);
        Assert.AreEqual(TokenType.Operator, tokens[0].Type, $"[: exp {TokenType.Operator} act {tokens[0].Type}");    //  [
        Assert.AreEqual(TokenType.Operator, tokens[1].Type, $"=: exp {TokenType.Operator} act {tokens[1].Type}");    //  =
        Assert.AreEqual(TokenType.Keyword, tokens[2].Type, $"proc: exp {TokenType.Keyword} act {tokens[2].Type}");     //  proc
        Assert.AreEqual(TokenType.Identifier, tokens[3].Type, $"test: exp {TokenType.Identifier} act {tokens[3].Type}");  //  test
        Assert.AreEqual(TokenType.Operator, tokens[4].Type, $"(: exp {TokenType.Operator} act {tokens[4].Type}");    //  (
        Assert.AreEqual(TokenType.Keyword, tokens[5].Type, $"input: exp {TokenType.Keyword} act {tokens[5].Type}");  //  input
        Assert.AreEqual(TokenType.Operator, tokens[6].Type, $"): exp {TokenType.Operator} act {tokens[6].Type}");    //  )
        Assert.AreEqual(TokenType.Operator, tokens[7].Type, $":: exp {TokenType.Operator} act {tokens[7].Type}");    //  :
        Assert.AreEqual(TokenType.Keyword, tokens[8].Type, $"if: exp {TokenType.Keyword} act {tokens[8].Type}");     //  if
        Assert.AreEqual(TokenType.Identifier, tokens[9].Type, $"x: exp {TokenType.Identifier} act {tokens[9].Type}");  //  x
        Assert.AreEqual(TokenType.Operator, tokens[10].Type, $"<: exp {TokenType.Operator} act {tokens[10].Type}");   //  <
        Assert.AreEqual(TokenType.Number, tokens[11].Type, $"5: exp {TokenType.Number} act {tokens[11].Type}");     //  5
        Assert.AreEqual(TokenType.Keyword, tokens[12].Type, $"then: exp {TokenType.Keyword} act {tokens[12].Type}");    //  then
        Assert.AreEqual(TokenType.Operator, tokens[13].Type, $":: exp {TokenType.Operator} act {tokens[13].Type}");   //  :
        Assert.AreEqual(TokenType.Keyword, tokens[14].Type, $"body: exp {TokenType.Keyword} act {tokens[14].Type}");    //  body
        Assert.AreEqual(TokenType.Identifier, tokens[15].Type, $"x: exp {TokenType.Identifier} act {tokens[15].Type}"); //  x
        Assert.AreEqual(TokenType.Operator, tokens[16].Type, $"=: exp {TokenType.Operator} act {tokens[16].Type}");   //  =
        Assert.AreEqual(TokenType.Number, tokens[17].Type, $"5: exp {TokenType.Number} act {tokens[17].Type}");     //  5
        Assert.AreEqual(TokenType.Operator, tokens[18].Type, $"]: exp {TokenType.Operator} act {tokens[18].Type}");   //  ]
    }

    [Test]
    public void TokenizeInvalid()
    {
        var tokens = lexer.Tokenize(new ArraySegment<char>("$".ToCharArray()));
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual("$", new string(tokens.First().Word()));
        Assert.AreEqual(TokenType.Invalid, tokens.First().Type);
    }

    #region Test SetUp & TearDown
    [SetUp]
    public void SetUp()
    {
        lexer = new Lexer();
    }

    [TearDown]
    public void TearDown()
    {
        lexer = null;
        Debug.WriteLine($"{TestContext.ContainerName}.{TestContext.TestMethodName} :: Result [{TestContext.TestResult}]{(!string.IsNullOrEmpty(TestContext.ErrorMessage) ? $" :: {TestContext.ErrorMessage}" : string.Empty)}");
    }
    #endregion
}
