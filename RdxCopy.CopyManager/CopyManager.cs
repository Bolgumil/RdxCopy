namespace RdxCopy.CopyManager
{
    public class CopyManager
    {
        public CopyManager()
        {

        }

        public Task StartCopy(string src, string dest)
        {
            Console.WriteLine($"Copying {src} to {dest} in the background...");
            Console.WriteLine(string.Empty);
            var longRunning = Task.Run(() =>
            {
                Thread.Sleep(5000);
                Console.WriteLine($"{Environment.NewLine}Copy {src} to {dest} finished!");
                Console.Write(string.Empty);
            });
            return longRunning;
        }
    }
}