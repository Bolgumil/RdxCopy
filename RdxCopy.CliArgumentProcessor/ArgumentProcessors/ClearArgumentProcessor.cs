using RdxCopy.Commands;

namespace RdxCopy.CliArgumentProcessor.ArgumentProcessors
{
    /// <summary>
    /// Translates the help (-c [, --clear]) argument into <see cref="ClearCommand"/>.<br/>
    /// </summary>
    public class ClearArgumentProcessor : ArgumentProcessorBase
    {
        private string[] clearFlags = { "-c", "--clear" };

        protected override int NumberOfArgumentsRequired => 1;

        /// <summary>
        /// Converts the incoming clear argument to a <see cref="ClearCommand"/>
        /// </summary>
        /// <param name="args">
        /// String array of the following arguments:
        /// -c [, --clear]: Exit argument.<br/>
        /// </param>
        /// <returns>
        /// <see cref="ClearCommand"/> if a valid clear argument is provided.<br/>
        /// <see cref="NotCommand"/> if valid clear argument could not be found.
        /// </returns>
        public override ICommand GetCommand(string[] args)
        {
            if (!ValidArgumentCount(args) || !clearFlags.Any(ef => ef == args[0]))
                return NotCommandResult();
            else
                return ClearCommandResult();
        }

        public override string GetHelpText()
        {
            return
                "To clear the console:" + Environment.NewLine +
                "  -c: Clears the console." + Environment.NewLine +
                "      Alias: --clear";
        }

        private ICommand ClearCommandResult()
        {
            return new ClearCommand();
        }
    }
}
