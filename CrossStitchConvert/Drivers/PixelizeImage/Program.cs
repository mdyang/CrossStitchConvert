using System.ComponentModel;
using Library.Configuration;
using Library.Utils;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Pixelize image");

ImageUtils.Pixelize(
    Config.PixelizerInputPath, 
    Config.PixelizerSampleInterval, 
    string.Format(
        Config.PixelizerOutputPath, 
        Config.PixelizerSampleInterval));
