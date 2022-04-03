using NUnit.Framework;
using RdxCopy.TestHelper;

namespace StringToArgsConverter.Tests
{
    public class Tests
    {
        [Test]
        [TestCase("", new object[] { })]
        [TestCase("-x", new object[] { "-x" })]
        [TestCase("-x -y", new object[] { "-x", "-y" })]
        [TestCase("-x               -y", new object[] { "-x", "-y" })]
        [TestCase("    -x       -y    ", new object[] { "-x", "-y" })]
        [TestCase("--xxx --yyy", new object[] { "--xxx", "--yyy" })]
        [TestCase("-x test1 -y test2", new object[] { "-x", "test1", "-y", "test2", })]
        [TestCase("-x .\\src -y", new object[] { "-x", ".\\src", "-y" })]
        [TestCase("-x \".\\Test Folder\\src\"", new object[] { "-x", ".\\Test Folder\\src" })]
        [TestCase("-x ^^ -y", new object[] { "-x", "^", "-y" })]
        [TestCase("escaping ^\"test 123^\"", new object[] { "escaping", "\"test", "123\"" })]
        [TestCase("test1 \"\" \"test2 ^^ test3\"", new object[] { "test1", "", "test2 ^ test3" })]
        [TestCase("test=test", new object[] { "test=test" })]
        [TestCase("test:test", new object[] { "test:test" })]
        [TestCase("'test test'", new object[] { "'test", "test'" })]
        [TestCase("\"'test test'\"", new object[] { "'test test'" })]
        [TestCase("   \"   a\"   \"   b\"   ", new object[] { "   a", "   b" })]
        public void ToArgsTests(string argsRaw, object[] expectedConversionResultObj)
        {
            // Arrange
            string[] expectedConversionResult = expectedConversionResultObj.ToStringArray();

            // Act
            string[] args = argsRaw.ToArgs();

            // Assert
            Assert.AreEqual(expectedConversionResult.Length, args.Length);
            for (int i = 0; i < args.Length; i++)
            {
                Assert.AreEqual(expectedConversionResult[i], args[i]);
            }
        }
    }
}