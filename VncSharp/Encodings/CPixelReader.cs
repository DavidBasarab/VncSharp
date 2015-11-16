using System.IO;

namespace VncSharp.Encodings
{
    /// <summary>
    ///     A compressed PixelReader.
    /// </summary>
    public sealed class CPixelReader : PixelReader
    {
        public CPixelReader(BinaryReader reader, Framebuffer framebuffer)
                : base(reader, framebuffer) { }

        public override int ReadPixel()
        {
            var b = reader.ReadBytes(3);
            return ToGdiPlusOrder(b[2], b[1], b[0]);
        }
    }
}