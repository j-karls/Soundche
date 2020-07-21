using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.BLL
{
    public class PlaylistController
    {
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
    }
}
