using System;

namespace VncSharp
{
    public class VncEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the IDesktopUpdater object that will handling re-drawing the desktop.
        /// </summary>
        public IDesktopUpdater DesktopUpdater { get; private set; }

        public VncEventArgs(IDesktopUpdater updater)
        {
            DesktopUpdater = updater;
        }
    }
}