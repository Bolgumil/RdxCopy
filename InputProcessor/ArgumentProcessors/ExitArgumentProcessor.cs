using CliArgumentProcessor.Commands;
using System.Linq;

namespace CliArgumentProcessor.ArgumentProcessors
{
    public class ExitArgumentProcessor : ArgumentProcessorBase
    {
        private string[] exitFlags = { "-e", "--exit" };

        protected override int NumberOfArgumentsRequired => 1;

        public override ICommand GetCommand(string[] args)
        {
            if (!ValidArgumentCount(args) || !exitFlags.Any(ef => ef == args[0]))
                return NotCommandResult();
            else
                return ExitCommandResult();
        }

        public override string GetHelpText()
        {
            return "exit help";
        }

        private ICommand ExitCommandResult()
        {
            return new ExitCommand();
        }
    }
}
