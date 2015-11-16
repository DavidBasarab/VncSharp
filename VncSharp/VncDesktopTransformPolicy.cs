using System.Drawing;

namespace VncSharp
{
    /// <summary>
    ///     Base class for desktop clipping/scaling policies.  Used by RemoteDesktop.
    /// </summary>
    public abstract class VncDesktopTransformPolicy
    {
        protected RemoteDesktop remoteDesktop;
        protected VncClient vnc;

        public virtual bool AutoScroll
        {
            get { return false; }
        }

        public abstract Size AutoScrollMinSize { get; }

        public VncDesktopTransformPolicy(VncClient vnc,
                                         RemoteDesktop remoteDesktop)
        {
            this.vnc = vnc;
            this.remoteDesktop = remoteDesktop;
        }

        public abstract Rectangle AdjustUpdateRectangle(Rectangle updateRectangle);

        public abstract Point GetMouseMovePoint(Point current);

        public abstract Rectangle GetMouseMoveRectangle();

        public abstract Rectangle RepositionImage(Image desktopImage);

        public abstract Point UpdateRemotePointer(Point current);
    }
}