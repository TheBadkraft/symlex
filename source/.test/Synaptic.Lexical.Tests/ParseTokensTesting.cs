
using Synaptic.Analysis;
using Synaptic.Services.Analysis;

namespace Synaptic.Terminal.Tests;

/// <summary>
/// Test container to test FunctionParser class
/// </summary>
[TestContainer(Ignore = true)]
public class ParseTokensTesting
{
    private InputParser Parser { get; set; }

    public static TestContext TestContext { get; set; }

    #region Test Methods
    //  Parses a simple function definition with no parameters
    [Test]
    public void SimpleFunctionDefinition()
    {
        // Arrange
        var input = "[=proc simpleFunc :input :body ()]";
        Parser.ParseInput(Tokenize(input));
        var expSource = "simpleFunc :input :body ()";

        // Act
        ContextDescriptor descriptor = Parser.Descriptor;

        // Assert
        Assert.IsNotNull(descriptor);
        Assert.AreEqual(CompilerOp.Create, descriptor.Action);
        Assert.AreEqual(CompilerTarget.Function, descriptor.Target);
        //  "simpleFunc :input :body ()"
        int x = 0;
        Assert.AreEqual(TokenType.Identifier, descriptor.Data[x].Type, $"{x}:{descriptor.Data[x].Type}");
        Assert.AreEqual("simpleFunc", descriptor.Data[x].Word(), $"{x}:{descriptor.Data[x].Word()}");
        x++;
        Assert.AreEqual(TokenType.Operator, descriptor.Data[x].Type, $"{x}:{descriptor.Data[x].Type}");
        Assert.AreEqual(":", descriptor.Data[x].Word(), $"{x}:{descriptor.Data[x].Word()}");
        x++;
        Assert.AreEqual(TokenType.Keyword, descriptor.Data[x].Type, $"{x}:{descriptor.Data[x].Type}");
        Assert.AreEqual("input", descriptor.Data[x].Word(), $"{x}:{descriptor.Data[x].Word()}");
        x++;
        Assert.AreEqual(TokenType.Operator, descriptor.Data[x].Type, $"{x}:{descriptor.Data[x].Type}");
        Assert.AreEqual(":", descriptor.Data[x].Word(), $"{x}:{descriptor.Data[x].Word()}");
        x++;
        Assert.AreEqual(TokenType.Keyword, descriptor.Data[x].Type, $"{x}:{descriptor.Data[x].Type}");
        Assert.AreEqual("body", descriptor.Data[x].Word(), $"{x}:{descriptor.Data[x].Word()}");
        x++;
        Assert.AreEqual(TokenType.Operator, descriptor.Data[x].Type, $"{x}:{descriptor.Data[x].Type}");
        Assert.AreEqual("(", descriptor.Data[x].Word(), $"{x}:{descriptor.Data[x].Word()}");
        x++;
        Assert.AreEqual(TokenType.Operator, descriptor.Data[x].Type, $"{x}:{descriptor.Data[x].Type}");
        Assert.AreEqual(")", descriptor.Data[x].Word(), $"{x}:{descriptor.Data[x].Word()}");

        Assert.AreEqual(7, descriptor.Data.Count);

        Assert.AreEqual(expSource, descriptor.Data[0].Source);
        Debug.WriteLine($"{TestContext.TestMethodName} :: {new string(descriptor.Data[0].Source)}");

    }
    //  Parses a function definition with parameters
    [Test(Skip = false)]
    public void FunctionWithParameters()
    {
        // Arrange
        var input = "[=proc funcWithParams :input param1, param2 :body ()]";
        Parser.ParseInput(Tokenize(input));
        var expSource = "funcWithParams :input param1, param2 :body ()";

        // Act
        ContextDescriptor descriptor = Parser.Descriptor;

        // Assert
        Assert.AreEqual(CompilerOp.Create, descriptor.Action);
        Assert.AreEqual(CompilerTarget.Function, descriptor.Target);

        Assert.AreEqual(10, descriptor.Data.Count);

        Assert.AreEqual(expSource, descriptor.Data[0].Source);
        Debug.WriteLine($"{TestContext.TestMethodName} :: {new string(descriptor.Data[0].Source)}");
    }
    //  Parses function with body
    [Test(Skip = true)]
    public void FunctionWithBody()
    {
        // Arrange
        var input = "[=proc funcWithBody :input :body (var x 5)]";
        var tokens = Tokenize(input);

        // // Act
        // Function func = Parser.Parse(tokens);

        // // Assert
        // Assert.IsNotNull(func);
        // Assert.AreEqual("funcWithBody", func.Name);
        // Assert.AreEqual(0, func.Parameters.Count);
        // Assert.AreEqual(3, func.Body.Count);
    }
    //  fails to parse invalid function definition
    [Test(Skip = true)]
    public void InvalidFunctionDefinition()
    {
        // Arrange
        var input = "[=proc invalidFunc]";
        var tokens = Tokenize(input);

        // Act
        // Function func = Parser.Parse(tokens);

        // // Assert
        // Assert.IsNull(func);
    }
    #endregion Test Methods

    #region Helper Methods
    private IReadOnlyList<Token> Tokenize(string input)
    {
        var lexer = new LexerService();
        var statement = new ArraySegment<char>(input.ToCharArray());

        return lexer.Tokenize(statement);
    }
    #endregion Helper Methods
    #region Test SetUp & TearDown
    [SetUp]
    public void Setup()
    {
        Parser = new InputParser();
    }

    [TearDown]
    public void TearDown()
    {
        Parser = null;
        Debug.WriteLine($"{TestContext.ContainerName}.{TestContext.TestMethodName} :: Result [{TestContext.TestResult}]{(!string.IsNullOrEmpty(TestContext.ErrorMessage) ? $" :: {TestContext.ErrorMessage}" : string.Empty)}");
    }
    #endregion Test SetUp & TearDown
    #region Container Initalization & CleanUp
    #endregion Container Initalization & CleanUp
}
