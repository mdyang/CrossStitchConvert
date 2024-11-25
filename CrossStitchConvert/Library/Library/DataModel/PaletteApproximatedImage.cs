using System;
using Microsoft.Extensions.ObjectPool;

namespace Library.DataModel;

public class PaletteApproximatedImage
{
    public PaletteApproximatedImage()
    {
        Pixels = new PaletteColor[0, 0];
    }

    public PaletteColor[,] Pixels { get; private set; }

    public void Resize(int width, int height)
    {
        if (width != Pixels.GetLength(0) || height != Pixels.GetLength(1))
        {
            Pixels = new PaletteColor[width, height];
        }
    }
}

public class PaletteApproximatedImageObjectPoolPolicy : PooledObjectPolicy<PaletteApproximatedImage>
{
    public override PaletteApproximatedImage Create()
    {
        // generate a new image object. we will resize the image as appropriate outside of the pool abstraction
        return new PaletteApproximatedImage();
    }

    public override bool Return(PaletteApproximatedImage obj)
    {
        // don't reset the image. this is to save memory allocation. we will reset the image outside of the pool abstraction
        return true;
    }
}
