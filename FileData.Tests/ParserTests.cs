namespace FileData.Tests
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParserTests
    {
        private Parser parser;

        [TestInitialize]
        public void Initialise()
        {
            parser = new Parser();
        }

        [DataTestMethod]
        [DataRow("-s")]
        [DataRow("--s")]
        [DataRow("/s")]
        [DataRow("--size")]
        public void ParsesSizeAndFileName(string argument)
        {
            var result = parser.Parse(new string[] { argument, "file.txt" });
            result.ShowSize.Should().BeTrue();
            result.FileName.Should().Be("file.txt");
            result.ShowVersion.Should().BeFalse();
            result.ShowUsage.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("-v")]
        [DataRow("--v")]
        [DataRow("/v")]
        [DataRow("--version")]
        public void ParsesVersionAndFileName(string argument)
        {
            var result = parser.Parse(new string[] { argument, "file.txt" });
            result.ShowVersion.Should().BeTrue();
            result.FileName.Should().Be("file.txt");
            result.ShowSize.Should().BeFalse();
            result.ShowUsage.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("-v", "-s")]
        [DataRow("--v", "--s")]
        [DataRow("/v", "/s")]
        [DataRow("--version", "--size")]
        public void ParsesSizeVersionAndFileName(string argument1, string argument2)
        {
            var result = parser.Parse(new string[] { argument1, argument2, "file.txt" });
            result.ShowVersion.Should().BeTrue();
            result.ShowSize.Should().BeTrue();
            result.FileName.Should().Be("file.txt");
            result.ShowUsage.Should().BeFalse();
        }

        [TestMethod]
        public void ShowsUsageIfTooFewParamters()
        {
            var result = parser.Parse(new string[] { "file.txt" });
            AssetOnlyShowUsageIsSet(result);
        }

        [TestMethod]
        public void ShowsUsageIfTooManyParamters()
        {
            var result = parser.Parse(new string[] { "-v", "-s", "file1.txt", "file2.txt" });
            AssetOnlyShowUsageIsSet(result);
        }

        [TestMethod]
        public void ShowsUsageIfDuplicateActionsParamters()
        {
            var result = parser.Parse(new string[] { "-v", "-v", "file1.txt" });
            AssetOnlyShowUsageIsSet(result);
        }

        [DataTestMethod]
        [DataRow("-v", "-s")]
        [DataRow("-v", "--s")]
        [DataRow("-v", "/s")]
        [DataRow("-v", "--size")]
        [DataRow("-s", "-v")]
        [DataRow("-s", "--v")]
        [DataRow("-s", "/v")]
        [DataRow("-s", "--version")]
        public void ShowsUsageIfFilenameIsMissing(string argument1, string argument2)
        {
            //The case of only one paramter passed in is covered by ShowsUsageIfTooFewParamters()
            //Note: we're also testing that the second action parameter isn't misinterpreted as the file name
            var result = parser.Parse(new string[] { argument1, argument2 });
            AssetOnlyShowUsageIsSet(result);
        }

        [DataTestMethod]
        [DataRow("-v", "-?")]
        [DataRow("-?", "-v")]
        [DataRow("-s", "-?")]
        [DataRow("-?", "-s")]
        public void ShowsUsageIfUnrecognisedParamter(string argument1, string argument2)
        {
            var result = parser.Parse(new string[] { argument1, argument2, "file1.txt" });
            AssetOnlyShowUsageIsSet(result);
        }

        [TestMethod]
        public void ShowsUsageIfUnrecognisedParamterSingle()
        {
            var result = parser.Parse(new string[] { "-?", "file1.txt" });
            AssetOnlyShowUsageIsSet(result);
        }

        private void AssetOnlyShowUsageIsSet(Options options)
        {
            options.ShowUsage.Should().BeTrue();
            options.ShowVersion.Should().BeFalse();
            options.ShowSize.Should().BeFalse();
            options.FileName.Should().BeNull();
        }
    }
}
