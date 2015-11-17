using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using VncSharp;

namespace VNCTester
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        public VncStream VncStream { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public BitmapSource LoadBitmap(Bitmap source)
        {
            BitmapSource bs = null;
            var ip = IntPtr.Zero;
            try
            {
                ip = source.GetHbitmap();

                bs = Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Exception ex)
            {
                WriteMessage(ex.Message);
                WriteMessage(ex.StackTrace);
            }
            finally
            {
                if (ip != IntPtr.Zero) DeleteObject(ip);

                source.Dispose();
            }

            return bs;
        }

        private void OnStreamClick(object sender, RoutedEventArgs e)
        {
            var serverName = txtIpAddress.Text;
            var port = ushort.Parse(txtPort.Text);
            var password = txtPassword.Text;

            Task.Run(() =>
                     {
                         try
                         {
                             VncStream = new VncStream(serverName, port, password);

                             VncStream.VncFrameReceived += OnVncFrameRecieved;

                             VncStream.Connect();
                         }
                         catch (Exception ex)
                         {
                             WriteMessage(ex.Message);
                             WriteMessage(ex.StackTrace);
                         }
                     });
        }

        private void OnVncFrameRecieved(Bitmap frame)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                                                  {
                                                      var bitmapSource = LoadBitmap(frame);

                                                      if (bitmapSource != null) currentImage.Source = bitmapSource;
                                                  }));
            }
            catch (Exception ex)
            {
                WriteMessage(ex.Message);
                WriteMessage(ex.StackTrace);
            }
        }

        private void WriteMessage(string message, params object[] args)
        {
            Dispatcher.BeginInvoke(new Action(() => { txtData.Text += string.Format(message, args) + Environment.NewLine; }));
        }
    }
}