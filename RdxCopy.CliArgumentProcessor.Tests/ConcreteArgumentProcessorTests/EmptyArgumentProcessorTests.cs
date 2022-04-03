using NUnit.Framework;
using RdxCopy.CliArgumentProcessor.ArgumentProcessors;
using RdxCopy.Commands;
using RdxCopy.TestHelper;
using System;

namespace RdxCopy.CliArgumentProcessor.Tests.ConcreteArgumentProcessorTests
{
    public class EmptyArgumentProcessorTests
    {
        private static EmptyArgumentProcessor _emptyArgumentProcessor;

        [OneTimeSetUp]
        public static void Init()
        {
            _emptyArgumentProcessor = new EmptyArgumentProcessor();
        }

        [Test]
        public void GetCommand_NotCommand_Null()
        {
            // Arrange
            string[] args = null;

            // Act
            var result = _emptyArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }

        [Test]
        public void GetCommand()
        {
            // Arrange
            var args = Array.Empty<string>();

            // Act
            var result = _emptyArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<EmptyCommand>(result);
        }

        [Test]
        [TestCase(new object[] { "" })]
        [TestCase(new object[] { "-s" })]
        [TestCase(new object[] { "-c", "-c" })]
        [TestCase(new object[] { "--clear", "-c" })]
        [TestCase(new object[] { "-clear" })]
        [TestCase(new object[] { "--c" })]
        public void GetCommand_NotCommands(object[] argsObj)
        {
            // Arrange
            var args = argsObj.ToStringArray();

            // Act
            var result = _emptyArgumentProcessor.GetCommand(args);

            // Assert
            Assert.IsInstanceOf<NotCommand>(result);
        }
    }
}
