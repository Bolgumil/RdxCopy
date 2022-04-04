using System.Text;

namespace RdxCopy.TestHelper
{
    public static class TestResourceManager
    {
        public const string DefaultSourceDirectory = ".\\src";
        public const string DefaultDestinationDirectory = ".\\dest";

        public const string DefaultFileName = "default_test_file";
        public const string DefaultFileExtension = ".txt";

        public const string DefaultFileContent = "Test content";

        public static string DefaultFileFullName => DefaultFileName + DefaultFileExtension;

        public static void CreateDefaultTestDirectories()
        {
            CreateTestDestinationDirectory();
            CreateTestSourceDirectory();
        }

        public static void CreateDefaultTestFile()
        {
            CreateFile(Path.Combine(DefaultSourceDirectory, DefaultFileFullName));
        }

        public static void CreateTestDestinationDirectory()
        {
            CreateDirectory(DefaultDestinationDirectory);
        }

        public static void CreateTestSourceDirectory()
        {
            CreateDirectory(DefaultSourceDirectory);
        }

        public static void CreateDirectory(string dir)
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }

        public static void DeleteDefaultTestFolders()
        {
            DeleteTestDestinationDirectory();
            DeleteTestSourceDirectory();
        }                  
                           
        public static void DeleteTestDestinationDirectory()
        {
            DeleteDirectory(DefaultDestinationDirectory);
        }

        public static void DeleteTestSourceDirectory()
        {
            DeleteDirectory(DefaultSourceDirectory);
        }

        public static void DeleteDirectory(string dir)
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
        }

        public static void CreateFile(string fullPath, string fileContent = DefaultFileContent)
        {
            using (FileStream fs = File.Create(fullPath))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(fileContent);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

        public static void WriteFile(string fullPath, string fileContent = DefaultFileContent)
        {
            using (FileStream fs = File.Open(fullPath, FileMode.OpenOrCreate))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(fileContent);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

        public static string ReadFile(string fullPath)
        {
            return File.ReadAllText(fullPath, Encoding.UTF8);
        }
    }
}
