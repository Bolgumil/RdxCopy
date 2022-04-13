using RdxCopy.CopyManager.DTOs;
using System.Collections.Concurrent;

namespace RdxCopy.CopyManager
{
    public class CopyManager
    {
        private ConcurrentDictionary<string, (Task copyTask, ConcurrentQueue<FileCopyDTO> fileQueue)> _copyRepository;

        public CopyManager()
        {
            _copyRepository = new ConcurrentDictionary<string, (Task copyTask, ConcurrentQueue<FileCopyDTO> fileQueue)>();
        }

        public Task StartCopy(string src, string dest, bool replace, bool recurse)
        {
            var copyTask = CopyDirectory(src, dest, replace, recurse);

            Console.WriteLine($"Copying {src} to {dest} in the background...");
            Console.WriteLine(string.Empty);

            return copyTask;
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

            foreach (var batch in filesPerExtension)
            {
                if (_copyRepository.ContainsKey(batch.Key))
                {
                    AppendNewFilesToExtensionQueue(src, dest, replace, batch);
                }
                else
                {
                    StartNewExtensionCopyQueue(src, dest, replace, batch);
                }
            }

            return Task.WhenAll(_copyRepository.Select(x => x.Value.copyTask));
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

        private void StartNewExtensionCopyQueue(string src, string dest, bool replace, KeyValuePair<string, List<FileInfo>> batch)
        {
            var copyTask = new Task(() => CopyFilesSequentially(batch.Key));
            var filesToCopy = batch.Value.Select(file => new FileCopyDTO
            {
                FileInfo = file,
                Src = src,
                Dest = dest,
                Replace = replace
            });

            _copyRepository.TryAdd(
                batch.Key,
                (
                    copyTask: copyTask,
                    fileQueue: new ConcurrentQueue<FileCopyDTO>(filesToCopy)
                ));

            copyTask.Start();
            copyTask.ContinueWith(t => OnCopyTaskSuccess(t, batch.Key), TaskContinuationOptions.OnlyOnRanToCompletion);
            copyTask.ContinueWith(t => OnCopyTaskError(t, batch.Key), TaskContinuationOptions.OnlyOnFaulted);
        }

        private void AppendNewFilesToExtensionQueue(string src, string dest, bool replace, KeyValuePair<string, List<FileInfo>> batch)
        {
            foreach (var file in batch.Value)
            {
                _copyRepository[batch.Key].fileQueue.Enqueue(new FileCopyDTO
                {
                    FileInfo = file,
                    Src = src,
                    Dest = dest,
                    Replace = replace
                });
            }
        }

        private void CopyFilesSequentially(string extension)
        {
            while (_copyRepository.ContainsKey(extension) &&
                    _copyRepository[extension].fileQueue.TryDequeue(out var fileCopyDTO))
            {
                var targetDirectory = fileCopyDTO.FileInfo.DirectoryName.Replace(Path.GetFullPath(fileCopyDTO.Src), Path.GetFullPath(fileCopyDTO.Dest));
                Directory.CreateDirectory(targetDirectory);
                var fileFullPath = Path.Combine(targetDirectory, fileCopyDTO.FileInfo.Name);

                if (File.Exists(fileFullPath))
                {
                    if (fileCopyDTO.Replace) File.Delete(fileFullPath);
                    else continue;
                }

                fileCopyDTO.FileInfo.CopyTo(fileFullPath);
            }
        }

        private void OnCopyTaskSuccess(Task t, string extension)
        {
            _copyRepository.TryRemove(extension, out _);
            Console.WriteLine($"{Environment.NewLine}Copying files with Extension '{extension}' finished!");
            Console.Write(string.Empty);
        }

        private void OnCopyTaskError(Task t, string extension)
        {
            _copyRepository.TryRemove(extension, out _);
            Console.WriteLine($"{Environment.NewLine}Copying files with Extension '{extension}' failed!");
            foreach (var e in t.Exception.InnerExceptions)
            {
                Console.WriteLine($"{e.Message}");
            }
            Console.Write(string.Empty);
        }
    }
}