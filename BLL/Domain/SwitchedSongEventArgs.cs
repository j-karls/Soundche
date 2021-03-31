using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain
{
    public class SwitchedSongEventArgs : EventArgs
    {
        public TrackRequest NewTrackRequest { get; set; }
        public DateTime SwitchedSongTime { get; set; }

        public SwitchedSongEventArgs(TrackRequest newTrackRequest, DateTime switchedSongTime)
        {
            NewTrackRequest = newTrackRequest;
            SwitchedSongTime = switchedSongTime;
        }
    }
}
