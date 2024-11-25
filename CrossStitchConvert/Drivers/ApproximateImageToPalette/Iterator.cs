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

public class Iterator
{
    public static void Iterate()
    {
        var startIteration = Config.ApproximationStartIteration;
        var endIteration = Config.ApproximationEndIteration;
        var originalImageFile = Config.ApproximatedImageInputPath;
        var paletteFile = Config.PalettePath;

        var palette = PaletteUtils.ReadPalette(paletteFile);
        var originalImage = Image.Load<Rgba32>(originalImageFile);
        var previousIteration = startIteration;
        var currentIteration = startIteration + 1;
        for (; currentIteration <= endIteration && currentIteration < palette.Colors.Length; previousIteration++, currentIteration++)
        {
            var previousIr = ApproximatedImageProtobuf.Parser.ParseFrom(
                File.ReadAllBytes(string.Format(Config.ApproximatedIrOutputPath, previousIteration)));
            var previousImage = ImageUtils.ReadIntermediateResultIntoMemoryImage(previousIr, palette);

            var attemptedColors = new HashSet<int>(previousIr.AttemptedColors);
            var pickedColors = new HashSet<int>(previousIr.PickedColors);
            
            PaletteApproximatedImage? bestImage = null;
            var colorToAdd = palette.Colors[0];
            var bestDistance = int.MaxValue;
            foreach (var color in palette.Colors.Where(c => !attemptedColors.Contains(c.ColorIndex)))
            {
                var result = ImageApproximation.TryAddColor(originalImage, previousImage, color, previousIr.Distance);
                if (result.Item1)
                {
                    if (bestImage != null)
                    {
                        PaletteApproximatedImageFactory.Return(bestImage);
                    }

                    bestImage = result.Item2;
                    colorToAdd = color;
                    bestDistance = result.Item3;
                }
            }

            if (bestImage == null)
            {
                Console.WriteLine("No better color found. Exiting.");
                break;
            }

            attemptedColors.Add(colorToAdd.ColorIndex);
            pickedColors.Add(colorToAdd.ColorIndex);
            var nextIr = ImageUtils.Serialize(
                bestImage, 
                attemptedColors.ToArray(), 
                pickedColors.ToArray(), 
                bestDistance);

            // Console.WriteLine(JsonSerializer.Serialize(nextIr));
            
            File.WriteAllBytes(string.Format(Config.ApproximatedIrOutputPath, currentIteration), nextIr.ToByteArray());
            ImageUtils.OutputIntermediateResultToImage(bestImage, string.Format(Config.ApproximatedImageOutputPath, currentIteration));
        }
    }
}
