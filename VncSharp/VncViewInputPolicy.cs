using System.Diagnostics;
using System.Drawing;

namespace VncSharp
{
    /// <summary>
    ///     A view-only version of IVncInputPolicy.
    /// </summary>
    public sealed class VncViewInputPolicy : IVncInputPolicy
    {
        public VncViewInputPolicy(RfbProtocol rfb)
        {
            Debug.Assert(rfb != null);
        }

        public void WriteKeyboardEvent(uint keysym, bool pressed) { }

        public void WritePointerEvent(byte buttonMask, Point point) { }
    }
}