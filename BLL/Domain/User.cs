using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Domain
{
    public class User
    {
        public string Name { get; set; }
        public List<Playlist> Playlists { get; set; }


        public User() { } // Public empty ctor necessary for database
        public User(string name, List<Playlist> playlists = null)
        {
            /*if (String.IsNullOrEmpty(name)) throw new InvalidDataException();
            Name = name;
            Playlists = playlists ?? new List<Playlist>();*/
        }
    }
}
