using RdxCopy;
using RdxCopy.InputProcessor;
using StringToArgsConverter;

Console.WriteLine("Welcome to RdxCopy! Type -h for help.");
Console.SetOut(new PrefixedWriter());
Console.Write(string.Empty);

var inputProcessor = new InputProcessor();

if (args != null && args.Length > 0)
{
    inputProcessor.ProcessInput(args);
}

do
{
    args = Console.ReadLine().ToArgs();
} while (inputProcessor.ProcessInput(args));