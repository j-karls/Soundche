using Core.Domain;
using Core.Domain.SongQueueMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.BLL
{
    public class PlaylistController
    {
        public SongQueueMethodEnum SongQueueType { get; set; }

        //private func songfunc
        
        public List<Playlist> ActivePlaylists { get; private set; }




        public Track GetNewTrack() // Get new track from a playlist
        {
            // Put this into some "collected playlists manager" class
            return new Track("Yir Boi", "urlboi", 0, 10);
        }

        public void AddPlaylist(Playlist playlist)
        {

            // Add a playlist to the total playback
            throw new NotImplementedException();
        }
    }
}
