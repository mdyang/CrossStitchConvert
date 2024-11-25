using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Library.DataModel;

namespace Library.Utils;

public class ImageUtils
{
    public static void Pixelize(string inputPath, int sampleFactor, string outputPath)
    {
        // Read the image
        using (Image<Rgba32> inputImage = Image.Load<Rgba32>(inputPath))
        {
            // Create a new blank image with sampled dimensions
            int sampledWidth = inputImage.Width / sampleFactor;
            int sampledHeight = inputImage.Height / sampleFactor;

            using (Image<Rgba32> outputImage = new Image<Rgba32>(sampledWidth, sampledHeight))
            {
                // Sample pixels from the input image
                for (int y = 0; y < sampledHeight; y++)
                {
                    for (int x = 0; x < sampledWidth; x++)
                    {
                        // Map sampled coordinates to original image coordinates
                        int originalX = x * sampleFactor;
                        int originalY = y * sampleFactor;

                        // Get the pixel color from the original image
                        Rgba32 pixelColor = inputImage[originalX, originalY];

                        // Set the pixel color in the new image
                        outputImage[x, y] = pixelColor;
                    }
                }

                // Save the output image
                outputImage.Save(outputPath);
            }
        }
    }

    public static PaletteApproximatedImage ReadIntermediateResultIntoMemoryImage(ApproximatedImageProtobuf ir, Palette palette)
    {
        var image = PaletteApproximatedImageFactory.Get(ir.ImageData.Columns[0].Pixels.Count, ir.ImageData.Columns.Count);
        for (var x = 0; x < ir.ImageData.Columns.Count; x++)
        {
            for (var y = 0; y < ir.ImageData.Columns[x].Pixels.Count; y++)
            {
                var color = palette[ir.ImageData.Columns[x].Pixels[y]];
                image.Pixels[x, y] = color;
            }
        }

        return image;
    }

    public static void FillWithOneColor(PaletteApproximatedImage image, PaletteColor color)
    {
        for (var x = 0; x < image.Pixels.GetLength(0); x++)
        {
            for (var y = 0; y < image.Pixels.GetLength(1); y++)
            {
                image.Pixels[x, y] = color;
            }
        }
    }

    public static ApproximatedImageProtobuf Serialize(
        PaletteApproximatedImage image, 
        int[] attemptedColors,
        int[] pickedColors,
        int distance)
    {
        var result = new ApproximatedImageProtobuf { Distance = distance };
        result.ImageData = new ImageData();
        result.ImageData.Columns.AddRange(
            Enumerable.Range(0, image.Pixels.GetLength(0))
                .Select(x => 
                {
                    var column = new ImageColumnData();
                    column.Pixels.AddRange(Enumerable.Range(0, image.Pixels.GetLength(1)).Select(y => 
                        image.Pixels[x, y].ColorIndex));
                    return column;
                }));
                    
        result.AttemptedColors.AddRange(attemptedColors);
        result.PickedColors.AddRange(pickedColors);
        return result;
    }

    public static void OutputIntermediateResultToImage(PaletteApproximatedImage image, string outputPath)
    {
        using (var outputImage = new Image<Rgba32>(image.Pixels.GetLength(0), image.Pixels.GetLength(1)))
        {
            for (var x = 0; x < image.Pixels.GetLength(0); x++)
            {
                for (var y = 0; y < image.Pixels.GetLength(1); y++)
                {
                    var color = image.Pixels[x, y].Color;
                    outputImage[x, y] = new Rgba32(color.R, color.G, color.B);
                }
            }

            outputImage.Save(outputPath);
        }
    }
}
