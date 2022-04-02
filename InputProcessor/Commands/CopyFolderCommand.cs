namespace CliArgumentProcessor.Commands
{
    public class CopyFolderCommand : ICommand
    {
        public string Source { get; set; }
        public string Destination { get; set; }

        public CopyFolderCommand(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}
