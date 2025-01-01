
using Synaptic.IO;
using Synaptic.Core;

const string dataFilePath = "functions.data";

using (Runtime runtime = new Runtime())
{
    var terminal = SynapticHub.Instance.Resources.GetResource<ITerminal>();
    terminal.Prompt("Welcome to Synaptic Console! Type 'exit' to quit, 'save' to save functions.");
    if (args.Length == 1 && args[0] == "/i")
    {
        if (!File.Exists(dataFilePath))
        {
            terminal.WriteErr($"File not found: {dataFilePath}");
            goto EXIT;
        }

        terminal.WriteLn("Core function installation is not implemented");
        goto EXIT;
    }

    //  When runtime is launched, we are ready to begin processing input
    runtime.Launch();

EXIT:
    terminal.Write("\n   ... Goodbye!");
}

