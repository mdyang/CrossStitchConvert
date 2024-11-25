using System;
using System.ComponentModel;

namespace Library.DataModel;

public class RawPaletteColor
{
    public string ColorCode { get; set; }
    public string DmcName { get; set; }
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
    public string RGBHex { get; set; }
}
