using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;
using System.Threading.Tasks;
using CHIP_8_Virtual_Machine;
using CHIP_8_Anti_Flicker;

namespace Chip8.UI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isClosing = false;
        private VM _vm;
        private AntiFlickerDisplay _display;
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
            _display = new AntiFlickerDisplay(_vm.Display, true, 20);
            _display.OnDisplayUpdatedAsBitmap += UpdateDisplay;
            _vm.Run(ClockMode.Threaded, 100);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            _vm.Keypad.KeyUp(e.Key.ToString());
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            _vm.Keypad.KeyDown(e.Key.ToString());
        }

        private void UpdateDisplay(object sender, BitmapDisplayUpdateInfo info)
        {
            var bitmap = BitmapFactory.New(info.Width, info.Height);
            bitmap = bitmap.FromByteArray(info.Pixels, info.Width, info.Height);

            Dispatcher.InvokeAsync(() =>
            {
                Screen.Stretch = Stretch.None;
                Screen.Source = bitmap;
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
