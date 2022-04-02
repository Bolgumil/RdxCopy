using CliArgumentProcessor.ArgumentProcessors;
using CliArgumentProcessor.Commands;
using NUnit.Framework;
using TestHelper;

namespace CliArgumentProcessor.Tests2.ConcreteArgumentProcessorTests
{
    public class CopyFolderArgumentProcessorTests
    {
        private static CopyFolderArgumentProcessor _copyFolderArgumentProcessor;

        [OneTimeSetUp]
        public static void Init()
        {
            _copyFolderArgumentProcessor = new CopyFolderArgumentProcessor();
            TestFolderManager.CreateTestFolders();
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            TestFolderManager.DeleteTestFolders();
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
        [TestCase(new object[] { "-s", "test" })]
        [TestCase(new object[] { "-s", ".\\" })]
        [TestCase(new object[] { "-s", ".\\", "-d" })]
        [TestCase(new object[] { "-d", ".\\", "-x", "-s", ".\\" })]
        [TestCase(new object[] { "-destination", ".\\dest", "-source", ".\\src" })]
        [TestCase(new object[] { "--s", ".\\src", "--d", ".\\dest" })]
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
        [TestCase(new object[] { "-d", ".\\", "-x", ".\\" }, "source_required")]
        [TestCase(new object[] { "-x", ".\\", "-s", ".\\" }, "destination_required")]
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
        [TestCase(new object[] { "-s", ".\\src", "-d", ".\\dest" }, "invalid_source_path")]
        public void GetCommand_ArgumentErrors_PathDoesNotExist(object[] argsObj, string errorCode)
        {
            // Arrange
            TestFolderManager.DeleteTestFolders();
            var args = argsObj.ToStringArray();

            // Act
            var result = _copyFolderArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<ArgumentErrorCommand>(result);
            var aec = result as ArgumentErrorCommand;
            Assert.AreEqual(errorCode, aec.ErrorCode);

            // Cleanup
            TestFolderManager.CreateTestFolders();
        }

        [Test]
        [TestCase(new object[] { "-s", ".\\src", "-d", ".\\dest" })]
        [TestCase(new object[] { "-d", ".\\dest", "-s", ".\\src" })]
        [TestCase(new object[] { "--dest", ".\\dest", "--src", ".\\src" })]
        [TestCase(new object[] { "--destination", ".\\dest", "--source", ".\\src" })]
        public void GetCommand(object[] argsObj)
        {
            // Arrange
            var args = argsObj.ToStringArray();

            // Act
            var result = _copyFolderArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<CopyFolderCommand>(result);
            var cfc = result as CopyFolderCommand;
            Assert.AreEqual(".\\dest", cfc.Destination);
            Assert.AreEqual(".\\src", cfc.Source);
        }
    }
}
