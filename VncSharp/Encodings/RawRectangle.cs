using System.Drawing;

namespace VncSharp.Encodings
{
    /// <summary>
    ///     Implementation of Raw encoding, as well as drawing support. See RFB Protocol document v. 3.8 section 6.5.1.
    /// </summary>
    public sealed class RawRectangle : EncodedRectangle
    {
        public RawRectangle(RfbProtocol rfb, Framebuffer framebuffer, Rectangle rectangle)
                : base(rfb, framebuffer, rectangle, RfbProtocol.RAW_ENCODING) { }

        public override void Decode()
        {
            // Each pixel from the remote server represents a pixel to be drawn
            for (var i = 0; i < rectangle.Width * rectangle.Height; ++i) framebuffer[i] = preader.ReadPixel();
        }
    }
}