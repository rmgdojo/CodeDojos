namespace CHIP_8_Anti_Flicker
{
    public class BitmapDisplayUpdateInfo
    {
        public byte[] Pixels { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }

        public BitmapDisplayUpdateInfo(byte[] pixels, int width, int height)
        {
            Pixels = pixels;
            Width = width;
            Height = height;
        }
    }
}
