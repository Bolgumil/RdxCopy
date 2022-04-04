using NUnit.Framework;
using RdxCopy.CliArgumentProcessor.ArgumentProcessors;
using RdxCopy.Commands;
using RdxCopy.TestHelper;

namespace RdxCopy.CliArgumentProcessor.Tests.ConcreteArgumentProcessorTests
{
    public class CopyFolderArgumentProcessorTests
    {
        private static CopyFolderArgumentProcessor _copyFolderArgumentProcessor;

        [OneTimeSetUp]
        public static void Init()
        {
            _copyFolderArgumentProcessor = new CopyFolderArgumentProcessor();
            TestResourceManager.CreateDefaultTestDirectories();
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            TestResourceManager.DeleteDefaultTestFolders();
        }

        [Test]
        public void GetCommand_NotCommand_Null()
        {
            // Arrange
            string[] args = null;

            // Act
            var result = _copyFolderArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }

        [Test]
        public void GetCommand_NotCommand_Empty()
        {
            // Arrange
            var args = System.Array.Empty<string>();

            // Act
            var result = _copyFolderArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }

        [Test]
        [TestCase(new object[] { "" })]
        [TestCase(new object[] { "-s" })]
        [TestCase(new object[] { "-o" })]
        [TestCase(new object[] { "-r" })]
        [TestCase(new object[] { "-r", "-o" })]
        [TestCase(new object[] { "-s", "test" })]
        [TestCase(new object[] { "-s", "test" })]
        [TestCase(new object[] { "-s", "test" })]
        [TestCase(new object[] { "-s", ".\\" })]
        [TestCase(new object[] { "-s", ".\\", "-d" })]
        [TestCase(new object[] { "-d", ".\\", "-x", "-s", ".\\" })]
        [TestCase(new object[] { "-x", ".\\", "-s", ".\\" })]
        [TestCase(new object[] { "-destination", TestResourceManager.DefaultDestinationDirectory, "-source", TestResourceManager.DefaultSourceDirectory })]
        [TestCase(new object[] { "--s", TestResourceManager.DefaultSourceDirectory, "--d", TestResourceManager.DefaultDestinationDirectory })]
        public void GetCommand_NotCommands(object[] argsObj)
        {
            // Arrange
            var args = argsObj.ToStringArray();

            // Act
            var result = _copyFolderArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }

        [Test]
        [TestCase(new object[] { "-s", ".\\", "-d", "test" }, "invalid_destination_path")]
        [TestCase(new object[] { "-d", ".\\", "-s", "test" }, "invalid_source_path")]
        [TestCase(new object[] { "-d", ".\\", "-s", ".\\nonexistentfolder" }, "invalid_source_path")]
        [TestCase(new object[] { "-s", "-o", "-s", ".\\" }, "invalid_source_path")]
        [TestCase(new object[] { "-s", ".\\", "-d", "-r" }, "invalid_destination_path")]
        public void GetCommand_ArgumentErrors(object[] argsObj, string errorCode)
        {
            // Arrange
            var args = argsObj.ToStringArray();

            // Act
            var result = _copyFolderArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<ArgumentErrorCommand>(result);
            var aec = result as ArgumentErrorCommand;
            Assert.AreEqual(errorCode, aec.ErrorCode);
        }

        [Test]
        [TestCase(new object[] { "-s", TestResourceManager.DefaultSourceDirectory, "-d", TestResourceManager.DefaultDestinationDirectory }, "invalid_source_path")]
        public void GetCommand_ArgumentErrors_PathDoesNotExist(object[] argsObj, string errorCode)
        {
            // Arrange
            TestResourceManager.DeleteDefaultTestFolders();
            var args = argsObj.ToStringArray();

            // Act
            var result = _copyFolderArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<ArgumentErrorCommand>(result);
            var aec = result as ArgumentErrorCommand;
            Assert.AreEqual(errorCode, aec.ErrorCode);

            // Cleanup
            TestResourceManager.CreateDefaultTestDirectories();
        }

        [Test]
        [TestCase(new object[] { "-s", TestResourceManager.DefaultSourceDirectory, "-d", TestResourceManager.DefaultDestinationDirectory })]
        [TestCase(new object[] { "-d", TestResourceManager.DefaultDestinationDirectory, "-s", TestResourceManager.DefaultSourceDirectory })]
        [TestCase(new object[] { "--dest", TestResourceManager.DefaultDestinationDirectory, "--src", TestResourceManager.DefaultSourceDirectory })]
        [TestCase(new object[] { "--destination", TestResourceManager.DefaultDestinationDirectory, "--source", TestResourceManager.DefaultSourceDirectory })]
        public void GetCommand(object[] argsObj)
        {
            // Arrange
            var args = argsObj.ToStringArray();

            // Act
            var result = _copyFolderArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<CopyFolderCommand>(result);
            var cfc = result as CopyFolderCommand;
            Assert.AreEqual(TestResourceManager.DefaultDestinationDirectory, cfc.Destination);
            Assert.AreEqual(TestResourceManager.DefaultSourceDirectory, cfc.Source);
        }

        [Test]
        [TestCase(new object[] { "-s", TestResourceManager.DefaultSourceDirectory, "-d", TestResourceManager.DefaultDestinationDirectory }, false, false)]
        [TestCase(new object[] { "-s", TestResourceManager.DefaultSourceDirectory, "-d", TestResourceManager.DefaultDestinationDirectory, "-o" }, false, true)]
        [TestCase(new object[] { "-s", TestResourceManager.DefaultSourceDirectory, "-d", TestResourceManager.DefaultDestinationDirectory, "--override" }, false, true)]
        [TestCase(new object[] { "-s", TestResourceManager.DefaultSourceDirectory, "-d", TestResourceManager.DefaultDestinationDirectory, "-r" }, true, false)]
        [TestCase(new object[] { "-s", TestResourceManager.DefaultSourceDirectory, "-d", TestResourceManager.DefaultDestinationDirectory, "--recurse" }, true, false)]
        [TestCase(new object[] { "-r", "-s", TestResourceManager.DefaultSourceDirectory, "-d", TestResourceManager.DefaultDestinationDirectory, "-o" }, true, true)]
        [TestCase(new object[] { "-s", TestResourceManager.DefaultSourceDirectory, "-r", "-d", TestResourceManager.DefaultDestinationDirectory, "-o" }, true, true)]
        public void GetCommand_WithOptionalOperators(object[] argsObj, bool recurse, bool replace)
        {
            // Arrange
            var args = argsObj.ToStringArray();

            // Act
            var result = _copyFolderArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<CopyFolderCommand>(result);
            var cfc = result as CopyFolderCommand;
            Assert.AreEqual(TestResourceManager.DefaultDestinationDirectory, cfc.Destination);
            Assert.AreEqual(TestResourceManager.DefaultSourceDirectory, cfc.Source);
            Assert.AreEqual(recurse, cfc.Recurse);
            Assert.AreEqual(replace, cfc.Replace);
        }
    }
}
