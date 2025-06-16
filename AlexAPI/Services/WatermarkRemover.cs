using System.Drawing;
using System.Drawing.Imaging;
using Tesseract;
using SixLabors.ImageSharp;                      // ImageSharp
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

public static class WatermarkDetector
{
    public static bool HasSuperyachtTimesWatermark(string webpPath, string tessdataPath)
    {
        // Load WebP using ImageSharp
        using var imageSharpImage = Image.Load<Rgba32>(webpPath);

        // Convert to System.Drawing.Bitmap
        using var ms = new MemoryStream();
        imageSharpImage.SaveAsBmp(ms);
        ms.Position = 0;
        using var bitmap = new Bitmap(ms);

        // Crop bottom-left area
        int cropWidth = bitmap.Width / 3;
        int cropHeight = bitmap.Height / 5;
        var cropRect = new System.Drawing.Rectangle(0, bitmap.Height - cropHeight, cropWidth, cropHeight);

        using var cropped = bitmap.Clone(cropRect, bitmap.PixelFormat);

        // Save to temp .bmp for Tesseract
        string tempCropPath = Path.GetTempFileName();
        cropped.Save(tempCropPath, System.Drawing.Imaging.ImageFormat.Bmp);

        // OCR with Tesseract
        using var engine = new TesseractEngine(tessdataPath, "eng", EngineMode.Default);
        using var croppedPix = Pix.LoadFromFile(tempCropPath);
        using var page = engine.Process(croppedPix);
        var text = page.GetText()?.ToLowerInvariant();

        File.Delete(tempCropPath); // Clean up

        return text?.Contains("superyacht times") == true;
    }
}
