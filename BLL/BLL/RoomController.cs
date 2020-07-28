using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Core.BLL
{
    public class RoomController
    {
        public Timer PlaybackTimer = null;
        public Track CurrentTrack = null;

        public PlaylistController PlaylistController { get; set; }
        public IDatabaseController DatabaseController { get; set; }

        // This class is the singleton - as such, only one room is currently allowed in the program
        // It is the top level class that manages the database, the playlistmanager (which controlls which song to play) and
        // basically is the room (that manages the people that are inside, wanting to listen to songs together)

        // It also raises the database event to the top, so that the individual clients can react to it

        public RoomController()
        {
            PlaylistController = new PlaylistController();
            DatabaseController = new SQLiteDbController();
        }

        public event EventHandler<SwitchedSongEventArgs> SwitchedSongEvent;

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => StartNewSong();

        private void StartTimer(double interval)
        {
            PlaybackTimer = new Timer(interval);
            PlaybackTimer.Elapsed += OnTimerElapsed; //try using short lambda syntax here
            PlaybackTimer.AutoReset = false;
            PlaybackTimer.Enabled = true;
        }
        private void StartNewSong()
        {
            Track newTrack = PlaylistController.GetNewTrack();
            CurrentTrack = newTrack;
            StartTimer(newTrack.EndTime - newTrack.StartTime);
            SwitchedSongEvent(this, new SwitchedSongEventArgs(newTrack.YoutubeUrl, newTrack.StartTime));
        }

        public void StartPlayback()
        {
            if (CurrentTrack is null) // If no song is running - NOTE, this assumes that there is actually a playlist
            {
                StartNewSong();
            }
        }
        
        public void ConnectPlaylist(Playlist playlist)
        {
            PlaylistController.AddPlaylist(playlist);
        }

        public Playlist GetPlaylist(string name) => DatabaseController.GetPlaylist(name);

        public List<string> GetPlaylistNames(string username) => DatabaseController.GetUserInfo(username); 
        // gets playlist names and other misc publically available stuff

        // connect playlist
        // save playlist ?
        // load playlist ?
        // Aggregates playlist manager and database manager and maybe a room manager, if this ends up making sense
    }
}
