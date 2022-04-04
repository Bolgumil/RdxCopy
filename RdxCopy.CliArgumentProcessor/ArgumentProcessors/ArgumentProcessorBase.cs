using RdxCopy.Commands;

namespace RdxCopy.CliArgumentProcessor.ArgumentProcessors
{
    public abstract class ArgumentProcessorBase
    {
        /// <returns>Help text that provides information about the parameters and usage of the concrete ArgumentProcessor</returns>
        public abstract string GetHelpText();
        /// <summary>
        /// Gets an <see cref="ICommand"/> based on the concrete ArgumentProcessor's implementation
        /// </summary>
        /// <param name="args">Input arguments</param>
        public abstract ICommand GetCommand(string[] args);

        protected abstract int MinNumberOfArgumentsRequired { get; }
        protected virtual int? MaxNumberOfArgumentsRequired { get; } = null;

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
            return args != null && 
                ((args.Length == MinNumberOfArgumentsRequired && MaxNumberOfArgumentsRequired == null) ||
                (args.Length >= MinNumberOfArgumentsRequired && args.Length <= MaxNumberOfArgumentsRequired));
        }
    }
}
