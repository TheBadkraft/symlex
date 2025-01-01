
using Synaptic.Analysis;
using Synaptic.Services.Analysis;

namespace Synaptic.Terminal.Tests;

[TestContainer(Ignore = true)]
public class LexerTesting
{
    private LexerService lexer;

    public static TestContext TestContext { get; set; }

    [Test]
    public void EmptyString()
    {
        var tokens = lexer.Tokenize<Token>(new ArraySegment<char>());
        Assert.IsEmpty(tokens);
    }

    [Test]
    public void WhiteSpace()
    {
        var tokens = lexer.Tokenize<Token>(new ArraySegment<char>(" \t\n\r".ToCharArray()));
        Assert.IsEmpty(tokens);
    }

    [Test]
    public void SingleWord()
    {
        var tokens = lexer.Tokenize<Token>(new ArraySegment<char>("hello".ToCharArray()));
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual("hello", new string(tokens.First().Word()));
        Assert.AreEqual(TokenType.Identifier, tokens.First().Type);
    }

    [Test]
    public void SingleNumber()
    {
        var tokens = lexer.Tokenize<Token>(new ArraySegment<char>("123".ToCharArray()));
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual("123", new string(tokens.First().Word()));
        Assert.AreEqual(TokenType.Number, tokens.First().Type);
    }

    [Test]
    public void SingleOperator()
    {
        var tokens = lexer.Tokenize<Token>(new ArraySegment<char>("+".ToCharArray()));
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual("+", new string(tokens.First().Word()));
        Assert.AreEqual(TokenType.Operator, tokens.First().Type);
    }

    [Test]
    public void Keyword()
    {
        var tokens = lexer.Tokenize<Token>(new ArraySegment<char>("if".ToCharArray()));
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual("if", new string(tokens.First().Word()));
        Assert.AreEqual(TokenType.Keyword, tokens.First().Type);
    }

    //  tokenize comma delimited list
    [Test]
    public void CommaDelimitedList()
    {
        var input = "x, y, z";
        var statement = new ArraySegment<char>(input.ToCharArray());
        var tokens = lexer.Tokenize<Token>(statement);

        int x = 0;
        Assert.AreEqual(5, tokens.Count);
        Assert.AreEqual("x", new string(tokens[x].Word()));
        Assert.AreEqual(TokenType.Identifier, tokens[x++].Type);
        Assert.AreEqual(",", new string(tokens[x].Word()));
        Assert.AreEqual(TokenType.Operator, tokens[x++].Type);
        Assert.AreEqual("y", new string(tokens[x].Word()));
        Assert.AreEqual(TokenType.Identifier, tokens[x++].Type);
        Assert.AreEqual(",", new string(tokens[x].Word()));
        Assert.AreEqual(TokenType.Operator, tokens[x++].Type);
        Assert.AreEqual("z", new string(tokens[x].Word()));
        Assert.AreEqual(TokenType.Identifier, tokens[x++].Type);
    }

    [Test]
    public void SimpleExpression()
    {
        var input = "[=proc test(input): if x < 5 then: body x = 5]";
        var statement = new ArraySegment<char>(input.ToCharArray());
        var tokens = lexer.Tokenize<Token>(statement);

        int x = 0;
        Assert.AreEqual(18, tokens.Count);
        Assert.AreEqual(TokenType.Operator, tokens[x].Type, $"[=: exp {TokenType.Operator} act {tokens[x++].Type}");    //  [=
        Assert.AreEqual(TokenType.Keyword, tokens[x].Type, $"proc: exp {TokenType.Keyword} act {tokens[x++].Type}");     //  proc
        Assert.AreEqual(TokenType.Identifier, tokens[x].Type, $"test: exp {TokenType.Identifier} act {tokens[x++].Type}");  //  test
        Assert.AreEqual(TokenType.Operator, tokens[x].Type, $"(: exp {TokenType.Operator} act {tokens[x++].Type}");    //  (
        Assert.AreEqual(TokenType.Keyword, tokens[x].Type, $"input: exp {TokenType.Keyword} act {tokens[x++].Type}");  //  input
        Assert.AreEqual(TokenType.Operator, tokens[x].Type, $"): exp {TokenType.Operator} act {tokens[x++].Type}");    //  )
        Assert.AreEqual(TokenType.Operator, tokens[x].Type, $":: exp {TokenType.Operator} act {tokens[x++].Type}");    //  :
        Assert.AreEqual(TokenType.Keyword, tokens[x].Type, $"if: exp {TokenType.Keyword} act {tokens[x++].Type}");     //  if
        Assert.AreEqual(TokenType.Identifier, tokens[x].Type, $"x: exp {TokenType.Identifier} act {tokens[x++].Type}");  //  x
        Assert.AreEqual(TokenType.Operator, tokens[x].Type, $"<: exp {TokenType.Operator} act {tokens[x++].Type}");   //  <
        Assert.AreEqual(TokenType.Number, tokens[x].Type, $"5: exp {TokenType.Number} act {tokens[x++].Type}");     //  5
        Assert.AreEqual(TokenType.Keyword, tokens[x].Type, $"then: exp {TokenType.Keyword} act {tokens[x++].Type}");    //  then
        Assert.AreEqual(TokenType.Operator, tokens[x].Type, $":: exp {TokenType.Operator} act {tokens[x++].Type}");   //  :
        Assert.AreEqual(TokenType.Keyword, tokens[x].Type, $"body: exp {TokenType.Keyword} act {tokens[x++].Type}");    //  body
        Assert.AreEqual(TokenType.Identifier, tokens[x].Type, $"x: exp {TokenType.Identifier} act {tokens[x++].Type}"); //  x
        Assert.AreEqual(TokenType.Operator, tokens[x].Type, $"=: exp {TokenType.Operator} act {tokens[x++].Type}");   //  =
        Assert.AreEqual(TokenType.Number, tokens[x].Type, $"5: exp {TokenType.Number} act {tokens[x++].Type}");     //  5
        Assert.AreEqual(TokenType.Operator, tokens[x].Type, $"]: exp {TokenType.Operator} act {tokens[x++].Type}");   //  ]
    }

    [Test]
    public void Invalid()
    {
        var tokens = lexer.Tokenize<Token>(new ArraySegment<char>("$".ToCharArray()));
        Assert.AreEqual(1, tokens.Count);
        Assert.AreEqual("$", new string(tokens.First().Word()));
        Assert.AreEqual(TokenType.Invalid, tokens.First().Type);
    }

    [Test]
    public void MultilineStatement()
    {
        var input = @"[=proc
add
: input a, b
:body + a, b]
";
        var tokens = lexer.Tokenize<Token>(new(input.ToCharArray()));

    }

    #region Test SetUp & TearDown
    [SetUp]
    public void SetUp()
    {
        //  passing NULL to the constructor is okay for now since it isn't being used
        lexer = new(null);
    }

    [TearDown]
    public void TearDown()
    {
        lexer = null;
        Debug.WriteLine($"{TestContext.ContainerName}.{TestContext.TestMethodName} :: Result [{TestContext.TestResult}]{(!string.IsNullOrEmpty(TestContext.ErrorMessage) ? $" :: {TestContext.ErrorMessage}" : string.Empty)}");
    }
    #endregion
}
