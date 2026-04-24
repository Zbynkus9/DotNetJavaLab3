using System.Drawing;

namespace ImageProcessing;

public static class ImageFilters
{
    public static Bitmap ApplyGrayscale(Bitmap original)
    {
        Bitmap bmp = new Bitmap(original); // Kopia dla bezpieczeństwa wątkowego
        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                Color p = bmp.GetPixel(x, y);
                int gray = (int)(p.R * 0.3 + p.G * 0.59 + p.B * 0.11);
                bmp.SetPixel(x, y, Color.FromArgb(p.A, gray, gray, gray));
            }
        }
        return bmp;
    }

    public static Bitmap ApplyNegative(Bitmap original)
    {
        Bitmap bmp = new Bitmap(original);
        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                Color p = bmp.GetPixel(x, y);
                bmp.SetPixel(x, y, Color.FromArgb(p.A, 255 - p.R, 255 - p.G, 255 - p.B));
            }
        }
        return bmp;
    }

    public static Bitmap ApplyThreshold(Bitmap original, int threshold = 128)
    {
        Bitmap bmp = new Bitmap(original);
        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                Color p = bmp.GetPixel(x, y);
                int gray = (int)(p.R * 0.3 + p.G * 0.59 + p.B * 0.11);
                Color resultColor = gray > threshold ? Color.White : Color.Black;
                bmp.SetPixel(x, y, resultColor);
            }
        }
        return bmp;
    }

    public static Bitmap ApplyRedFilter(Bitmap original)
    {
        Bitmap bmp = new Bitmap(original);
        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                Color p = bmp.GetPixel(x, y);
                bmp.SetPixel(x, y, Color.FromArgb(p.A, p.R, 0, 0));
            }
        }
        return bmp;
    }
}