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
            originalOut.WriteLine(String.Format("{0} {1}", ">> ", message));
        }
        public override void Write(string message)
        {
            originalOut.Write(String.Format("{0} {1}", ">> ", message));
        }
    }
}
