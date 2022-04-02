using CliArgumentProcessor.Commands;

namespace CliArgumentProcessor.ArgumentProcessors
{
    public abstract class ArgumentProcessorBase
    {
        public abstract string GetHelpText();
        public abstract ICommand GetCommand(string[] args);

        protected abstract int NumberOfArgumentsRequired { get; }

        protected ICommand ArgumentErrorCommandResult(string errorCode)
        {
            return new ArgumentErrorCommand(errorCode);
        }

        protected ICommand NotCommandResult()
        {
            return new NotCommand();
        }

        protected bool ValidArgumentCount(string[] args)
        {
            return args != null && args.Length == NumberOfArgumentsRequired;
        }
    }
}
