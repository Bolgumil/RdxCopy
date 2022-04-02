using NUnit.Framework;
using RdxCopy.Commands;
using RdxCopy.TestHelper;

namespace RdxCopy.CliArgumentProcessor.Tests
{
    public class ArgumentProcessorTests
    {
        [TearDown]
        public static void Cleanup()
        {
            TestFolderManager.DeleteTestFolders();
        }

        [Test]
        [TestCase(new object[] { "" })]
        [TestCase(new object[] { "-x" })]
        [TestCase(new object[] { "-e", "-h" })]
        [TestCase(new object[] { "-d", "-h" })]
        [TestCase(new object[] { "-exit" })]
        [TestCase(new object[] { "-s", "e" })]
        public void ProcessArguments_DefaultCtor_NotCommand(object[] argsObj)
        {
            // Arrange
            var argumentProcessor = new ArgumentProcessor();
            var args = argsObj.ToStringArray();

            // Act
            var result = argumentProcessor.ProcessArguments(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }

        [Test]
        [TestCase(new object[] { "-e" })]
        [TestCase(new object[] { "--exit" })]
        public void ProcessArguments_DefaultCtor_ExitCommand(object[] argsObj)
        {
            // Arrange
            var argumentProcessor = new ArgumentProcessor();
            var args = argsObj.ToStringArray();

            // Act
            var result = argumentProcessor.ProcessArguments(args);

            // Assert
            Assert.IsInstanceOf<ExitCommand>(result);
        }

        [Test]
        [TestCase(new object[] { "-h" })]
        [TestCase(new object[] { "-?" })]
        [TestCase(new object[] { "--help" })]
        public void ProcessArguments_DefaultCtor_HelpCommand(object[] argsObj)
        {
            // Arrange
            var argumentProcessor = new ArgumentProcessor();
            var args = argsObj.ToStringArray();

            // Act
            var result = argumentProcessor.ProcessArguments(args);

            // Assert
            Assert.IsInstanceOf<HelpCommand>(result);
        }

        [Test]
        [TestCase(new object[] { "-s", ".\\src", "-d", ".\\dest" })]
        [TestCase(new object[] { "-d", ".\\dest", "-s", ".\\src" })]
        [TestCase(new object[] { "--dest", ".\\dest", "--src", ".\\src" })]
        [TestCase(new object[] { "--destination", ".\\dest", "--source", ".\\src" })]
        public void ProcessArguments_DefaultCtor_CopyFolderCommand(object[] argsObj)
        {
            // Arrange
            TestFolderManager.CreateTestFolders();
            var argumentProcessor = new ArgumentProcessor();
            var args = argsObj.ToStringArray();

            // Act
            var result = argumentProcessor.ProcessArguments(args);

            // Assert
            Assert.IsInstanceOf<CopyFolderCommand>(result);
            var cfc = result as CopyFolderCommand;
            Assert.AreEqual(".\\dest", cfc.Destination);
            Assert.AreEqual(".\\src", cfc.Source);
        }
    }
}
