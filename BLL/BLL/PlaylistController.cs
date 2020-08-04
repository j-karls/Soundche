using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.BLL
{
    public class PlaylistController
    {
        public enum SongQueueMethod // How do we select the next song?
        { 
            Randomize,          // Put all songs from all playlists into a pot, randomly select one 
            RoundRobin,         // Like good ol' plugdj - allow each playlist to play one song, then onto the next one, round and round
            WeightedRoundRobin  // We go around like roundrobin, but each playlist is selected according to how many songs it contains
                                // If one playlist has twice the songs of another, we will skip the short one in half the rounds
        } 

        public SongQueueMethod SongQueueType { get; set; }

        //private func songfunc
        
        private Track GetTrackRandomize()
        {
            // Flatten list, then get random element
            Random rand = new Random();
            Track[] flat = ActivePlaylists.SelectMany(x => x.Tracks).ToArray();
            return flat[rand.Next(flat.Length)];
        }

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
