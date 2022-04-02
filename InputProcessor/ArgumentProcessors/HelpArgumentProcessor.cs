using CliArgumentProcessor.Commands;
using System.Linq;

namespace CliArgumentProcessor.ArgumentProcessors
{
    public class HelpArgumentProcessor : ArgumentProcessorBase
    {
        private string[] helpFlags = { "-h", "--help", "-?" };
        
        protected override int NumberOfArgumentsRequired => 1;

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
