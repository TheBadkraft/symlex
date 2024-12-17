using System.Text;

using Symlex.IO;

namespace Symlex.Input.Tests;

[TestContainer(Ignore = false)]
public class ReadStatementTesting
{
    private InputBuffer inputBuffer;
    private MemoryStream mockInputStream;
    private StringBuilder consoleOutput;

    public static TestContext TestContext { get; set; }

    [Test(Skip = true)]
    public void ReadStatementSingleLine()
    {
        WriteToMockStream("test\n");
        var statement = inputBuffer.ReadStatement();
        var result = new string(statement.Array, statement.Offset, statement.Count);

        Assert.AreEqual("test", result);
        Debug.WriteLine($"ReadStatementSingleLine (statement) :: {result}");
    }

    [Test]
    public void ReadStatementMultiLine()
    {
        string input = "[=proc add\n:input a, b\n:body a + b]\n";
        WriteToMockStream(input);
        var statement = inputBuffer.ReadStatement();
        var result = new string(statement.Array, statement.Offset, statement.Count);

        Assert.AreEqual("[=proc add:input a, b:body a + b]", result);
        Debug.WriteLine($"ReadStatementMultiLine (statement) :: {result}");

        Assert.AreEqual(input.TrimEnd('\n'), consoleOutput.ToString());
        Debug.WriteLine($"ReadStatementMultiLine (console)   :: {consoleOutput}");
    }

    private void WriteToMockStream(string text)
    {
        byte[] data = Encoding.UTF8.GetBytes(text);
        mockInputStream.Write(data, 0, data.Length);
        mockInputStream.Seek(0, SeekOrigin.Begin); // Reset the stream position
    }

    #region Test SetUp & TearDown
    [SetUp]
    public void SetUp()
    {
        consoleOutput = new StringBuilder();
        mockInputStream = new MemoryStream();
        Console.SetOut(new StringWriter(consoleOutput));
        inputBuffer = new InputBuffer(mockInputStream);
    }

    [TearDown]
    public void TearDown()
    {
        inputBuffer.Dispose();
        Debug.WriteLine($"{TestContext.ContainerName}.{TestContext.TestMethodName} :: Result [{TestContext.TestResult}]");
    }
    #endregion Test SetUp & TearDown

}
