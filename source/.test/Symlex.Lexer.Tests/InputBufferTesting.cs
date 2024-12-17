using System.Text;

using Symlex.IO;

namespace Symlex.Input.Tests;

[TestContainer]
public class InputBufferTesting
{

    private InputBuffer inputBuffer;
    private MemoryStream mockInputStream;
    private StringBuilder consoleOutput;

    public static TestContext TestContext { get; set; }

    [Test]
    public void InitializedInputBuffer()
    {
        Assert.IsNotNull(inputBuffer);
    }

    [Test]
    public void InputBufferLength()
    {
        string input = "test";
        WriteToMockStream(input);
        Assert.AreEqual(input.Length, inputBuffer.Length);
    }

    [Test]
    public void ReadSingleLineInput()
    {
        string input = "test";
        WriteToMockStream(input);

        var scanBuffer = new List<char>();
        int scan;

        while ((scan = inputBuffer.Read()) != -1)
        {
            scanBuffer.Add((char)scan);
        }

        var statement = new string(scanBuffer.ToArray());
        scanBuffer.Clear();

        Assert.AreEqual(input, statement);
        Debug.WriteLine($"ReadInputBuffer (statement) :: {statement}");

        Assert.AreEqual(input, consoleOutput.ToString());
        Debug.WriteLine($"ReadInputBuffer (console)   :: {consoleOutput}");
    }


    [Test]
    public void ReadMultiLineInput()
    {
        string input = "[=proc add:input a, b:body a + b]";
        string multiLineInput = "[=proc add\n:input a, b\n:body a + b]\n";
        WriteToMockStream(multiLineInput);

        var scanBuffer = new List<char>();
        var scan = 0;

        while ((scan = inputBuffer.Read()) != -1)
        {
            scanBuffer.Add((char)scan);
        }

        var statement = new string(scanBuffer.ToArray());
        scanBuffer.Clear();

        Assert.AreEqual(input, statement);
        Debug.WriteLine($"ReadInputBuffer (statement) :: {statement}");

        Assert.AreEqual(multiLineInput.TrimEnd(), consoleOutput.ToString());
        Debug.WriteLine($"ReadInputBuffer (console)   :: {consoleOutput}");
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
        // Console.SetIn(new StreamReader(mockInputStream));
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

    #region Container Initialization & CleanUp
    [ContainerInitialize]
    public static void ContainerInitialize(TestContext context)
    {
        // TestContext = context;
    }
    #endregion Container Initialization & CleanUp
}
