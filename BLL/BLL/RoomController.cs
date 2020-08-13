using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Core.BLL
{
    public class RoomController
    {
        public Timer PlaybackTimer = null;
        public Track CurrentTrack = null;

        public PlaylistManager PlaylistController { get; private set; }
        public IDatabaseManager DatabaseController { get; private set; }

        // This class is the singleton - as such, only one room is currently allowed in the program
        // It is the top level class that manages the database, the playlistmanager (which controlls which song to play) and
        // basically is the room (that manages the people that are inside, wanting to listen to songs together)

        public RoomController()
        {
            DatabaseController = new LiteDbManager();
        }

        public event EventHandler<SwitchedSongEventArgs> SwitchedSongEvent;

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => StartNewSong();

        private void StartTimer(double interval)
        {
            PlaybackTimer = new Timer(interval);
            PlaybackTimer.Elapsed += OnTimerElapsed;
            PlaybackTimer.AutoReset = false;
            PlaybackTimer.Enabled = true;
        }
        private void StartNewSong()
        {
            Track newTrack = PlaylistController.GetNextTrack();
            CurrentTrack = newTrack;
            StartTimer(newTrack.EndTime - newTrack.StartTime);
            SwitchedSongEvent(this, new SwitchedSongEventArgs(newTrack.YoutubeUrl, newTrack.StartTime));
        }

        public void ConnectPlaylist(Playlist playlist)
        {
            if (PlaylistController is null) PlaylistController = new PlaylistManager(new List<Playlist>{ playlist });
            else PlaylistController.AddPlaylist(playlist);
        }

        public User GetUserInfo(string username) => DatabaseController.GetUser(username);

        public Task CallStuff()
        {
            return new Task(DatabaseController.InsertShitHardcodedUserBoi);
        }
    }
}
