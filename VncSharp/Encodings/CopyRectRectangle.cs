using System.Drawing;
using System.Drawing.Imaging;

namespace VncSharp.Encodings
{
    /// <summary>
    ///     Implementation of CopyRect encoding, as well as drawing support. See RFB Protocol document v. 3.8 section 6.5.2.
    /// </summary>
    public sealed class CopyRectRectangle : EncodedRectangle
    {
        // CopyRect Source Point (x,y) from which to copy pixels in Draw
        private Point source;

        public CopyRectRectangle(RfbProtocol rfb, Framebuffer framebuffer, Rectangle rectangle)
                : base(rfb, framebuffer, rectangle, RfbProtocol.COPYRECT_ENCODING) { }

        /// <summary>
        ///     Decodes a CopyRect encoded rectangle.
        /// </summary>
        public override void Decode()
        {
            // Read the source point from which to begin copying pixels
            source = new Point();
            source.X = rfb.ReadUInt16();
            source.Y = rfb.ReadUInt16();
        }

        public override unsafe void Draw(Bitmap desktop)
        {
            // Given a source area, copy this region to the point specified by destination
            var bmpd = desktop.LockBits(new Rectangle(new Point(0, 0), desktop.Size),
                                        ImageLockMode.ReadWrite,
                                        desktop.PixelFormat);

            // Avoid exception if window is dragged bottom of screen
            if (rectangle.Top + rectangle.Height >= framebuffer.Height) rectangle.Height = framebuffer.Height - rectangle.Top - 1;

            try
            {
                var pSrc = (int*)(void*)bmpd.Scan0;
                var pDest = (int*)(void*)bmpd.Scan0;

                // Calculate the difference between the stride of the desktop, and the pixels we really copied. 
                var nonCopiedPixelStride = desktop.Width - rectangle.Width;

                // Move source and destination pointers
                pSrc += source.Y * desktop.Width + source.X;
                pDest += rectangle.Y * desktop.Width + rectangle.X;

                // BUG FIX (Peter Wentworth) EPW:  we need to guard against overwriting old pixels before
                // they've been moved, so we need to work out whether this slides pixels upwards in memeory,
                // or downwards, and run the loop backwards if necessary. 
                if (pDest < pSrc)
                {
                    // we can copy with pointers that increment
                    for (var y = 0; y < rectangle.Height; ++y)
                    {
                        for (var x = 0; x < rectangle.Width; ++x) *pDest++ = *pSrc++;

                        // Move pointers to beginning of next row in rectangle
                        pSrc += nonCopiedPixelStride;
                        pDest += nonCopiedPixelStride;
                    }
                }
                else
                {
                    // Move source and destination pointers to just beyond the furthest-from-origin 
                    // pixel to be copied.
                    pSrc += (rectangle.Height * desktop.Width) + rectangle.Width;
                    pDest += (rectangle.Height * desktop.Width) + rectangle.Width;

                    for (var y = 0; y < rectangle.Height; ++y)
                    {
                        for (var x = 0; x < rectangle.Width; ++x) *(--pDest) = *(--pSrc);

                        // Move pointers to end of previous row in rectangle
                        pSrc -= nonCopiedPixelStride;
                        pDest -= nonCopiedPixelStride;
                    }
                }
            }
            finally
            {
                desktop.UnlockBits(bmpd);
                bmpd = null;
            }
        }
    }
}