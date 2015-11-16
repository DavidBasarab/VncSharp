using System.IO;

namespace VncSharp.Encodings
{
    /// <summary>
    ///     An 8-bit PixelReader
    /// </summary>
    public sealed class PixelReader8 : PixelReader
    {
        private readonly RfbProtocol rfb;

        public PixelReader8(BinaryReader reader, Framebuffer framebuffer, RfbProtocol rfb)
                : base(reader, framebuffer)
        {
            this.rfb = rfb;
        }

        /// <summary>
        ///     Reads an 8-bit pixel.
        /// </summary>
        /// <returns>Returns an Integer value representing the pixel in GDI+ format.</returns>
        public override int ReadPixel()
        {
            var idx = reader.ReadByte();
            return ToGdiPlusOrder((byte)rfb.MapEntries[idx, 0], (byte)rfb.MapEntries[idx, 1], (byte)rfb.MapEntries[idx, 2]);
        }
    }
}