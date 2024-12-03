# How to Use the `ConvertImageToWebp` Extension Method

This guide explains how to use the `ConvertImageToWebp` extension method to convert an uploaded image file (`IFormFile`) into WebP format.

---

## Method Definition

```csharp
public static Task<MemoryStream> ToWebpFormat(this IFormFile formFile, int quality = 75)
```
### Parameters
- `formFile`: An instance of `IFormFile` representing the uploaded file. It must be an image file.
- ```quality``` (optional): An integer between 1 and 100 specifying the quality of the Webp image (default is 75)
### Returns
- A ```Task<MemoryStream>``` containing the converted image in Webp format.
---
## Prerequisites 
1. **Library Reference**: Ensure you have the `Scsl.Drawing` library installed for image processing. You can install it via NuGet:
```
dotnet add package Scsl.Drawing
```
2. **Webp Encoder**: The method uses `ConvertImageToWebp` and `ConvertImageToWebpAsync`, which is part of `Scsl.Drawing`.
---
## Usage Exmaple
Here is an example of how to use the `ConvertImageToWebp` extension method in an ASP.NET Cor application:
### 1. Accept an Image File in the a Controller:
```csharp
[HttpPost("upload-image")]
public async Task<IActionResult> UploadImage(IFormFile file)
{
    try
    {
        // Convert the uploaded image to WebP format
        MemoryStream webpStream = await file.ConvertImageToWebpAsync(80);

        // Example: Save the WebP file to disk (optional)
        var filePath = Path.Combine("wwwroot", "images", "converted-image.webp");
        await System.IO.File.WriteAllBytesAsync(filePath, webpStream.ToArray());

        return Ok(new { Message = "Image converted to WebP successfully.", Path = filePath });
    }
    catch (Exception ex)
    {
        // Handle exceptions (e.g., invalid file format)
        return BadRequest(new { Message = ex.Message });
    }
}
```
### 2. Client-Side Example (Postman or cURL):
Send POST request with an image file:
```
curl -X POST "http://localhost:5000/upload-image" -F "file=@path-to-image.jpg"
```
---
### Detailed Behavior
**1. Input Validation**
- If the `formFile` is `null`, an `ArgumentNullException` is thrown.
- If the file is not an image, a `UnknownImageFormatException` is thrown.
- if the image quality is less than 1 and greater than 100, a `FormatException` is thrown.

**2. Conversion**
- The input image is loaded into memory
- It is than encoded in Webp format using the specified quality setting.

**3. Output**
- The converted image is returned as a `MemoryStream`, ready to be saved or processed further.
---
### Notes
- **Supported Formats**: The input file must be an image format supported by `Scsl.Drawing` (e.g., JPG, PNG, JPEG, etc.).
- **Peformanace**: Webp compression can be computationally intensive for large images. Consider handling large file asynchronously.
---
### Common Issues
**1. Unsupported File Format:**
- Error `File format is not supported.`
- Solution: Ensure the uploaded file's `Content-Type` starts with `"image/"`.