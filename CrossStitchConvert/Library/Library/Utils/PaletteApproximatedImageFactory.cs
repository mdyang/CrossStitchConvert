using System;
using Library.DataModel;
using Microsoft.Extensions.ObjectPool;

namespace Library.Utils;

public class PaletteApproximatedImageFactory
{
    private static ObjectPool<PaletteApproximatedImage> _pool = 
        new DefaultObjectPool<PaletteApproximatedImage>(new PaletteApproximatedImageObjectPoolPolicy(), 100);

    public static PaletteApproximatedImage Get(int width, int height)
    {
        var image = _pool.Get();
        image.Resize(width, height);
        return image;
    }

    public static PaletteApproximatedImage Clone(PaletteApproximatedImage image)
    {
        var newImage = Get(image.Pixels.GetLength(0), image.Pixels.GetLength(1));
        for (int x = 0; x < image.Pixels.GetLength(0); x++)
        {
            for (int y = 0; y < image.Pixels.GetLength(1); y++)
            {
                newImage.Pixels[x, y] = image.Pixels[x, y];
            }
        }
        return newImage;
    }

    public static void Return(PaletteApproximatedImage image)
    {
        _pool.Return(image);
    }
}
