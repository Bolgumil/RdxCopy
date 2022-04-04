using System.Text.RegularExpressions;

namespace RdxCopy.CopyManager
{
    public class CopyManager
    {
        public CopyManager()
        {

        }

        public Task StartCopy(string src, string dest, bool replace, bool recurse)
        {
            Console.WriteLine($"Copying {src} to {dest} in the background...");
            Console.WriteLine(string.Empty);
            return Task.Run(async () =>
            {
                await CopyDirectory(src, dest, replace, recurse);
                Console.WriteLine($"{Environment.NewLine}Copy {src} to {dest} finished!");
                Console.Write(string.Empty);
            });
        }

        /// <summary>
        /// Copies files within a directory recursively.<br/>
        /// Source: https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        /// </summary>
        /// <param name="src">Source folder</param>
        /// <param name="dest">Destination folder</param>
        /// <param name="recursive">Copy files within subdirectories</param>
        /// <exception cref="DirectoryNotFoundException">Thrown when source directory not found.</exception>
        public Task CopyDirectory(string src, string dest, bool replace, bool recurse)
        {
            var srcDir = new DirectoryInfo(src);

            if (!srcDir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {srcDir.FullName}");

            var filesPerExtension = DiscoverDirectory(src, recurse);
            
            var copyTasks = new List<Task>();
            foreach (var files in filesPerExtension.Values)
            {
                copyTasks.Add(Task.Run(() => {
                    CopyFilesSequentially(src, dest, replace, files);
                }));
            }

            return Task.WhenAll(copyTasks);
        }

        /// <summary>
        /// Discover the directory and returns back files grouped together based on their extension
        /// </summary>
        /// <param name="src">The source directory where the files will be searched</param>
        /// <param name="recursive">Tells if nested directories should be looked for files or not</param>
        /// <returns>List of files grouped by their extensions, organised in a Dictionary.<br/>
        /// Key: File extension<br/>
        /// Value: List of FileInfo
        /// </returns>
        public Dictionary<string, List<FileInfo>> DiscoverDirectory(string src, bool recursive)
        {
            var result = new Dictionary<string, List<FileInfo>>();
            var srcDir = new DirectoryInfo(src);

            foreach (FileInfo file in srcDir.GetFiles())
            {
                if (result.ContainsKey(file.Extension))
                    result[file.Extension].Add(file);
                else
                    result[file.Extension] = new List<FileInfo> { file };
            }

            if (recursive)
            {
                var subDirs = srcDir.GetDirectories();
                foreach (DirectoryInfo subDir in subDirs)
                {
                    result.MergeDictionaries(DiscoverDirectory(subDir.FullName, true));
                }
            }

            return result;
        }

        public void CopyFilesSequentially(string src, string dest, bool replace, List<FileInfo> files)
        {
            foreach (var file in files)
            {
                var targetDirectory = file.DirectoryName.Replace(Path.GetFullPath(src), Path.GetFullPath(dest));
                Directory.CreateDirectory(targetDirectory);
                var fileFullPath = Path.Combine(targetDirectory, file.Name);

                if (File.Exists(fileFullPath))
                {
                    if (replace) File.Delete(fileFullPath);
                    else continue;
                }

                file.CopyTo(fileFullPath);
            }
        }
    }
}