namespace FileData.Tests
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class FileInspectorTests
    {
        private Mock<FileDetailsAdapter> mockFileDetails;

        [TestInitialize]
        public void Initialise()
        {
            mockFileDetails = new Mock<FileDetailsAdapter>();
        }

        [TestMethod]
        public void GetsSize()
        {
            //Arrange
            mockFileDetails.Setup(x => x.Size(It.IsAny<string>())).Returns(128);
            var fileInspector = new FileInspector(mockFileDetails.Object);
            var options = new Options()
            {
                ShowSize = true,
                FileName = "file1.txt"
            };

            //Act
            var result = fileInspector.InspectFile(options);

            //Assert
            mockFileDetails.Verify(x => x.Size("file1.txt"), Times.Once);
            result.Size.Should().Be(128);
            result.Version.Should().BeNull();
        }

        [TestMethod]
        public void GetsVersion()
        {
            //Arrange
            mockFileDetails.Setup(x => x.Version(It.IsAny<string>())).Returns("1.2.3");
            var fileInspector = new FileInspector(mockFileDetails.Object);
            var options = new Options()
            {
                ShowVersion = true,
                FileName = "file1.txt"
            };

            //Act
            var result = fileInspector.InspectFile(options);

            //Assert
            mockFileDetails.Verify(x => x.Version("file1.txt"), Times.Once);
            result.Version.Should().Be("1.2.3");
            result.Size.Should().BeNull();
        }

        [TestMethod]
        public void GetsBothSizeAndVersion()
        {
            //Arrange
            mockFileDetails.Setup(x => x.Size(It.IsAny<string>())).Returns(128);
            mockFileDetails.Setup(x => x.Version(It.IsAny<string>())).Returns("1.2.3");
            var fileInspector = new FileInspector(mockFileDetails.Object);
            var options = new Options()
            {
                ShowSize = true,
                ShowVersion = true,
                FileName = "file1.txt"
            };

            //Act
            var result = fileInspector.InspectFile(options);

            //Assert
            mockFileDetails.Verify(x => x.Size("file1.txt"), Times.Once);
            mockFileDetails.Verify(x => x.Version("file1.txt"), Times.Once);
            result.Version.Should().Be("1.2.3");
            result.Size.Should().Be(128);
        }

        [TestMethod]
        public void ReturnsNothingIfNoAction()
        {
            //Arrange
            var fileInspector = new FileInspector(mockFileDetails.Object);
            var options = new Options()
            {
                FileName = "file1.txt"
            };

            //Act
            var result = fileInspector.InspectFile(options);

            //Assert
            result.Version.Should().BeNull();
            result.Size.Should().BeNull();
        }

        [TestMethod]
        public void ThrowsExceptionIfMissingFileName()
        {
            //Arrange
            var fileInspector = new FileInspector(mockFileDetails.Object);
            var options = new Options()
            {
                ShowSize = true
            };

            //Act
            Action act = () => fileInspector.InspectFile(options);

            //Assert
            act.Should().Throw<ArgumentException>().WithMessage("FileName");
        }
    }
}
