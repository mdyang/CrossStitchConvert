using System;
using Library.DataModel;
using CsvHelper;

namespace Library.Utils;

public class PaletteUtils
{
    public static Palette ReadPalette(string path)
    {
        using (var streamReader = new StreamReader(path))
        using (var csvReader = new CsvReader(streamReader))
        {
            return new Palette(csvReader.GetRecords<RawPaletteColor>().ToArray());
        }
    }
}
