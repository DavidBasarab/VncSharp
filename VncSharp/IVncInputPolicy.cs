using System.Drawing;

namespace VncSharp
{
    /// <summary>
    ///     A strategy encapsulating mouse/keyboard input.  Used by VncClient.
    /// </summary>
    public interface IVncInputPolicy
    {
        void WriteKeyboardEvent(uint keysym, bool pressed);

        void WritePointerEvent(byte buttonMask, Point point);
    }
}