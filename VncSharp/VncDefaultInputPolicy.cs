using System.Diagnostics;
using System.Drawing;

namespace VncSharp
{
    /// <summary>
    ///     A view-only version of IVncInputPolicy.
    /// </summary>
    public sealed class VncDefaultInputPolicy : IVncInputPolicy
    {
        private readonly RfbProtocol rfb;

        public VncDefaultInputPolicy(RfbProtocol rfb)
        {
            Debug.Assert(rfb != null);
            this.rfb = rfb;
        }

        // Let all exceptions get caught in VncClient

        public void WriteKeyboardEvent(uint keysym, bool pressed)
        {
            rfb.WriteKeyEvent(keysym, pressed);
        }

        public void WritePointerEvent(byte buttonMask, Point point)
        {
            rfb.WritePointerEvent(buttonMask, point);
        }
    }
}