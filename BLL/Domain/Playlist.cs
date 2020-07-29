using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Domain
{
    public class Playlist
    {
        public string Name { get; set; }
        public List<Track> Tracks { get; set; }

        public Playlist() { }
        public Playlist(string name, List<Track> tracks = null)
        {
            /*if (String.IsNullOrEmpty(name)) throw new InvalidDataException();
            Name = name;
            Tracks = tracks ?? new List<Track>();*/
        }

        public void AddTrack(Track track)
        {
            Tracks.Add(track);
        }

        public void RemoveTrack(Track track)
        {
            Tracks.Remove(track);
        }
    }
}
