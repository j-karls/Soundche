using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain
{
    public class TrackRequest
    {
        /// <summary>
        /// A trackRequest contains a track and all the surrounding metadata about who is trying to play the song
        /// </summary>
        public Track Song { get; set; }
        public User DJ { get; set; }
        public string PlaylistName { get; set; }
        public TrackRequest(Track song, User dj, string playlistName)
        {
            Song = song;
            DJ = dj;
            PlaylistName = playlistName;
        }
    }
}
