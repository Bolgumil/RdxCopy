using CliArgumentProcessor.ArgumentProcessors;
using CliArgumentProcessor.Commands;
using NUnit.Framework;
using TestHelper;

namespace CliArgumentProcessor.Tests.ConcreteArgumentProcessorTests
{
    public class HelpArgumentProcessorTests
    {
        private static HelpArgumentProcessor _helpArgumentProcessor;

        [OneTimeSetUp]
        public static void Init()
        {
            _helpArgumentProcessor = new HelpArgumentProcessor();
        }

        [Test]
        public void GetCommand_NotCommand_Null()
        {
            // Arrange
            string[] args = null;

            // Act
            var result = _helpArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }

        [Test]
        public void GetCommand_NotCommand_Empty()
        {
            // Arrange
            var args = System.Array.Empty<string>();

            // Act
            var result = _helpArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }

        [Test]
        [TestCase(new object[] { "" })]
        [TestCase(new object[] { "-x" })]
        [TestCase(new object[] { "-h", "-?" })]
        [TestCase(new object[] { "--help", "-?" })]
        [TestCase(new object[] { "-help" })]
        [TestCase(new object[] { "--?" })]
        public void GetCommand_NotCommands(object[] argsObj)
        {
            // Arrange
            var args = argsObj.ToStringArray();

            // Act
            var result = _helpArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }

        [Test]
        [TestCase(new object[] { "-h" })]
        [TestCase(new object[] { "-?" })]
        [TestCase(new object[] { "--help" })]
        public void GetCommand(object[] argsObj)
        {
            // Arrange
            var args = argsObj.ToStringArray();

            // Act
            var result = _helpArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<HelpCommand>(result);
        }
    }
}
