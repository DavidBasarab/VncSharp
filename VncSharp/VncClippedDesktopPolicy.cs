using System.Drawing;

namespace VncSharp
{
    /// <summary>
    ///     A clipped version of VncDesktopTransformPolicy.
    /// </summary>
    public sealed class VncClippedDesktopPolicy : VncDesktopTransformPolicy
    {
        public override bool AutoScroll
        {
            get { return true; }
        }

        public override Size AutoScrollMinSize
        {
            get
            {
                if (vnc != null && vnc.Framebuffer != null) return new Size(vnc.Framebuffer.Width, vnc.Framebuffer.Height);
                return new Size(100, 100);
            }
        }

        public VncClippedDesktopPolicy(VncClient vnc,
                                       RemoteDesktop remoteDesktop)
                : base(vnc, remoteDesktop) { }

        public override Rectangle AdjustUpdateRectangle(Rectangle updateRectangle)
        {
            int x, y;

            if (remoteDesktop.ClientSize.Width > remoteDesktop.Desktop.Size.Width) x = updateRectangle.X + (remoteDesktop.ClientRectangle.Width - remoteDesktop.Desktop.Width) / 2;
            else x = updateRectangle.X + remoteDesktop.AutoScrollPosition.X;

            if (remoteDesktop.ClientSize.Height > remoteDesktop.Desktop.Size.Height) y = updateRectangle.Y + (remoteDesktop.ClientRectangle.Height - remoteDesktop.Desktop.Height) / 2;
            else y = updateRectangle.Y + remoteDesktop.AutoScrollPosition.Y;

            return new Rectangle(x, y, updateRectangle.Width, updateRectangle.Height);
        }

        public override Point GetMouseMovePoint(Point current)
        {
            return current;
        }

        public override Rectangle GetMouseMoveRectangle()
        {
            var desktopRect = vnc.Framebuffer.Rectangle;

            if (remoteDesktop.ClientSize.Width > remoteDesktop.Desktop.Size.Width) desktopRect.X = (remoteDesktop.ClientRectangle.Width - remoteDesktop.Desktop.Width) / 2;

            if (remoteDesktop.ClientSize.Height > remoteDesktop.Desktop.Size.Height) desktopRect.Y = (remoteDesktop.ClientRectangle.Height - remoteDesktop.Desktop.Height) / 2;

            return desktopRect;
        }

        public override Rectangle RepositionImage(Image desktopImage)
        {
            // See if the image needs to be clipped (i.e., it is too big for the 
            // available space) or centered (i.e., it is too small)
            int x, y;

            if (remoteDesktop.ClientSize.Width > desktopImage.Width) x = (remoteDesktop.ClientRectangle.Width - desktopImage.Width) / 2;
            else x = remoteDesktop.DisplayRectangle.X;

            if (remoteDesktop.ClientSize.Height > desktopImage.Height) y = (remoteDesktop.ClientRectangle.Height - desktopImage.Height) / 2;
            else y = remoteDesktop.DisplayRectangle.Y;

            return new Rectangle(x, y, desktopImage.Width, desktopImage.Height);
        }

        public override Point UpdateRemotePointer(Point current)
        {
            var adjusted = new Point();
            if (remoteDesktop.ClientSize.Width > remoteDesktop.Desktop.Size.Width) adjusted.X = current.X - ((remoteDesktop.ClientRectangle.Width - remoteDesktop.Desktop.Width) / 2);
            else adjusted.X = current.X - remoteDesktop.AutoScrollPosition.X;

            if (remoteDesktop.ClientSize.Height > remoteDesktop.Desktop.Size.Height) adjusted.Y = current.Y - ((remoteDesktop.ClientRectangle.Height - remoteDesktop.Desktop.Height) / 2);
            else adjusted.Y = current.Y - remoteDesktop.AutoScrollPosition.Y;

            return adjusted;
        }
    }
}