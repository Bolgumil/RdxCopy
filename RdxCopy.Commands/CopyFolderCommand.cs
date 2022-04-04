namespace RdxCopy.Commands
{
    public class CopyFolderCommand : ICommand
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public bool Replace { get; set; }
        public bool Recurse { get; set; }

        public CopyFolderCommand(string source, string destination, bool replace, bool recurse)
        {
            Source = source;
            Destination = destination;
            Replace = replace;
            Recurse = recurse;
        }
    }
}
