using CliArgumentProcessor;
using CliArgumentProcessor.Commands;

var argumentProcessor = new ArgumentProcessor();

switch (argumentProcessor.ProcessArguments(args))
{
    case CopyFolderCommand copyFolder:
        Console.WriteLine("Copy folder command");
        break;
    case ExitCommand exit:
        Console.WriteLine("Exit command");
        break;
    case HelpCommand help:
    default:
        Console.WriteLine("Help command");
        break;
}