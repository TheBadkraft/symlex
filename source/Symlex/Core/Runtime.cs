// Last update: 2024-12-16 14:30

using Symlex.IO;

namespace Symlex.Core;

public class Runtime
{
    const string PROMPT = "E:";

    private string DataFilePath { get; init; }
    private string IndexFilePath { get; init; }
    private InputPreprocessor InputPreprocessor { get; init; } = new InputPreprocessor() { Prompt = PROMPT };

    public Runtime(string dataFilePath, string indexFilePath)
    {
        DataFilePath = dataFilePath;
        IndexFilePath = indexFilePath;
    }

    public void Run()
    {
        //  threadpool task
        InputPreprocessor.Run();
    }
}
