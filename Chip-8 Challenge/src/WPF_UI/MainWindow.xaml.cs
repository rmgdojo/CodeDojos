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
using System.Threading.Tasks;

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
            _vm = new VM();
            _vm.Keypad.MapKeys(new WindowsKeypadMap());

            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;

            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                string imagePath = dialog.FileName;
                _vm.Load(imagePath);
                _vm.Display.OnDisplayUpdated += UpdateDisplay;
                _vm.SoundTimer.OnStart += StartSound;
                _vm.Run(ClockMode.Threaded, 1);
            }
            else
            {
                this.Close();
            }
        }

        private void StartSound(object sender, int cycles)
        {
            Console.Beep(500, (1000 / 60) * cycles);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            _vm.Keypad.KeyUp(e.Key.ToString());
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            _vm.Keypad.KeyDown(e.Key.ToString());
        }

        private void UpdateDisplay(object sender, bool[,] pixels)
        {
            if (!_isClosing)
            {
                int magnification = 20;
                int xExtent = pixels.GetLength(0) * magnification;
                int yExtent = pixels.GetLength(1) * magnification;

                byte[] pixelBytes = new byte[xExtent * yExtent * 4];
                int pixelIndex = 0;

                // x left to right, add 4 bytes per pixel (RGBA), then next line
                for (int y = 0; y < yExtent; y++)
                {
                    for (int x = 0; x < xExtent; x++)
                    {
                        byte pixel = (byte)(pixels[x / magnification, y / magnification] ? 255 : 0);
                        pixelBytes[pixelIndex++] = pixel; // Red
                        pixelBytes[pixelIndex++] = pixel; // Green
                        pixelBytes[pixelIndex++] = pixel; // Blue
                        pixelBytes[pixelIndex++] = 255;   // Alpha (always 0xFF)
                    }
                }

                Dispatcher.InvokeAsync(() =>
                {
                    var bitmap = BitmapFactory.New(xExtent, yExtent).FromByteArray(pixelBytes);

                    Screen.Stretch = Stretch.None;
                    Screen.Source = bitmap;
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
