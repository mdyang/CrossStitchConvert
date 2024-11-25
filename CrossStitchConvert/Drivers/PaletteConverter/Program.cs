// See https://aka.ms/new-console-template for more information
using Library.Configuration;
using Library.Utils;

Console.WriteLine("Pallette converter");

var input = PaletteUtils.ReadPalette(Config.PalettePath);
;