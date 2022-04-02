using RdxCopy;
using RdxCopy.CommandManager;

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
    args = Console.ReadLine().Split(' ');
} while (commandManager.ProcessInput(args));

