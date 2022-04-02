using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHelper
{
    public static class TestFolderManager
    {
        public static void CreateTestFolders()
        {
            CreateTestDestinationFolder();
            CreateTestSourceFolder();
        }

        public static void CreateTestDestinationFolder()
        {
            var dir = ".\\dest";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }

        public static void CreateTestSourceFolder()
        {
            var dir = ".\\src";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }

        public static void DeleteTestFolders()
        {
            DeleteTestDestinationFolder();
            DeleteTestSourceFolder();
        }                  
                           
        public static void DeleteTestDestinationFolder()
        {
            var dir = ".\\dest";
            if (Directory.Exists(dir)) Directory.Delete(dir);
        }

        public static void DeleteTestSourceFolder()
        {
            var dir = ".\\src";
            if (Directory.Exists(dir)) Directory.Delete(dir);
        }
    }
}
