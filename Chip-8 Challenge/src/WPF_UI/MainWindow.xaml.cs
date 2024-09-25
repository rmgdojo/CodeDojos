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
using System.Linq.Expressions;
using System.Windows.Media.Media3D;

namespace Chip8.UI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isClosing = false;
        private VM _vm;
        private string _imagePath;

        public MainWindow()
        {
            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (_vm is null)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    _imagePath = dialog.FileName;
                }
                else
                {
                    this.Close();
                }

                SetupVM();
            }
        }

        private void SetupVM()
        {
            _vm = new VM(new WindowsKeypadMap());
            _vm.Load(_imagePath);
            _vm.Display.OnDisplayUpdated += (sender, info) => UpdateDisplay(info);
            _vm.Run(ClockMode.Threaded, 2);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            _vm.Keypad.KeyUp(e.Key.ToString());
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            _vm.Keypad.KeyDown(e.Key.ToString());
        }

        private void UpdateDisplay(DisplayUpdateInfo info)
        {
            Task.Run(() =>
            {
                int magnification = 20;
                int width = info.Width * magnification;
                int height = info.Height * magnification;

                byte[] pixelBytes = new byte[width * height * 4];
                int pixelIndex = 0;

                // x left to right, add 4 bytes per pixel (RGBA), then next line
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte pixel = (byte)(info.Pixels[x / magnification, y / magnification] ? 255 : 0);
                        pixelBytes[pixelIndex++] = pixel;// pixel; // Red
                        pixelBytes[pixelIndex++] = pixel; // Green
                        pixelBytes[pixelIndex++] = pixel;// pixel; // Blue
                        pixelBytes[pixelIndex++] = 255;   // Alpha (always 0xFF)
                    }
                }

                Dispatcher.InvokeAsync(() =>
                {
                    var bitmap = BitmapFactory.New(width, height).FromByteArray(pixelBytes);
                    Screen.Stretch = Stretch.None;
                    Screen.Source = bitmap;
                });
            });
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
