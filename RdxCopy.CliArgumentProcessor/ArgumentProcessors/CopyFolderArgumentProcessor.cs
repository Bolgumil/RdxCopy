using RdxCopy.Commands;

namespace RdxCopy.CliArgumentProcessor.ArgumentProcessors
{
    /// <summary>
    /// Translates the source (-s [, --src, --source]) and destination (-d [,--dest, --source]) arguments into <see cref="CopyFolderCommand"/>.<br/>
    /// Parameter for both flags must be a valid and existing directory.
    /// </summary>
    public class CopyFolderArgumentProcessor : ArgumentProcessorBase
    {
        private string[] sourceFlags = { "-s", "--src", "--source" };
        private string[] destinationFlags = { "-d", "--dest", "--destination" };

        protected override int NumberOfArgumentsRequired => 4;

        /// <summary>
        /// Converts the incoming source and destination flags to a <see cref="CopyFolderCommand"/>
        /// </summary>
        /// <param name="args">
        /// String array of the following arguments:
        /// -s [, --src, --source]: Source directory for the copy operation. Path to the source directory must follow the argument within the array.<br/>
        /// -d [,--dest, --source]: Destination directory for the copy operation. Path to the destination directory must follow the argument within the array.<br/>
        /// Both of the argument must be present.
        /// </param>
        /// <returns>
        /// <see cref="CopyFolderCommand"/> if both source and destination was specified both with valid and existing path to a directory.<br/>
        /// <see cref="ArgumentErrorCommand"/> if either source or destination is missing or points to an invalid or nonexistent directory.<br/>
        /// <see cref="NotCommand"/> if neither source nor destination could be found within the specified arguments.
        /// </returns>
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
