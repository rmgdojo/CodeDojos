using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CHIP_8_Virtual_Machine;
using System;

namespace Chip8.UI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isClosing = false;
        private VM _vm;

        public MainWindow()
        {
            _vm = new VM(new WindowsKeypadMap());

            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;

            _vm.Display.OnDisplayUpdated += UpdateDisplay;

            _vm.Load("c:\\temp\\pong.rom");
            _vm.Run();
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            _vm.Keypad.KeyUp(e.Key.ToString());
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            _vm.Keypad.KeyDown(e.Key.ToString());
        }

        public void UpdateDisplay(object sender, bool[,] pixels)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            if (!_isClosing)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    int magnification = 10;
                    int xExtent = pixels.GetLength(0) * magnification;
                    int yExtent = pixels.GetLength(1) * magnification;

                    byte[] pixelBytes = new byte[xExtent * yExtent * 4];
                    int pixelIndex = 0;

                    // x left to right, add 4 byte per pixel (RGBA), then next line
                    for (int y = 0; y < yExtent; y++)
                    {
                        for (int x = 0; x < xExtent; x++)
                        {
                            byte pixel = (byte)(pixels[x / magnification, y / magnification] ? 0 : 255);
                            pixelBytes[pixelIndex++] = pixel;
                            pixelBytes[pixelIndex++] = pixel;
                            pixelBytes[pixelIndex++] = pixel;
                            pixelBytes[pixelIndex++] = 255;
                        }
                    }

                    var bitmap = BitmapFactory.New(640, 320).FromByteArray(pixelBytes);
                    Display.Stretch = Stretch.Fill;
                    Display.Source = bitmap;
                });
            }
        }
        
        protected override void OnClosing(CancelEventArgs e)
        {
            _isClosing = true;
            _vm?.Stop();
            base.OnClosing(e);
            Application.Current.Shutdown();
        }
    }
}
