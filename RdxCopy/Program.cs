using RdxCopy;
using RdxCopy.CommandManager;
using StringToArgsConverter;

Console.WriteLine("Welcome to RdxCopy!");
Console.SetOut(new PrefixedWriter());

var commandManager = new CommandManager();

if (args != null && args.Length > 0)
{
    commandManager.ProcessInput(args);
}

do
{
    Console.WriteLine("Waiting for next input...");
    Console.Write(string.Empty);
    args = Console.ReadLine().ToArgs();
} while (commandManager.ProcessInput(args));