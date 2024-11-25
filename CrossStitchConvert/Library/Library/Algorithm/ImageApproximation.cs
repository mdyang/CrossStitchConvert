using System.Linq;
using Library.DataModel;
using Library.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Library.Algorithm;

public class ImageApproximation
{
    public static int CalculateDistance(Image<Rgba32> image1, PaletteApproximatedImage image2)
    {
        return Enumerable.Range(0, image1.Width)
            .SelectMany(x => Enumerable.Range(0, image1.Height)
                .Select(y => image1[x, y]))
            .Zip(image2.Pixels.Cast<PaletteColor>(), PixelDistance).Sum();
    }

    // (new color applied, new distance)
    public static (bool, PaletteApproximatedImage?, int) TryAddColor(
        Image<Rgba32> originalImage, 
        PaletteApproximatedImage currentApproximateImage, 
        PaletteColor color, 
        int currentDistance)
    {
        var newImage = PaletteApproximatedImageFactory.Clone(currentApproximateImage);
        var newDistance = currentDistance;
        var isNewBetter = false;
        for (var x = 0; x < originalImage.Width; x++)
        {
            for (var y = 0; y < originalImage.Height; y++)
            {
                var newPixelDistance = PixelDistance(originalImage[x, y], color);
                var existingPixelDistance = PixelDistance(originalImage[x, y], currentApproximateImage.Pixels[x, y]);
                if (newPixelDistance < existingPixelDistance)
                {
                    isNewBetter = true;
                    newImage.Pixels[x, y] = color;
                    newDistance -= existingPixelDistance - newPixelDistance;
                }
            }
        }

        if (!isNewBetter)
        {
            PaletteApproximatedImageFactory.Return(newImage);
            return (false, null, 0);
        }
        else
        {
            return (true, newImage, newDistance);
        }
    }

    public static int PixelDistance(Rgba32 pixel1, PaletteColor pixel2)
    {
        return Math.Abs(pixel1.R - pixel2.Color.R) + Math.Abs(pixel1.G - pixel2.Color.G) + Math.Abs(pixel1.B - pixel2.Color.B);
    }
}
