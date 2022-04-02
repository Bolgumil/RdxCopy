using RdxCopy.CliArgumentProcessor;
using RdxCopy.Commands;
using System.Text;

namespace RdxCopy.CommandManager
{
    public class CommandManager
    {
        private ArgumentProcessor _argumentProcessor;

        public CommandManager()
        {
            _argumentProcessor = new ArgumentProcessor();
        }

        public bool ProcessInput(string[] args)
        {
            switch (_argumentProcessor.ProcessArguments(args))
            {
                case CopyFolderCommand copyFolder:
                    Console.WriteLine("Copy folder command");
                    return true;
                case ExitCommand _:
                    Console.WriteLine("Exiting RdxCopy...");
                    return false;
                case HelpCommand _:
                    var sb = new StringBuilder();
                    foreach (var processor in _argumentProcessor.Processors)
                    {
                        Array.ForEach(
                            processor.GetHelpText().Split(Environment.NewLine),
                            Console.WriteLine
                        );
                        Console.WriteLine(string.Empty);
                    }
                    return true;
                case ArgumentErrorCommand err:
                    Console.WriteLine($"Following error happened: {err.ErrorCode}.");
                    Console.WriteLine("Try -h [, -?, --help] for information about valid arguments.");
                    Console.WriteLine(string.Empty);
                    return true;
                case NotCommand _:
                default:
                    Console.WriteLine("No such argument supported. ");
                    Console.WriteLine("Try -h [, -?, --help] for information about valid arguments.");
                    Console.WriteLine(string.Empty);
                    return true;
            }
        }
    }
}