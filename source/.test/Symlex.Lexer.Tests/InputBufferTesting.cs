using System.Text;

using Symlex.IO;

namespace Symlex.Terminal.Tests;

[TestContainer(Ignore = true)]
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
    public void ReadSingleCharacter()
    {
        string input = "t";
        WriteToMockStream(input);

        int scan = inputBuffer.Read();
        Assert.AreEqual(input[0], (char)scan);
        Debug.WriteLine($"ReadSingleCharacter (scan)    :: {scan} [{(char)scan}]");

        Assert.AreEqual(input, consoleOutput.ToString());
        Debug.WriteLine($"ReadSingleCharacter (console) :: {consoleOutput}");
    }

    [Test]
    public void ReadMultiCharacter()
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
        Debug.WriteLine($"ReadInputBuffer (scan)      :: {statement}");

        Assert.AreEqual(input, consoleOutput.ToString());
        Debug.WriteLine($"ReadInputBuffer (console)   :: {consoleOutput}");
    }

    [Test]
    public void ReadBackspace()
    {
        string input = "t\bp";
        WriteToMockStream(input);

        var scanBuffer = new List<char>();
        int scan;

        while ((scan = inputBuffer.Read()) != -1)
        {
            scanBuffer.Add((char)scan);
        }

        var statement = new string(scanBuffer.ToArray());
        scanBuffer.Clear();

        Assert.AreEqual("t\bp", statement);
        Debug.WriteLine($"ReadBackspace (scan)      :: {statement}");

        Assert.AreEqual("t\bp", consoleOutput.ToString());
        Debug.WriteLine($"ReadBackspace (console)   :: {consoleOutput}");
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

    #region Container Initialization & CleanUp
    [ContainerInitialize]
    public static void ContainerInitialize(TestContext context)
    {
    }
    #endregion Container Initialization & CleanUp
}
