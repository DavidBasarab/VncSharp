using System.Drawing;

namespace VncSharp
{
    /// <summary>
    ///     Classes that implement IDesktopUpdater are used to update and Draw on a local Bitmap representation of the remote desktop.
    /// </summary>
    public interface IDesktopUpdater
    {
        /// <summary>
        ///     The region of the desktop Bitmap that needs to be re-drawn.
        /// </summary>
        Rectangle UpdateRectangle { get; }

        /// <summary>
        ///     Given a desktop Bitmap that is a local representation of the remote desktop, updates sent by the server are drawn into the area specifed by UpdateRectangle.
        /// </summary>
        /// <param
        ///     name="desktop" >
        ///     The desktop Bitmap on which updates should be drawn.
        /// </param>
        void Draw(Bitmap desktop);
    }
}