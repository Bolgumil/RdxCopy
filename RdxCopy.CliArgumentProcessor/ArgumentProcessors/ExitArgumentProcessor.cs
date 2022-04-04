using RdxCopy.Commands;

namespace RdxCopy.CliArgumentProcessor.ArgumentProcessors
{
    /// <summary>
    /// Translates the exit (-e [, --exit]) argument into <see cref="ExitCommand"/>.<br/>
    /// </summary>
    public class ExitArgumentProcessor : ArgumentProcessorBase
    {
        private string[] exitFlags = { "-e", "--exit" };

        protected override int MinNumberOfArgumentsRequired => 1;

        /// <summary>
        /// Converts the incoming exit argument to a <see cref="ExitCommand"/>
        /// </summary>
        /// <param name="args">
        /// String array of the following arguments:
        /// -e [, --exit]: Exit argument.<br/>
        /// </param>
        /// <returns>
        /// <see cref="ExitCommand"/> if a valid exit argument is provided.<br/>
        /// <see cref="NotCommand"/> if valid exit argument could not be found.
        /// </returns>
        public override ICommand GetCommand(string[] args)
        {
            if (!ValidArgumentCount(args) || !exitFlags.Any(ef => ef == args[0]))
                return NotCommandResult();
            else
                return ExitCommandResult();
        }

        public override string GetHelpText()
        {
            return
                "To exit the program:" + Environment.NewLine +
                "  -e: Exits the program." + Environment.NewLine +
                "      Alias: --exit";
        }

        private ICommand ExitCommandResult()
        {
            return new ExitCommand();
        }
    }
}
