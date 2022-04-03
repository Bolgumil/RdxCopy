using RdxCopy;
using RdxCopy.InputProcessor;
using StringToArgsConverter;

Console.WriteLine("Welcome to RdxCopy!");
Console.SetOut(new PrefixedWriter());

var inputProcessor = new InputProcessor();

if (args != null && args.Length > 0)
{
    inputProcessor.ProcessInput(args);
}

do
{
    Console.WriteLine("Waiting for next input...");
    Console.Write(string.Empty);
    args = Console.ReadLine().ToArgs();
} while (inputProcessor.ProcessInput(args));