using NUnit.Framework;
using RdxCopy.TestHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RdxCopy.CopyManager.Tests
{
    public class Tests
    {
        private static CopyManager _copyManager;

        [SetUp]
        public static void Init()
        {
            _copyManager = new CopyManager();
            TestResourceManager.CreateDefaultTestDirectories();
            TestResourceManager.CreateDefaultTestFile();
        }

        [TearDown]
        public static void Cleanup()
        {
            TestResourceManager.DeleteDefaultTestFolders();
        }

        [Test]
        public void StartCopy()
        {
            // Arrange

            // Act
            var copyTask = _copyManager.StartCopy(
                TestResourceManager.DefaultSourceDirectory,
                TestResourceManager.DefaultDestinationDirectory,
                false,
                false
            );

            // Assert
            FileAssert.DoesNotExist(Path.Combine(TestResourceManager.DefaultDestinationDirectory, TestResourceManager.DefaultFileFullName));
            copyTask.Wait();
            Assert.IsTrue(copyTask.IsCompleted);
            FileAssert.Exists(Path.Combine(TestResourceManager.DefaultDestinationDirectory, TestResourceManager.DefaultFileFullName));
        }

        #region DiscoverDirectory Tests

        [Test]
        public void DiscoverDirectory_SingleFile()
        {
            // Arrange

            // Act
            var result = _copyManager.DiscoverDirectory(TestResourceManager.DefaultSourceDirectory, true);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.Values.First().Count);
            Assert.AreEqual(TestResourceManager.DefaultFileExtension, result.Keys.First());
            Assert.AreEqual(TestResourceManager.DefaultFileFullName, result.Values.First().First().Name);
        }

        [Test]
        public void DiscoverDirectory_MultipleFiles_SameExtension()
        {
            // Arrange
            var secondFileName = "second_file";
            var secondFileFullName = secondFileName + TestResourceManager.DefaultFileExtension;
            TestResourceManager.CreateFile(Path.Combine(TestResourceManager.DefaultSourceDirectory, secondFileFullName));

            // Act
            var result = _copyManager.DiscoverDirectory(TestResourceManager.DefaultSourceDirectory, true);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result.Values.First().Count);
            Assert.AreEqual(TestResourceManager.DefaultFileExtension, result.Keys.First());
            Assert.AreEqual(TestResourceManager.DefaultFileFullName, result.Values.First().ElementAt(0).Name);
            Assert.AreEqual(secondFileFullName, result.Values.First().ElementAt(1).Name);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void DiscoverDirectory_MultipleFiles_SameExtension_NestedDirectory(bool recurse)
        {
            // Arrange
            var firstNestedFileFullName = "file1" + TestResourceManager.DefaultFileExtension;
            var firstNestedDirectory = ".\\nested_dir_1";
            var secondNestedFileFullName = "file2" + TestResourceManager.DefaultFileExtension;
            var secondNestedDirectory = ".\\nested_dir_2";
            CreateNestedFilesInDefaultSourceDirectory(new List<(string fileFullName, string containingFolder)>
            {
                (firstNestedFileFullName, firstNestedDirectory),
                (secondNestedFileFullName, secondNestedDirectory),
            });

            // Act
            var result = _copyManager.DiscoverDirectory(TestResourceManager.DefaultSourceDirectory, recurse);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(recurse ? 3 : 1, result.Values.First().Count);
            Assert.AreEqual(TestResourceManager.DefaultFileExtension, result.Keys.First());
            Assert.AreEqual(TestResourceManager.DefaultFileFullName, result.Values.First().ElementAt(0).Name);
            if (recurse)
            {
                Assert.AreEqual(firstNestedFileFullName, result.Values.First().ElementAt(1).Name);
                Assert.AreEqual(secondNestedFileFullName, result.Values.First().ElementAt(2).Name);
            }
        }

        [Test]
        public void DiscoverDirectory_MultipleFiles_DifferentExtension()
        {
            // Arrange
            var secondFileName = "second_file";
            var secondFileExtension = ".config";
            var secondFileFullName = secondFileName + secondFileExtension;
            TestResourceManager.CreateFile(Path.Combine(TestResourceManager.DefaultSourceDirectory, secondFileFullName));

            // Act
            var result = _copyManager.DiscoverDirectory(TestResourceManager.DefaultSourceDirectory, true);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey(TestResourceManager.DefaultFileExtension));
            Assert.IsTrue(result.ContainsKey(secondFileExtension));
            Assert.AreEqual(1, result[TestResourceManager.DefaultFileExtension].Count);
            Assert.AreEqual(1, result[secondFileExtension].Count);
            Assert.AreEqual(TestResourceManager.DefaultFileFullName, result[TestResourceManager.DefaultFileExtension].First().Name);
            Assert.AreEqual(secondFileFullName, result[secondFileExtension].First().Name);
        }

        #endregion

        #region CopyFilesSequentially Tests

        [Test]
        public void CopyFilesSequentially_SingleFile()
        {
            // Arrange
            var files = _copyManager.DiscoverDirectory(TestResourceManager.DefaultSourceDirectory, true);

            // Act
            _copyManager.CopyFilesSequentially(
                TestResourceManager.DefaultSourceDirectory,
                TestResourceManager.DefaultDestinationDirectory,
                false,
                files[TestResourceManager.DefaultFileExtension]
            );

            // Assert
            FileAssert.Exists(
                Path.Combine(
                    TestResourceManager.DefaultDestinationDirectory,
                    TestResourceManager.DefaultFileFullName
                )
            );
        }

        [Test]
        [TestCase(false, TestResourceManager.DefaultFileContent)]
        [TestCase(true, "Not default test content")]
        public void CopyFilesSequentially_SingleFile_ExistingDestination(bool recurse, string expectedFileContent)
        {
            // Arrange
            var files = _copyManager.DiscoverDirectory(TestResourceManager.DefaultSourceDirectory, true);
            TestResourceManager.CreateFile(
                Path.Combine(
                    TestResourceManager.DefaultDestinationDirectory,
                    TestResourceManager.DefaultFileFullName
                )
            );
            TestResourceManager.WriteFile(
                Path.Combine(
                    TestResourceManager.DefaultSourceDirectory,
                    TestResourceManager.DefaultFileFullName
                ),
                "Not default test content"
            );

            // Act
            _copyManager.CopyFilesSequentially(
                TestResourceManager.DefaultSourceDirectory,
                TestResourceManager.DefaultDestinationDirectory,
                recurse,
                files[TestResourceManager.DefaultFileExtension]
            );

            // Assert
            FileAssert.Exists(
                Path.Combine(
                    TestResourceManager.DefaultDestinationDirectory,
                    TestResourceManager.DefaultFileFullName
                )
            );
            Assert.AreEqual(
                expectedFileContent, 
                TestResourceManager.ReadFile(
                    Path.Combine(
                        TestResourceManager.DefaultDestinationDirectory,
                        TestResourceManager.DefaultFileFullName
                    )
                )
            );
        }

        [Test]
        public void CopyFilesSequentially_MultipleFiles_SameDirectory()
        {
            // Arrange
            var firstNestedFileFullName = "file1" + TestResourceManager.DefaultFileExtension;
            var secondNestedFileFullName = "file2" + TestResourceManager.DefaultFileExtension;
            var nestedDirectory = ".\\nested_dir";
            CreateNestedFilesInDefaultSourceDirectory(new List<(string fileFullName, string containingFolder)>
            {
                (firstNestedFileFullName, nestedDirectory),
                (secondNestedFileFullName, nestedDirectory),
            });
            var files = _copyManager.DiscoverDirectory(TestResourceManager.DefaultSourceDirectory, true);

            // Act
            _copyManager.CopyFilesSequentially(
                TestResourceManager.DefaultSourceDirectory,
                TestResourceManager.DefaultDestinationDirectory,
                false,
                files[TestResourceManager.DefaultFileExtension]
            );

            // Assert
            FileAssert.Exists(
                Path.Combine(
                    TestResourceManager.DefaultDestinationDirectory,
                    TestResourceManager.DefaultFileFullName
                )
            );
            FileAssert.Exists(
                Path.Combine(
                    TestResourceManager.DefaultDestinationDirectory,
                    nestedDirectory,
                    firstNestedFileFullName
                )
            );
            FileAssert.Exists(
                Path.Combine(
                    TestResourceManager.DefaultDestinationDirectory,
                    nestedDirectory,
                    secondNestedFileFullName
                )
            );
        }

        [Test]
        public void CopyFilesSequentially_MultipleFiles_DifferentNestedDirectory()
        {
            // Arrange
            var firstNestedFileFullName = "file1" + TestResourceManager.DefaultFileExtension;
            var firstNestedDirectory = ".\\nested_dir_1";
            var secondNestedFileFullName = "file2" + TestResourceManager.DefaultFileExtension;
            var secondNestedDirectory = ".\\nested_dir_2";
            CreateNestedFilesInDefaultSourceDirectory(new List<(string fileFullName, string containingFolder)>
            {
                (firstNestedFileFullName, firstNestedDirectory),
                (secondNestedFileFullName, secondNestedDirectory),
            });
            var files = _copyManager.DiscoverDirectory(TestResourceManager.DefaultSourceDirectory, true);

            // Act
            _copyManager.CopyFilesSequentially(
                TestResourceManager.DefaultSourceDirectory,
                TestResourceManager.DefaultDestinationDirectory,
                false,
                files[TestResourceManager.DefaultFileExtension]
            );

            // Assert
            FileAssert.Exists(
                Path.Combine(
                    TestResourceManager.DefaultDestinationDirectory,
                    TestResourceManager.DefaultFileFullName
                )
            );
            FileAssert.Exists(
                Path.Combine(
                    TestResourceManager.DefaultDestinationDirectory,
                    firstNestedDirectory,
                    firstNestedFileFullName
                )
            );
            FileAssert.Exists(
                Path.Combine(
                    TestResourceManager.DefaultDestinationDirectory,
                    secondNestedDirectory,
                    secondNestedFileFullName
                )
            );
        }

        #endregion

        #region Test helpers

        private void CreateNestedFilesInDefaultSourceDirectory(List<(string fileFullName, string containingFolder)> filesAndContainingDirectories)
        {
            foreach ((string fileFullName, string containingFolder) in filesAndContainingDirectories)
            {
                TestResourceManager.CreateDirectory(Path.Combine(TestResourceManager.DefaultSourceDirectory, containingFolder));
                TestResourceManager.CreateFile(Path.Combine(TestResourceManager.DefaultSourceDirectory, containingFolder, fileFullName));
            }
        }

        #endregion
    }
}