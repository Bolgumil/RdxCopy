using RdxCopy.Commands;

namespace RdxCopy.CliArgumentProcessor.ArgumentProcessors
{
    /// <summary>
    /// Translates an empty argument array into <see cref="EmptyCommand"/>.<br/>
    /// </summary>
    public class EmptyArgumentProcessor : ArgumentProcessorBase
    {
        protected override int NumberOfArgumentsRequired => 0;

        /// <summary>
        /// Converts the incoming empty argument array to an <see cref="EmptyCommand"/>
        /// </summary>
        /// <param name="args">
        /// Empty array
        /// </param>
        /// <returns>
        /// <see cref="EmptyCommand"/> if an empty array is received<br/>
        /// <see cref="NotCommand"/> if not empty array is received.
        /// </returns>
        public override ICommand GetCommand(string[] args)
        {
            if (!ValidArgumentCount(args))
                return NotCommandResult();
            else
                return EmptyCommandResult();
        }

        public override string GetHelpText()
        {
            return string.Empty;
        }

        private ICommand EmptyCommandResult()
        {
            return new EmptyCommand();
        }
    }
}
