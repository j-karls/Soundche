using Soundche.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Soundche.Core.BLL
{
    public class RoomManager
    {
        public Timer PlaybackTimer = null;
        public Track CurrentTrack = null;

        public PlaylistManager PlaylistController { get; private set; }
        public IDatabaseManager DatabaseController { get; private set; }

        // This class is the singleton - as such, only one room is currently allowed in the program
        // It is the top level class that manages the database, the playlistmanager (which controlls which song to play) and
        // basically is the room (that manages the people that are inside, wanting to listen to songs together)

        public RoomManager()
        {
            DatabaseController = new LiteDbManager();
            PlaylistController = new PlaylistManager();
        }

        public event EventHandler<SwitchedSongEventArgs> SwitchedSongEvent;

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => StartNewSong();

        private void StartTimer(double interval)
        {
            PlaybackTimer = new Timer(interval);
            PlaybackTimer.Elapsed += OnTimerElapsed;
            PlaybackTimer.AutoReset = false;
            PlaybackTimer.Start();
        }

        private void StartNewSong()
        {
            Track newTrack = PlaylistController.GetNextTrack();
            if (newTrack == null) return; 

            CurrentTrack = newTrack;
            StartTimer((newTrack.EndTime * 1000) - (newTrack.StartTime * 1000)); // Convert ms to s
            SwitchedSongEvent(this, new SwitchedSongEventArgs(newTrack, DateTime.Now));
        }

        public void SkipSong() => StartNewSong();

        public void ConnectPlaylist(Playlist playlist)
        {
            // Adds the playlist to the playback and starts the playback (if it isn't already started)

            // TODO: Maybe there should be a check to see if the playlist is already on the list? Or maybe possibility of a deepcopy, if adding the playlist twice is something we want to allow?

            PlaylistController.AddPlaylist(playlist);
            if (CurrentTrack == null) StartNewSong();
        }

        public void DisconnectPlaylist(Playlist playlist)
        {
            // Removes playlist - playback will stop after the end of the current song
            PlaylistController.RemovePlaylist(playlist);
        }

        public User GetUserInfo(string username) => DatabaseController.GetUser(username);

        public void CallStuff()
        {
            DatabaseController.InsertShitHardcodedUserBoi();
        }

        public void GetStuff()
        {
            var u = DatabaseController.GetShit();
        }
    }
}
