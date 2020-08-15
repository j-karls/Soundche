using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain
{
    public class SwitchedSongEventArgs : EventArgs
    {
        public string NewSongUrl { get; set; }
        public int StartTime { get; set; }

        public SwitchedSongEventArgs(string newSongUrl, int startTime)
        {
            NewSongUrl = newSongUrl;
            StartTime = startTime;
        }
    }
}
