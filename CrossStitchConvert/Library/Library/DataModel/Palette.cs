using System;

namespace Library.DataModel;

public class Palette
{
    public Palette(RawPaletteColor[] colors)
    {
        Colors = Enumerable.Range(0, colors.Length).Select(i => 
            new PaletteColor { Color = colors[i], ColorIndex = i }).ToArray();
    }
    public PaletteColor[] Colors { get; private set; }

    public PaletteColor this[int index] => Colors[index];
}
