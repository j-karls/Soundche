using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Linq;

namespace Soundche.Core.Domain
{
    public class Playlist
    {
        [Required] [BsonId]
        public string Name { get; set; }
        public List<Track> Tracks { get; set; }
        public int Playtime => Tracks.Select(x => x.Exclude ? 0 : x.Playtime).Aggregate((x,y) => x + y);

        public Playlist() { }

        public Playlist(string name, List<Track> tracks = null)
        {
            if (String.IsNullOrEmpty(name)) throw new InvalidDataException();
            Name = name;
            Tracks = tracks ?? new List<Track>();
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
