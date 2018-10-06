namespace FileData.Tests
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ProgramTests
    {
        private Mock<IFileInspector> mockFileInspector;
        private Mock<IParser> mockParser;

        [TestInitialize]
        public void Initialise()
        {
            mockFileInspector = new Mock<IFileInspector>();
            mockParser = new Mock<IParser>();
        }

        [TestMethod]
        public void ProgramShowsUsageInstructions()
        {
            //Arrange
            mockParser.Setup(x => x.Parse(It.IsAny<string[]>())).Returns(new Options() { ShowUsage = true });
            var p = new Program();

            //Act
            string output = p.Run(mockParser.Object, mockFileInspector.Object, new string[] { });

            //Assert
            output.Should().Be("FileData.exe (-s | -v) filenane");
        }

        [TestMethod]
        public void ProgramShowsSize()
        {
            //Arrange
            mockParser.Setup(x => x.Parse(It.IsAny<string[]>())).Returns(new Options() { ShowSize = true });
            mockFileInspector.Setup(x => x.InspectFile(It.Is<Options>(o => o.ShowSize == true)))
                .Returns(new FileInspectorResult() { Size = 128 });
            var p = new Program();

            //Act
            string output = p.Run(mockParser.Object, mockFileInspector.Object, new string[] { });

            //Assert
            output.Should().Be("Size: 128");
        }

        [TestMethod]
        public void ProgramShowsVersion()
        {
            //Arrange
            mockParser.Setup(x => x.Parse(It.IsAny<string[]>())).Returns(new Options() { ShowVersion = true });
            mockFileInspector.Setup(x => x.InspectFile(It.Is<Options>(o => o.ShowVersion == true)))
                .Returns(new FileInspectorResult() { Version = "1.2.3" });
            var p = new Program();

            //Act
            string output = p.Run(mockParser.Object, mockFileInspector.Object, new string[] { });

            //Assert
            output.Should().Be("Version: 1.2.3");
        }

        [TestMethod]
        public void ProgramShowsBothSizeAndVersion()
        {
            //Arrange
            mockParser.Setup(x => x.Parse(It.IsAny<string[]>()))
                .Returns(new Options()
                {
                    ShowSize = true,
                    ShowVersion = true
                });
            mockFileInspector.Setup(x => x.InspectFile(It.Is<Options>(o => o.ShowVersion == true && o.ShowVersion == true)))
                .Returns(new FileInspectorResult()
                {
                    Size = 128,
                    Version = "1.2.3"
                });
            var p = new Program();

            //Act
            string output = p.Run(mockParser.Object, mockFileInspector.Object, new string[] { });

            //Assert
            output.Should().Be("Size: 128, Version: 1.2.3");
        }

        [TestMethod]
        public void ProgramShowsNoData()
        {
            //Arrange
            mockParser.Setup(x => x.Parse(It.IsAny<string[]>())).Returns(new Options());
            mockFileInspector.Setup(x => x.InspectFile(It.Is<Options>(o => o.ShowVersion == false && o.ShowVersion == false)))
                .Returns(new FileInspectorResult());
            var p = new Program();

            //Act
            string output = p.Run(mockParser.Object, mockFileInspector.Object, new string[] { });

            //Assert
            output.Should().Be("No file information");
        }
    }
}
