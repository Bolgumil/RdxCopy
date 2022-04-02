using CliArgumentProcessor.ArgumentProcessors;
using CliArgumentProcessor.Commands;
using NUnit.Framework;
using TestHelper;

namespace CliArgumentProcessor.Tests.ConcreteArgumentProcessorTests
{
    public class ExitArgumentProcessorTests
    {
        private static ExitArgumentProcessor _exitArgumentProcessor;

        [OneTimeSetUp]
        public static void Init()
        {
            _exitArgumentProcessor = new ExitArgumentProcessor();
        }

        [Test]
        public void GetCommand_NotCommand_Null()
        {
            // Arrange
            string[] args = null;

            // Act
            var result = _exitArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }

        [Test]
        public void GetCommand_NotCommand_Empty()
        {
            // Arrange
            var args = System.Array.Empty<string>();

            // Act
            var result = _exitArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }

        [Test]
        [TestCase(new object[] { "" })]
        [TestCase(new object[] { "-s" })]
        [TestCase(new object[] { "-e", "-e" })]
        [TestCase(new object[] { "--exit", "-e" })]
        [TestCase(new object[] { "-exit" })]
        [TestCase(new object[] { "--e" })]
        public void GetCommand_NotCommands(object[] argsObj)
        {
            // Arrange
            var args = argsObj.ToStringArray();

            // Act
            var result = _exitArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }

        [Test]
        [TestCase(new object[] { "-e" })]
        [TestCase(new object[] { "--exit" })]
        public void GetCommand(object[] argsObj)
        {
            // Arrange
            var args = argsObj.ToStringArray();

            // Act
            var result = _exitArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<ExitCommand>(result);
        }
    }
}
