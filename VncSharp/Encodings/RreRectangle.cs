using System.Drawing;

namespace VncSharp.Encodings
{
    /// <summary>
    ///     Implementation of RRE encoding, as well as drawing support. See RFB Protocol document v. 3.8 section 6.5.3.
    /// </summary>
    public sealed class RreRectangle : EncodedRectangle
    {
        public RreRectangle(RfbProtocol rfb, Framebuffer framebuffer, Rectangle rectangle)
                : base(rfb, framebuffer, rectangle, RfbProtocol.RRE_ENCODING) { }

        public override void Decode()
        {
            var numSubRect = (int)rfb.ReadUint32(); // Number of sub-rectangles within this rectangle
            var bgPixelVal = preader.ReadPixel(); // Background colour
            var subRectVal = 0; // Colour to be used for each sub-rectangle

            // Dimensions of each sub-rectangle will be read into these
            int x, y, w, h;

            // Initialize the full pixel array to the background colour
            FillRectangle(rectangle, bgPixelVal);

            // Colour in all the subrectangles, reading the properties of each one after another.
            for (var i = 0; i < numSubRect; i++)
            {
                subRectVal = preader.ReadPixel();
                x = rfb.ReadUInt16();
                y = rfb.ReadUInt16();
                w = rfb.ReadUInt16();
                h = rfb.ReadUInt16();

                // Colour in this sub-rectangle
                FillRectangle(new Rectangle(x, y, w, h), subRectVal);
            }
        }
    }
}