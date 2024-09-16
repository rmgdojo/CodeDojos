using CHIP_8_Virtual_Machine;
using Microsoft.AspNetCore.Components;

namespace CHIP_8_Anti_Flicker
{
    public class AntiFlickerDisplay
    {
        private bool _antiFlicker;
        private int _magnification;

        public event EventHandler<BitmapDisplayUpdateInfo> OnDisplayUpdatedAsBitmap;
        public event EventHandler<DisplayUpdateInfo> OnDisplayUpdatedAsPixels;

        private void OnDisplayUpdated(CHIP_8_Virtual_Machine.DisplayUpdateInfo info)
        {
            bool[,] pixels = info.Pixels;
            int width = info.Width * _magnification;
            int height = info.Height * _magnification;

            // relay to any direct clients
            OnDisplayUpdatedAsPixels?.Invoke(this, new DisplayUpdateInfo(info.Pixels, info.Width, info.Height));

            if (OnDisplayUpdatedAsBitmap is not null)
            {
                // convert to bitmap
                byte[] pixelBytes = new byte[width * height * 4];
                int pixelIndex = 0;

                // x left to right, add 4 bytes per pixel (RGBA), then next line
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte pixel = (byte)(info.Pixels[x / _magnification, y / _magnification] ? 255 : 0);
                        pixelBytes[pixelIndex++] = pixel; // Red
                        pixelBytes[pixelIndex++] = pixel; // Green
                        pixelBytes[pixelIndex++] = pixel; // Blue
                        pixelBytes[pixelIndex++] = 255;   // Alpha (always 0xFF)
                    }
                }

                OnDisplayUpdatedAsBitmap(this, new BitmapDisplayUpdateInfo(pixelBytes, width, height));
            }
        }

        public AntiFlickerDisplay(CHIP_8_Virtual_Machine.Display originalDisplay, bool antiFlicker, int bitmapMagnificationFactor = 10)
        {
            originalDisplay.OnDisplayUpdated += (sender, info) => OnDisplayUpdated(info);
            _antiFlicker = antiFlicker;
            _magnification = bitmapMagnificationFactor;
        }
    }
}
