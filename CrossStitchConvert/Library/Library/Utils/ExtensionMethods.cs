using System;
using Library.DataModel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Library.Utils;

public static class ExtensionMethods
{
    public static Color ToColor(this RawPaletteColor rawPaletteColor)
    {
        return new Color(new Rgba32(rawPaletteColor.R, rawPaletteColor.G, rawPaletteColor.B));
    }
}
