using System;
using System.Reflection.Metadata;
using Library.Algorithm;
using Library.Configuration;
using Library.DataModel;
using Library.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace Library.Blueprint;

public class BlueprintCell
{
    int cellSize;
    int pixelXLower;
    int pixelYLower;
    PaletteColor color;
    Image outputImage;
    HashSet<int> colorsToSkip;
    Color contrastColor;

    public BlueprintCell(int x, int y, int cellSize, PaletteColor color, Image outputImage, HashSet<int> colorsToSkip)
    {
        this.cellSize = cellSize;
        this.color = color;
        this.outputImage = outputImage;

        this.pixelXLower = x * cellSize;
        this.pixelYLower = y * cellSize;

        this.colorsToSkip = colorsToSkip;

        var distanceToBlack = ImageApproximation.PixelDistance(Color.Black, color);
        var distanceToWhite = ImageApproximation.PixelDistance(Color.White, color);
        this.contrastColor = distanceToBlack > distanceToWhite ? Color.Black : Color.White;

        if (colorsToSkip.Contains(color.ColorIndex))
        {
            // replace the background color with linen color so it shows real world visual results - I will be stitching on linen
            this.color.Color.R = 250;
            this.color.Color.G = 240;
            this.color.Color.B = 230;
        }
    }

    public void Draw()
    {
        this.DrawRectangle(this.pixelXLower, this.pixelYLower, this.cellSize, this.cellSize, Color.White);
        this.DrawRectangle(this.pixelXLower + 1, this.pixelYLower + 1, this.cellSize - 2, this.cellSize - 2, Color.Black);
        this.FillColor(this.pixelXLower + 2, this.pixelYLower + 2, this.cellSize - 4, this.cellSize - 4, this.color.Color);
        if (!colorsToSkip.Contains(this.color.ColorIndex))
        {
            this.DrawText(this.pixelXLower + 2, this.pixelYLower + 2, this.color.Color.ColorCode, contrastColor);
        }
    }

    private void DrawRectangle(int x, int y, int width, int height, Color color)
    {
        // Define the rectangle's position and size
        var rectangle = new RectangleF(x, y, width, height); 

        // Define the pen (color and thickness)
        var pen = Pens.Solid(color, 1); 

        // Draw the rectangle's outline
        outputImage.Mutate(ctx => ctx.Draw(pen, rectangle));
    }

    private void FillColor(int x, int y, int width, int height, RawPaletteColor color)
    {
        outputImage.Mutate(ctx => ctx.FillPolygon(
            color.ToColor(), 
            new PointF(x, y),
            new PointF(x + width, y),
            new PointF(x + width, y + height),
            new PointF(x, y + height)));
    }

    private void DrawText(int x, int y, string text, Color color)
    {
        var fontCollection = new FontCollection();
        var fontFamily = fontCollection.Add(Config.BlueprintFontPath);
        var font = fontFamily.CreateFont(this.cellSize / 3);

        var textOptions = new DrawingOptions
        {
            GraphicsOptions = new GraphicsOptions { Antialias = true }
        };

        // Draw the text onto the image
        outputImage.Mutate(ctx => ctx.DrawText(textOptions, text, font, color, new PointF(this.pixelXLower + 2, this.pixelYLower + 2)));
    }
}
