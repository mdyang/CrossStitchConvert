using System;

namespace Library.Configuration;

public class Config
{
    // general configuration
    public const string InputPath = "../input/";
    public const string OutputPath = "../output/";
    public const string PalettePath = $"{InputPath}/DMC color sheet.csv";

    // Pixelizer related configuration
    public const string PixelizerInputPath = $"{InputPath}/pxArt.png";
    public const string PixelizerOutputPath = $"{OutputPath}/pxArtPixelized_{{0}}.png";
    public const int PixelizerSampleInterval = 8;
    
    // Image approximation related configuration
    public const string ApproximatedImageInputPath = $"{OutputPath}/pxArtPixelized_8.png";
    public const string ApproximatedImageOutputPath = $"{OutputPath}/iterations/{{0}}.png";
    public const string ApproximatedIrOutputPath = $"{OutputPath}/iterations/{{0}}.bin";
    public const int ApproximationStartIteration = 1;
    public const int ApproximationEndIteration = 90;
    
    // blueprint generation related configuration
    public const string BlueprintIrInputPath = $"{OutputPath}/iterations/56.bin";
    public const string BlueprintOutputPath = $"{OutputPath}/blueprint.png";
    public const int BlueprintCellSize = 30;
    public const string BlueprintFontPath = "/System/Library/Fonts/Supplemental/Arial.ttf";
    public static readonly HashSet<int> BlueprintColorCodeToSkip = new HashSet<int>([3]);
}
