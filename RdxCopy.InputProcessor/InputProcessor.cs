using RdxCopy.CliArgumentProcessor;
using RdxCopy.CliArgumentProcessor.ArgumentProcessors;
using RdxCopy.Commands;

namespace RdxCopy.InputProcessor
{
    public class InputProcessor
    {
        private readonly ArgumentProcessor _argumentProcessor;
        private readonly CopyManager.CopyManager _copyManager;

        public InputProcessor()
        {
            _argumentProcessor = new ArgumentProcessor();
            _copyManager = new CopyManager.CopyManager();
        }

        public bool ProcessInput(string[] args)
        {
            switch (_argumentProcessor.ProcessArguments(args))
            {
                case CopyFolderCommand copyFolder:
                    return ProcessCopyCommand(copyFolder);
                case ExitCommand _:
                    return ProcessExitCommand();
                case HelpCommand _:
                    return ProcessHelpCommand(_argumentProcessor.Processors);
                case ArgumentErrorCommand err:
                    return ProcessErrorCommand(err.ErrorCode);
                case ClearCommand _:
                    return ProcessClearCommand();
                case EmptyCommand _:
                    return ProcessEmptyCommand();
                case NotCommand _:
                default:
                    return ProcessNotCommand();
            }
        }

        private bool ProcessCopyCommand(CopyFolderCommand copyFolderCommand)
        {
            _copyManager.StartCopy(
                copyFolderCommand.Source, 
                copyFolderCommand.Destination,
                copyFolderCommand.Replace,
                copyFolderCommand.Recurse
            );
            return WaitingForNextInout();
        }

        private bool ProcessExitCommand()
        {
            Console.WriteLine("Exiting RdxCopy...");
            return false;
        }

        private bool ProcessHelpCommand(IEnumerable<ArgumentProcessorBase> argumentProcessors)
        {
            foreach (var processor in argumentProcessors)
            {
                Array.ForEach(
                    processor.GetHelpText().Split(Environment.NewLine),
                    Console.WriteLine
                );
                Console.WriteLine(string.Empty);
            }
            return WaitingForNextInout();
        }

        private bool ProcessErrorCommand(string errorCode)
        {
            Console.WriteLine($"Following error happened: {errorCode}.");
            Console.WriteLine("Try -h [, -?, --help] for information about valid arguments.");
            Console.WriteLine(string.Empty);
            return WaitingForNextInout();
        }

        private bool ProcessClearCommand()
        {
            Console.Clear();
            return WaitingForNextInout();
        }

        private bool ProcessEmptyCommand()
        {
            Console.Write(string.Empty);
            return true;
        }

        private bool ProcessNotCommand()
        {
            Console.WriteLine("No such argument supported. ");
            Console.WriteLine("Try -h [, -?, --help] for information about valid arguments.");
            Console.WriteLine(string.Empty);
            return WaitingForNextInout();
        }

        private bool WaitingForNextInout()
        {
            Console.WriteLine("Waiting for next input...");
            Console.Write(string.Empty);
            return true;
        }
    }
}