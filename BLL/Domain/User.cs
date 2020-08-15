using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Soundche.Core.Domain
{
    public class User
    {
        public string Name { get; set; }
        public List<Playlist> Playlists { get; set; }

        public User() { } // Public empty ctor necessary for database serialization

        public static User CreateUser(string name, List<Playlist> playlists = null)
        {
            if (String.IsNullOrEmpty(name)) throw new InvalidDataException();

            return new User { Name = name, Playlists = playlists ?? new List<Playlist>() };
        }

        public Playlist GetPlaylist(string name)
        {
            return Playlists.First(x => x.Name == name);
        }
    }
}
