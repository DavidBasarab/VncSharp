using System;
using System.Collections.Generic;

namespace VncSharp
{
    public class VncEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the IDesktopUpdater object that will handling re-drawing the desktop.
        /// </summary>
        public List<IDesktopUpdater> DesktopUpdates { get; private set; }

        public VncEventArgs()
        {
            DesktopUpdates = new List<IDesktopUpdater>();
        }

        public VncEventArgs(List<IDesktopUpdater> updates)
        {
            DesktopUpdates = updates;
        }
    }
}