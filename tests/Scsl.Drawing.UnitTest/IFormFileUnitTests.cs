using Microsoft.AspNetCore.Http;

namespace Scsl.Drawing.UnitTest;

[TestClass]
public class IFormFileUnitTests
{
    [TestMethod]
    public async Task Should_Upload_Single_File_NotNull_AreEqual()
    {
        // Arrange
        var path = Path.Combine(Environment.CurrentDirectory, "user-placeholder.webp");
        var actualStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var actual = new MemoryStream();
        await actualStream.CopyToAsync(actual);
        actualStream.Position = 0;
        
        //Creates mock file using MemoryStream
        const string fileName = "user-placeholder.png";
        path = Path.Combine(Environment.CurrentDirectory, "user-placeholder.png");
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        stream.Position = 0;
        
        //Creates FormFile
        // ref: https://stackoverflow.com/questions/73400712/trying-to-set-contenttype-for-formfile-instance-throws-an-exception
        IFormFile file = new FormFile(stream, 0, stream.Length, "Data", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };
        
        //Act
        var results = await file.ToWebpFormat(100);
        
        //Assert
        Assert.IsNotNull(results);
        Assert.AreEqual(results.Length, actual.Length);
    }
    
    [TestMethod]
    public async Task Should_Upload_Single_File_NotNull_NotAreEqual()
    {
        // Arrange
        var path = Path.Combine(Environment.CurrentDirectory, "user-placeholder.png");
        var actualStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var actual = new MemoryStream();
        await actualStream.CopyToAsync(actual);
        actualStream.Position = 0;
        
        //Creates mock file using MemoryStream
        const string fileName = "user-placeholder.png";
        path = Path.Combine(Environment.CurrentDirectory, "user-placeholder.png");
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        stream.Position = 0;
        
        //Creates FormFile
        IFormFile file = new FormFile(stream, 0, stream.Length, "Data", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };
        
        //Act
        var results = await file.ToWebpFormat(100);
        
        //Assert
        Assert.IsNotNull(results);
        Assert.AreNotEqual(results.Length, actual.Length);
    }
}