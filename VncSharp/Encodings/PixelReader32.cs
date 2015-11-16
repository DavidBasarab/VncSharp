using System.IO;

namespace VncSharp.Encodings
{
    /// <summary>
    ///     A 32-bit PixelReader.
    /// </summary>
    public sealed class PixelReader32 : PixelReader
    {
        public PixelReader32(BinaryReader reader, Framebuffer framebuffer)
                : base(reader, framebuffer) { }

        public override int ReadPixel()
        {
            // Read the pixel value
            var b = reader.ReadBytes(4);

            var pixel = ((uint)b[0]) & 0xFF |
                        ((uint)b[1]) << 8 |
                        ((uint)b[2]) << 16 |
                        ((uint)b[3]) << 24;

            // Extract RGB intensities from pixel
            var red = (byte)((pixel >> framebuffer.RedShift) & framebuffer.RedMax);
            var green = (byte)((pixel >> framebuffer.GreenShift) & framebuffer.GreenMax);
            var blue = (byte)((pixel >> framebuffer.BlueShift) & framebuffer.BlueMax);

            return ToGdiPlusOrder(red, green, blue);
        }
    }
}