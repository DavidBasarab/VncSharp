using System;
using System.Drawing;

namespace VncSharp
{
    /// <summary>
    ///     A Design Mode version of VncDesktopTransformPolicy.
    /// </summary>
    public sealed class VncDesignModeDesktopPolicy : VncDesktopTransformPolicy
    {
        public override bool AutoScroll
        {
            get { return true; }
        }

        public override Size AutoScrollMinSize
        {
            get { return new Size(608, 427); // just a default for Design graphic. Will get changed once connected.
            }
        }

        public VncDesignModeDesktopPolicy(RemoteDesktop remoteDesktop)
                : base(null, remoteDesktop) { }

        public override Rectangle AdjustUpdateRectangle(Rectangle updateRectangle)
        {
            throw new NotImplementedException();
        }

        public override Point GetMouseMovePoint(Point current)
        {
            throw new NotImplementedException();
        }

        public override Rectangle GetMouseMoveRectangle()
        {
            throw new NotImplementedException();
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

            return new Rectangle(x, y, remoteDesktop.ClientSize.Width, remoteDesktop.ClientSize.Height);
        }

        public override Point UpdateRemotePointer(Point current)
        {
            throw new NotImplementedException();
        }
    }
}