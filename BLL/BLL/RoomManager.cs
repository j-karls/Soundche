using Soundche.Core.Domain;
using Soundche.Core.Domain.SongQueueMethod;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Soundche.Core.BLL
{
    public class RoomManager
    {
        public SwitchedSongEventArgs _lastSwitchedSongEvent { get; private set; } = null; 

        public Timer PlaybackTimer { get; set; }
        public Track CurrentTrack = null;

        private PlaylistManager PlaylistController { get; set; }
        private IDatabaseManager DatabaseController { get; set; }

        // This class is the singleton - as such, only one room is currently allowed in the program
        // It is the top level class that manages the database, the playlistmanager (which controlls which song to play) and
        // basically is the room (that manages the people that are inside, wanting to listen to songs together)

        public RoomManager()
        {
            DatabaseController = new LiteDbManager();
            PlaylistController = new PlaylistManager();
            SwitchedSongEvent += OnSwitchSong; // Raise the lastSwitchedSongEvent every time the song is switched
            PlaybackTimer = new Timer { AutoReset = false };
            PlaybackTimer.Elapsed += OnTimerElapsed;
        }

        public event EventHandler<SwitchedSongEventArgs> SwitchedSongEvent;

        private void OnSwitchSong(object sender, SwitchedSongEventArgs e)
        {
            _lastSwitchedSongEvent = e;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => StartNextSong(); 

        private void StartTimer(double interval)
        {
            PlaybackTimer.Stop();
            PlaybackTimer.Interval = interval;
            PlaybackTimer.Start();
        }

        public void StartNextSong() => StartTrack(PlaylistController.GetNextTrack());
        public void StartPreviousSong() => StartTrack(PlaylistController.GetPreviousTrack());

        private void StartTrack(Track newTrack)
        {
            if (newTrack == null) return;
            CurrentTrack = newTrack;
            StartTimer((newTrack.EndTime * 1000) - (newTrack.StartTime * 1000)); // Convert s to ms
            SwitchedSongEvent(this, new SwitchedSongEventArgs(newTrack, DateTime.Now));
        }

        public void StopPlaying()
        {
            PlaybackTimer.Stop();
            CurrentTrack = null;
            _lastSwitchedSongEvent = null;
        }

        public void ConnectPlaylist(Playlist playlist, User user)
        {
            // Adds the playlist to the playback and starts the playback (if it isn't already started)
            // Note that adding one playlist multiple times is allowed and should not cause any issues

            PlaylistController.AddPlaylist(playlist, user);
            if (CurrentTrack == null) StartNextSong();
        }

        public List<Playlist> GetConnectedPlaylists()
        {
            return PlaylistController.ActivePlaylists.pl;
        }

        public void DisconnectPlaylist(Playlist playlist, User user)
        {
            // Removes playlist - playback will stop after the end of the current song
            PlaylistController.RemovePlaylist(playlist, user);
        }

        public User GetUser(string username) => DatabaseController.GetUser(username);
        public void AddUser(User user) => DatabaseController.AddUser(user);
        public void UpdateUser(User user) => DatabaseController.UpdateUser(user);

        public void SwitchQueueMethod(SongQueueMethodEnum newType) => PlaylistController.SwitchSongQueueMethod(newType);
        public Track GetNextSong() => PlaylistController.GetNextTrack();
        public Track GetPreviousSong() => PlaylistController.GetPreviousTrack();
        // TODO Brug til forhåndsvisning af forrige og næste sang
        public void DisconnectAllPlaylists() => PlaylistController.RemoveAllPlaylists();
    }
}
