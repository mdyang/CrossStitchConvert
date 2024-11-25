using System;
using System.Text.Json;
using Google.Protobuf;
using Library.Algorithm;
using Library.Configuration;
using Library.DataModel;
using Library.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ApproximateImageToPalette;

public class Bootstraper
{
    public static void Bootstrap()
    {
        using (var originalImage = Image.Load<Rgba32>(Config.ApproximatedImageInputPath))
        {
            var palette = PaletteUtils.ReadPalette(Config.PalettePath);
            var minDistance = int.MaxValue;
            var bestImage = PaletteApproximatedImageFactory.Get(originalImage.Width, originalImage.Height);
            var bestColor = palette.Colors[0];
            foreach (var color in palette.Colors)
            {
                var approximatedImage = PaletteApproximatedImageFactory.Get(originalImage.Width, originalImage.Height);
                ImageUtils.FillWithOneColor(approximatedImage, color);
                var distance = ImageApproximation.CalculateDistance(originalImage, approximatedImage);
                if (distance < minDistance)
                {
                    PaletteApproximatedImageFactory.Return(bestImage);
                    bestImage = approximatedImage;
                    minDistance = distance;
                    bestColor = color;
                }
            }

            var ir = ImageUtils.Serialize(
                bestImage, 
                [bestColor.ColorIndex], 
                [bestColor.ColorIndex], 
                minDistance);

            Console.WriteLine(JsonSerializer.Serialize(ir));

            File.WriteAllBytes(string.Format(Config.ApproximatedIrOutputPath, 1), ir.ToByteArray());
            ImageUtils.OutputIntermediateResultToImage(bestImage, string.Format(Config.ApproximatedImageOutputPath, 1));
        }
    }
}
