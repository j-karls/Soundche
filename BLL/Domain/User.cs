using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Soundche.Core.Domain
{
    public class User
    {
        [Required(AllowEmptyStrings = false)]
        [BsonId] // BsonId is required for when I want to update an existing user, since I have to find it within the DB 
        public string Name { get; set; }
        public List<Playlist> Playlists { get; set; }

        public User() { } // Public empty ctor necessary for database serialization

        public User(string name, List<Playlist> playlists = null)
        {
            if (String.IsNullOrEmpty(name)) throw new InvalidDataException();
            Name = name;
            Playlists = playlists ?? new List<Playlist>();
        }

        public Playlist GetPlaylist(string name)
        {
            return Playlists.First(x => x.Name == name);
        }
    }
}
