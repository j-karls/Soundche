using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain
{
    public class SwitchedSongEventArgs : EventArgs
    {
        public Track NewTrack { get; set; }
        public DateTime SwitchedSongTime { get; set; }

        public SwitchedSongEventArgs(Track newTrack, DateTime switchedSongTime)
        {
            NewTrack = newTrack;
            SwitchedSongTime = switchedSongTime;
        }
    }
}
