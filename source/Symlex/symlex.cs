// See https://aka.ms/new-console-template for more information

string prompt = "S>";
string dataFilePath = "functions.data";
string indexFilePath = "functions.index";

// Runtime runtime = new Runtime(dataFilePath, indexFilePath);

Console.WriteLine("Welcome to Symlex Console! Type 'exit' to quit, 'save' to save functions.");

while (true)
{
    Console.Write(prompt);

    // Redirect standard input to a StreamReader
    using (var sr = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
    {
        StringBuilder multiLineInput = new StringBuilder();
        string line;

        do
        {
            line = sr.ReadLine(); // Read one line at a time

            if (line == null) // If EOF or user signals end of input
                break;

            multiLineInput.AppendLine(line);

        } while (!line.TrimEnd().EndsWith("]"));

        string input = multiLineInput.ToString().Trim();

        if (input.ToLower() == "[exit]")
        {
            return; // Exit the program
        }
        else if (input.ToLower() == "[save]")
        {
            // runtime.SaveFunctionsToFile(dataFilePath, indexFilePath);
            Console.WriteLine("Functions saved to file.");
        }
        else if (input.Length > 0)
        {
            // runtime.Execute(input);
        }
    }
}

Console.WriteLine("Goodbye!");
