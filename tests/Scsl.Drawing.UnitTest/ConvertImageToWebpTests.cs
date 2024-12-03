using System.Text;

using Microsoft.AspNetCore.Http;

using Moq;

using SixLabors.ImageSharp;

namespace Scsl.Drawing.UnitTest;

#nullable disable
[TestClass]
public class ConvertImageToWebpTests
{
    private FileInfo webpImageFileInfo;
    private FileInfo pngImageFileInfo;
    
    [TestInitialize]
    public void Initialize()
    {
        webpImageFileInfo = new FileInfo(Path.Combine(Environment.CurrentDirectory, "user-placeholder.webp"));
        pngImageFileInfo = new FileInfo(Path.Combine(Environment.CurrentDirectory, "user-placeholder.png"));
    }
    [TestMethod]
    public async Task Image_ShouldThrowFormatException_WhenQualityIsGreaterThan100()
    {
        // Arrange
        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(f => f.ContentType).Returns("image/png");
        formFileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
        var file = formFileMock.Object;

        //Act
        var exception = await Assert.ThrowsExceptionAsync<FormatException>(async () =>
        {
            await file.ConvertImageToWebpAsync(101);
        });

        //Assert
        Assert.AreEqual("Quality must be between 1 and 100.", exception.Message);
    }

    [TestMethod]
    public async Task Image_ShouldThrowFormatException_WhenQualityIsLessThan1()
    {
        // Arrange
        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(f => f.ContentType).Returns("image/png");
        formFileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
        var file = formFileMock.Object;

        //Act
        var exception = await Assert.ThrowsExceptionAsync<FormatException>(async () =>
        {
            await file.ConvertImageToWebpAsync(0);
        });

        //Assert
        Assert.AreEqual("Quality must be between 1 and 100.", exception.Message);
    }

    [TestMethod]
    public async Task Image_ShouldCompleteSuccessfully_WhenQualityIsBetween1And100()
    {
        // Arrange
        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(f => f.ContentType).Returns("image/png"); // Valid content type
        formFileMock.Setup(f => f.FileName).Returns(pngImageFileInfo.Name);
        formFileMock.Setup(f => f.Length).Returns(pngImageFileInfo.Length);
        formFileMock.Setup(f => f.OpenReadStream()).Returns(new FileStream(pngImageFileInfo.FullName, FileMode.Open, FileAccess.Read));

        var formFile = formFileMock.Object;

        // Act
        var result = await formFile.ConvertImageToWebpAsync(80); // Testing with quality within valid range

        // Assert
        Assert.IsNotNull(result); // Verify that a MemoryStream is returned
        Assert.IsInstanceOfType(result, typeof(MemoryStream)); // Check that the result is a MemoryStream
        Assert.AreEqual(0, result.Position); // Ensure the position of the MemoryStream is at the beginning
    }

    [TestMethod]
    public async Task Image_ShouldThrowArgumentNullException_WhenFormFileIsNull()
    {
        // Arrange
        IFormFile formFile = null; // Simulating a null form file

        // Act & Assert
        var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
        {
            await formFile.ConvertImageToWebpAsync(); // No quality specified, using default
        });

        // Verify the exception parameter name
        Assert.AreEqual("formFile", exception.ParamName);
    }

    [TestMethod]
    public async Task Image_ShouldThrowUnknownImageFormatException_WhenContentTypeIsInvalid()
    {
        // Arrange
        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(f => f.ContentType).Returns("application/pdf"); // Invalid content type
        formFileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[] { 1, 2, 3, 4 }));

        var formFile = formFileMock.Object;

        // Act & Assert
        var exception = await Assert.ThrowsExceptionAsync<UnknownImageFormatException>(async () =>
        {
            await formFile.ConvertImageToWebpAsync(); // No quality specified, using default
        });

        // Verify exception message
        Assert.AreEqual("File format is not supported.", exception.Message);
    }

    [TestMethod]
    public async Task ConvertImageFile_ShouldReturnMemoryStream_WhenFileIsValid()
    {
        // Arrange
        var imageData = Encoding.UTF8.GetBytes(pngImageFileInfo.FullName); // Simulating image data
        var memoryStream = new MemoryStream(imageData);

        // Create a mock IFormFile
        var fileStreamMock = new Mock<IFormFile>();
        fileStreamMock.Setup(f => f.FileName).Returns(pngImageFileInfo.Name);
        fileStreamMock.Setup(f => f.ContentType).Returns("image/png");
        fileStreamMock.Setup(f => f.Length).Returns(pngImageFileInfo.Length);
        fileStreamMock.Setup(f => f.OpenReadStream()).Returns(new FileStream(pngImageFileInfo.FullName, FileMode.Open, FileAccess.Read));

        var file = fileStreamMock.Object;

        // Act
        var results = await file.ConvertImageToWebpAsync(); // Use the new function name

        // Assert
        Assert.IsNotNull(results);
        Assert.AreNotEqual(results.Length, memoryStream.Length); // Ensure conversion occurred
    }
}