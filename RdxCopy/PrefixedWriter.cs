using System.Text;

namespace RdxCopy
{
    public class PrefixedWriter : TextWriter
    {
        private TextWriter originalOut;
        public PrefixedWriter()
        {
            originalOut = Console.Out;
        }

        public override Encoding Encoding
        {
            get { return new ASCIIEncoding(); }
        }

        public override void WriteLine(string message)
        {
            if (message.StartsWith(Environment.NewLine))
                originalOut.WriteLine(String.Format("{0}{1} {2}", Environment.NewLine, ">> ", message.Remove(0, Environment.NewLine.Length)));
            else
                originalOut.WriteLine(String.Format("{0} {1}", ">> ", message));
        }

        public override void Write(string message)
        {
            originalOut.Write(String.Format("{0} {1}", ">> ", message));
        }
    }
}
