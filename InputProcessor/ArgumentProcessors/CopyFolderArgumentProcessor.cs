using CliArgumentProcessor.Commands;
using System.IO;
using System.Linq;

namespace CliArgumentProcessor.ArgumentProcessors
{
    public class CopyFolderArgumentProcessor : ArgumentProcessorBase
    {
        private string[] sourceFlags = { "-s", "--src", "--source" };
        private string[] destinationFlags = { "-d", "--dest", "--destination" };

        protected override int NumberOfArgumentsRequired => 4;

        public override ICommand GetCommand(string[] args)
        {
            if (!ValidArgumentCount(args)) return NotCommandResult();

            var src = string.Empty;
            var dest = string.Empty;

            for (var i = 0; i < args.Length; i++)
            {
                if (sourceFlags.Any(sf => sf == args[i]))
                {
                    if (CheckIfNextArgIsDirectory(args, i))
                    {
                        src = args[i + 1];
                        i++;
                        continue;
                    }

                    return ArgumentErrorCommandResult("invalid_source_path");
                }
                else if (destinationFlags.Any(df => df == args[i]))
                {
                    if (CheckIfNextArgIsDirectory(args, i))
                    {
                        dest = args[i + 1];
                        i++;
                        continue;
                    }

                    return ArgumentErrorCommandResult("invalid_destination_path");
                }
            }

            return (string.IsNullOrEmpty(src), string.IsNullOrEmpty(dest)) switch
            {
                (true, true) => NotCommandResult(),
                (true, false) => ArgumentErrorCommandResult("source_required"),
                (false, true) => ArgumentErrorCommandResult("destination_required"),
                (false, false) => CopyFolderCommandResult(src, dest)
            };
        }

        public override string GetHelpText()
        {
            return "copy help";
        }

        private bool CheckIfNextArgIsDirectory(string[] args, int i)
        {
            return args.Length >= i + 2 && Directory.Exists(args[i + 1]);
        }

        private ICommand CopyFolderCommandResult(string src, string dest)
        {
            return new CopyFolderCommand(src, dest);
        }
    }
}
