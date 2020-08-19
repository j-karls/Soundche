using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain
{
    public class SwitchedSongEventArgs : EventArgs
    {
        public Track NewTrack { get; set; }
        public long SwitchedSongTimeTicks { get; set; }

        public SwitchedSongEventArgs(Track newTrack, DateTime switchedSongTime)
        {
            NewTrack = newTrack;
            SwitchedSongTimeTicks = switchedSongTime.Ticks;
        }
    }
}
