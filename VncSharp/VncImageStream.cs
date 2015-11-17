using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Authentication;

namespace VncSharp
{
    public delegate void VncFrameReceived(Bitmap frame);

    public class VncImageStream
    {
        private readonly string _ipAddress;

        private readonly object _lockObject = new object();
        private readonly string _password;
        private readonly ushort _port;
        private Rectangle _frameDimensions = new Rectangle(0, 0, 1, 1);
        private VncClientWithImage _vncClient;

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

        public VncImageStream(string ipAddress, ushort port = 5900, string password = "")
        {
            _ipAddress = ipAddress;
            _port = port;
            _password = password;
        }

        public event VncFrameReceived VncFrameReceived;

        public void Connect()
        {
            _vncClient = new VncClientWithImage();

            var needToAutenticate = _vncClient.Connect(_ipAddress, 0, _port, true);

            if (needToAutenticate) if (!_vncClient.Authenticate(_password)) throw new AuthenticationException(string.Format("Cannot Connect to {0} with password {1}", _ipAddress, _port));

            _vncClient.Initialize();

            Connected = true;

            AllocateFrameImage();

            _vncClient.ImageUpdated += OnImageUpdated;
            _vncClient.StartUpdates();
        }

        protected void OnVncFrameReceived()
        {
            var handler = VncFrameReceived;

            if (handler != null) handler(new Bitmap(LatestFrame));
        }

        private void AllocateFrameImage()
        {
            if (FrameDimensions.Width < 100 || FrameDimensions.Height < 100) throw new InvalidOperationException("FrameDimension is invalid.");

            LatestFrame = new Bitmap(FrameDimensions.Width, FrameDimensions.Height, PixelFormat.Format32bppArgb);
        }

        private void OnImageUpdated(Bitmap latestframe)
        {
            LatestFrame = latestframe;

            OnVncFrameReceived();

            _vncClient.RequestScreenUpdate(false);
        }
    }
}