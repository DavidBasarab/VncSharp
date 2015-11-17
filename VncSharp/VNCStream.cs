using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Authentication;

namespace VncSharp
{
    public delegate void VncFrameReceived(Bitmap frame);

    public class VncStream
    {
        private readonly string _ipAddress;

        private readonly object _lockObject = new object();
        private readonly string _password;
        private readonly ushort _port;
        private Rectangle _frameDimensions = new Rectangle(0, 0, 1, 1);
        private VncClient _vncClient;

        public Rectangle FrameDimensions
        {
            get
            {
                if (Connected && _frameDimensions == new Rectangle(0, 0, 1, 1)) _frameDimensions = _vncClient.Framebuffer.Rectangle;

                return _frameDimensions;
            }
        }

        private bool Connected { get; set; }

        private Bitmap LatestFrame { get; set; }

        public VncStream(string ipAddress, ushort port = 5900, string password = "")
        {
            _ipAddress = ipAddress;
            _port = port;
            _password = password;
        }

        public event VncFrameReceived VncFrameReceived;

        public void Connect()
        {
            _vncClient = new VncClient();

            var needToAutenticate = _vncClient.Connect(_ipAddress, 0, _port, true);

            if (needToAutenticate) if (!_vncClient.Authenticate(_password)) throw new AuthenticationException(string.Format("Cannot Connect to {0} with password {1}", _ipAddress, _port));

            _vncClient.Initialize();

            Connected = true;

            AllocateFrameImage();

            _vncClient.VncUpdate += OnFrameUpdated;
            _vncClient.StartUpdates();
        }

        protected void OnVncFrameReceived()
        {
            var handler = VncFrameReceived;

            if (handler != null) handler(new Bitmap(LatestFrame));
        }

        private void AllocateFrameImage()
        {
            if (FrameDimensions.Width < 100 || FrameDimensions.Height < 100)

                throw new InvalidOperationException("FrameDimension is invalid.");
                //LatestFrame = new Bitmap(1920, 1080, PixelFormat.Format32bppArgb);
            else LatestFrame = new Bitmap(FrameDimensions.Width, FrameDimensions.Height, PixelFormat.Format32bppArgb);
        }

        private void OnFrameUpdated(object sender, VncEventArgs e)
        {
            try
            {
                lock (_lockObject)
                {
                    foreach (var deskotpUpdate in e.DesktopUpdates.OrderBy(i => i.Order).ToList()) deskotpUpdate.Draw(LatestFrame);

                    OnVncFrameReceived();
                }
            }
            catch (Exception ex)
            {
                var temp = ex.Message;

                Console.WriteLine("Message = {0}", temp);
            }
            finally
            {
                _vncClient.RequestScreenUpdate(false);
            }
        }
    }
}