using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using SixLabors.ImageSharp.Formats.Webp;

namespace Scsl.Drawing;

public static class ConvertToImageTypeExtensions
{
    /// <summary>
    /// Converts an <see cref="IFormFile"/> to WebP format with the specified quality.
    /// </summary>
    /// <param name="formFile">The input file to be converted.</param>
    /// <param name="quality">The quality of the WebP output, default is 75.</param>
    /// <returns>A <see cref="MemoryStream"/> containing the converted WebP image.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="formFile"/> is null.</exception>
    /// <exception cref="UnknownImageFormatException">Thrown if the file format is not supported.</exception>
    ///  /// <exception cref="FormatException">Thrown if the quantity is not between 1 and 100.</exception>
    public static Task<MemoryStream> ConvertImageToWebpAsync(this IFormFile formFile, int quality = 75)
    {
        // Check if the input file is null and throw an exception if it is.
        ArgumentNullException.ThrowIfNull(formFile, nameof(formFile));

        // Ensure the file is an image by checking its Content-Type; throw an exception if not.
        if (!formFile.ContentType.StartsWith("image/")) throw new UnknownImageFormatException("File format is not supported.");
        
        // Ensure the quality is between 1 and 100; throw an exception if not.
        if(quality is <= 0 or >= 101) throw new FormatException("Quality must be between 1 and 100."); 
        
        // Create a MemoryStream to hold the output WebP image data.
        var ms = new MemoryStream();

        // Load the input image from the provided IFormFile stream.
        using var image = Image.Load(formFile.OpenReadStream());

        // Converts the loaded image to WebP format with the specified quality.
        Task.FromResult(image.SaveAsWebpAsync(ms, new WebpEncoder() { Quality = quality }));

        // Reset the MemoryStream position to the beginning so it can be read later.
        ms.Position = 0;

        // Return the MemoryStream wrapped in a Task for async compatibility.
        return Task.FromResult(ms);
    }
    
    /// <summary>
    /// Converts an <see cref="IFormFile"/> to WebP format with the specified quality.
    /// </summary>
    /// <param name="formFile">The input file to be converted.</param>
    /// <param name="quality">The quality of the WebP output, default is 75.</param>
    /// <returns>A <see cref="MemoryStream"/> containing the converted WebP image.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="formFile"/> is null.</exception>
    /// <exception cref="UnknownImageFormatException">Thrown if the file format is not supported.</exception>
    ///  /// <exception cref="FormatException">Thrown if the quantity is not between 1 and 100.</exception>
    public static MemoryStream ConvertImageToWebp(this IFormFile formFile, int quality = 75)
    {
        // Check if the input file is null and throw an exception if it is.
        ArgumentNullException.ThrowIfNull(formFile, nameof(formFile));

        // Ensure the file is an image by checking its Content-Type; throw an exception if not.
        if (!formFile.ContentType.StartsWith("image/")) throw new FormatException("File format is not supported.");
        
        // Ensure the quality is between 1 and 100; throw an exception if not.
        if(quality is <= 0 or >= 101) throw new FormatException("Quality must be between 1 and 100.");
        
        // Create a MemoryStream to hold the output WebP image data.
        var ms = new MemoryStream();

        // Load the input image from the provided IFormFile stream.
        using var image = Image.Load(formFile.OpenReadStream());

        // Converts the loaded image to WebP format with the specified quality.
        image.SaveAsWebp(ms, new WebpEncoder() { Quality = quality });

        // Reset the MemoryStream position to the beginning so it can be read later.
        ms.Position = 0;

        // Return the MemoryStream wrapped in a Task for async compatibility.
        return ms;
    }
}