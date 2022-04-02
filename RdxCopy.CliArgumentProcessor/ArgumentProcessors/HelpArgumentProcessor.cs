using RdxCopy.Commands;

namespace RdxCopy.CliArgumentProcessor.ArgumentProcessors
{
    /// <summary>
    /// Translates the help (-h [, -?, --help]) argument into <see cref="HelpCommand"/>.<br/>
    /// </summary>
    public class HelpArgumentProcessor : ArgumentProcessorBase
    {
        private string[] helpFlags = { "-h", "--help", "-?" };
        
        protected override int NumberOfArgumentsRequired => 1;

        /// <summary>
        /// Converts the incoming help argument to a <see cref="HelpCommand"/>
        /// </summary>
        /// <param name="args">
        /// String array of the following arguments:
        /// -h [, -?, --help]: Help argument.<br/>
        /// </param>
        /// <returns>
        /// <see cref="HelpCommand"/> if a valid help argument is provided.<br/>
        /// <see cref="NotCommand"/> if valid help argument could not be found.
        /// </returns>
        public override ICommand GetCommand(string[] args)
        {
            if (!ValidArgumentCount(args) || !helpFlags.Any(ef => ef == args[0]))
                return NotCommandResult();
            else
                return HelpCommandResult();
        }

        public override string GetHelpText()
        {
            return "help help";
        }

        private ICommand HelpCommandResult()
        {
            return new HelpCommand();
        }
    }
}
