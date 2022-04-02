namespace RdxCopy.Commands
{
    public class ArgumentErrorCommand : ICommand
    {
        public string ErrorCode { get; set; }

        public ArgumentErrorCommand(string errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
