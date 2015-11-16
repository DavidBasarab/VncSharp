using System;
using System.Drawing;

namespace VncSharp
{
    /// <summary>
    ///     A scaled version of VncDesktopTransformPolicy.
    /// </summary>
    public sealed class VncScaledDesktopPolicy : VncDesktopTransformPolicy
    {
        public override Size AutoScrollMinSize
        {
            get { return new Size(100, 100); }
        }

        private double ScaleFactor
        {
            get
            {
                if (((double)remoteDesktop.ClientRectangle.Width / vnc.Framebuffer.Width) <=
                    ((double)remoteDesktop.ClientRectangle.Height / vnc.Framebuffer.Height)) return ((double)remoteDesktop.ClientRectangle.Width / vnc.Framebuffer.Width);
                return ((double)remoteDesktop.ClientRectangle.Height / vnc.Framebuffer.Height);
            }
        }

        public VncScaledDesktopPolicy(VncClient vnc, RemoteDesktop remoteDesktop)
                : base(vnc, remoteDesktop) { }

        public override Rectangle AdjustUpdateRectangle(Rectangle updateRectangle)
        {
            var scaledSize = GetScaledSize(remoteDesktop.ClientRectangle.Size);
            var adjusted = new Rectangle(AdjusteNormalToScaled(updateRectangle.X) + ((remoteDesktop.ClientRectangle.Width - scaledSize.Width) / 2),
                                         AdjusteNormalToScaled(updateRectangle.Y) + ((remoteDesktop.ClientRectangle.Height - scaledSize.Height) / 2),
                                         AdjusteNormalToScaled(updateRectangle.Width),
                                         AdjusteNormalToScaled(updateRectangle.Height));
            adjusted.Inflate(1, 1);
            return adjusted;
        }

        public override Point GetMouseMovePoint(Point current)
        {
            return GetScaledMouse(current);
        }

        public override Rectangle GetMouseMoveRectangle()
        {
            return GetScaledRectangle(remoteDesktop.ClientRectangle);
        }

        public override Rectangle RepositionImage(Image desktopImage)
        {
            return GetScaledRectangle(remoteDesktop.ClientRectangle);
        }

        public override Point UpdateRemotePointer(Point current)
        {
            return GetScaledMouse(current);
        }

        private int AdjusteNormalToScaled(double value)
        {
            return (int)Math.Round(value * ScaleFactor);
        }

        private int AdjusteScaledToNormal(double value)
        {
            return (int)Math.Round(value / ScaleFactor);
        }

        private Point GetScaledMouse(Point src)
        {
            var scaledSize = GetScaledSize(remoteDesktop.ClientRectangle.Size);
            src.X = AdjusteScaledToNormal(src.X - ((remoteDesktop.ClientRectangle.Width - scaledSize.Width) / 2));
            src.Y = AdjusteScaledToNormal(src.Y - ((remoteDesktop.ClientRectangle.Height - scaledSize.Height) / 2));
            return src;
        }

        private Rectangle GetScaledRectangle(Rectangle rect)
        {
            var scaledSize = GetScaledSize(rect.Size);
            return new Rectangle((rect.Width - scaledSize.Width) / 2,
                                 (rect.Height - scaledSize.Height) / 2,
                                 scaledSize.Width,
                                 scaledSize.Height);
        }

        private Size GetScaledSize(Size s)
        {
            if (vnc == null) return new Size(remoteDesktop.Width, remoteDesktop.Height);

            if (((double)s.Width / vnc.Framebuffer.Width) <= ((double)s.Height / vnc.Framebuffer.Height)) return new Size(s.Width, (int)((double)s.Width / vnc.Framebuffer.Width * vnc.Framebuffer.Height));
            return new Size((int)((double)s.Height / vnc.Framebuffer.Height * vnc.Framebuffer.Width), s.Height);
        }
    }
}