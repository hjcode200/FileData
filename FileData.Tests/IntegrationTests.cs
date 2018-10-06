namespace FileData.Tests
{
    using System.Text.RegularExpressions;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Add a few end-to-end inegration tests using the real components to check the output
    /// </summary>
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void ShowsUageInstructions()
        {
            var p = new Program();

            var result = p.Run(new Parser(), new FileInspector(new FileDetailsAdapter()), new string[] { });

            result.Should().Be("FileData.exe (-s | -v) filenane");
        }

        [TestMethod]
        public void ReportsSize()
        {
            var p = new Program();

            var result = p.Run(new Parser(), new FileInspector(new FileDetailsAdapter()), new string[] { "-s", "file.txt" });

            //As this is an integration test rather than a unit test, we don't know what the real FileInspector will return.
            //We'll simply test that it returned an integer for the size.
            Regex.IsMatch(result, @"^Size: [0-9]+$").Should().BeTrue();
        }

        [TestMethod]
        public void ReportsVersion()
        {
            var p = new Program();

            var result = p.Run(new Parser(), new FileInspector(new FileDetailsAdapter()), new string[] { "--version", "file.txt" });

            //As this is an integration test rather than a unit test, we don't know what the real FileInspector will return.
            //We'll simply test that it returned a valid looking version number.
            Regex.IsMatch(result, @"^Version: [0-9]+\.[0-9]+\.[0-9]+$").Should().BeTrue();
        }

        [TestMethod]
        public void ReportsSizeAndVersion()
        {
            var p = new Program();

            var result = p.Run(new Parser(), new FileInspector(new FileDetailsAdapter()), new string[] { "--version", "/s", "file.txt" });

            Regex.IsMatch(result, @"^Size: [0-9]+, Version: [0-9]+\.[0-9]+\.[0-9]+$").Should().BeTrue();
        }
    }
}
