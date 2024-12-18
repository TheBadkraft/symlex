// See https://aka.ms/new-console-template for more information

using Symlex.Core;

const string dataFilePath = "functions.data";
const string indexFilePath = "functions.index";

Console.WriteLine("Welcome to Symlex Console! Type 'exit' to quit, 'save' to save functions.");
Runtime runtime = new Runtime(dataFilePath, indexFilePath);

//  When runtime is launched, we are ready to begin processing input
runtime.Run();

Console.WriteLine("Goodbye!");
