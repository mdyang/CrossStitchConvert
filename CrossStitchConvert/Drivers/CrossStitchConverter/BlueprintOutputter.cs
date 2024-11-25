using Library.Blueprint;
using Library.Configuration;
using Library.DataModel;
using Library.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace CrossStitchConverter;

public class BlueprintOutputter
{
    public static void Output()
    {
        var inputIr = Config.BlueprintIrInputPath;
        var output = Config.BlueprintOutputPath;
        var palette = PaletteUtils.ReadPalette(Config.PalettePath);
        var cellSize = Config.BlueprintCellSize;

        var ir = ApproximatedImageProtobuf.Parser.ParseFrom(File.ReadAllBytes(inputIr));
        var width = ir.ImageData.Columns.Count;
        var height = ir.ImageData.Columns[0].Pixels.Count;

        using (var image = new Image<Argb32>(width * cellSize, height * cellSize))
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var color = palette[ir.ImageData.Columns[x].Pixels[y]];
                    var cell = new BlueprintCell(x, y, cellSize, color, image, Config.BlueprintColorCodeToSkip);
                    cell.Draw();
                }
            }

            image.Save(output);
        }
    }
}
