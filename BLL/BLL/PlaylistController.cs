using Core.Domain;
using System;
using System.Collections.Generic;
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

        public List<Playlist> ActivePlaylists { get; private set; }


        //private database repository; 
        public Playlist ActivePlaylist { get; set; }
        public PlaylistController()
        {
            //repository = new database{};
        }

        public void SetActivePlaylist (string playlistName)
        {
            ActivePlaylist = GetPlaylist(playlistName);
            //repository.SetActivePlaylist(playlistName);
        }

        public Playlist GetPlaylist(string playlistName)
        {
            return null; //repository.GetPlaylist(playlistName);
        }

        public Playlist GetPlaylists()
        {
            return null; //repository.GetPlaylists();
        }

        public Playlist NewPlaylist(string playlistName)
        {
            new Playlist { Name = playlistName };
            //return repository.NewPlaylist(new Playlist{});
            return null;
        }
        public Playlist Addtrack(Track track, Playlist playlist)
        {
            playlist.AddTrak(track);
            Playlist newplaylist = null;// = repository.UpdatePlaylist(EditPlaylist);
            return newplaylist;
        }

        public Playlist RemoveTrack (Track track, Playlist playlist)
        {
            playlist.RemoveTrack(track);
            Playlist newplaylist = null; //= repository.UpdatePlaylist(playlist);
            return newplaylist;
        }
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
