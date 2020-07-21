using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public class Playlist
    {
        public List<Track> Tracks { get; set; }

        public void AddTrak(Track track)
        {
            Tracks.Add(track);
        }

        public void RemoveTrack(Track track)
        {
            Tracks.Remove(track);
        }
    }
}
