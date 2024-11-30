using Microsoft.AspNetCore.Http;

using SixLabors.ImageSharp.Formats.Webp;

namespace Scsl.Drawing;

public static class Convert
{
    /// <summary>
    /// Converts an <see cref="IFormFile"/> to WebP format with the specified quality.
    /// </summary>
    /// <param name="formFile">The input file to be converted.</param>
    /// <param name="quality">The quality of the WebP output, default is 75.</param>
    /// <returns>A <see cref="MemoryStream"/> containing the converted WebP image.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="formFile"/> is null.</exception>
    /// <exception cref="FormatException">Thrown if the file format is not supported.</exception>
    public static Task<MemoryStream> ToWebpFormat(this IFormFile formFile, int quality = 75)
    {
        // Checks if the file is valid
        ArgumentNullException.ThrowIfNull(formFile, nameof(formFile));
        string[] contentTypesExtensions = [".png", ".jpeg", ".webp", ".jpg"];
        if (!contentTypesExtensions.Contains(formFile.ContentType.ToLowerInvariant()))
            throw new FormatException("File format is not supported.");
        
        var ms = new MemoryStream();
        using var image = Image.Load(formFile.OpenReadStream());
        //Convert the image to webp format
        Task.FromResult(image.SaveAsWebpAsync(ms, new WebpEncoder() { Quality = quality }));
        //Rest the MemoryStream back to zero
        ms.Position = 0;
        
        return Task.FromResult(ms);
    }
}