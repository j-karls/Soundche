using Soundche.Core.Domain;
using Soundche.Core.Domain.SongQueueMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Soundche.Core.BLL
{
    public class RoomManager
    {
        public SwitchedSongEventArgs LastSwitchedSongEvent { get; private set; } = null; 

        public Timer PlaybackTimer { get; set; }
        public TrackRequest CurrentTrack = null;

        private PlaylistManager PlaylistController { get; set; }
        private IDatabaseManager DatabaseController { get; set; }

        public Timer ActiveUserTimer { get; set; }
        public List<string> ActiveUsers { get; private set; } = new List<string>();
        private List<string> aliveUserSessionRequests = new List<string>();

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

            ActiveUserTimer = new Timer { AutoReset = true, Interval = 10000 };
            ActiveUserTimer.Elapsed += CleanActiveUsers;
            ActiveUserTimer.Start();
        }

        public void NotifyUserSessionRequest(string username) => aliveUserSessionRequests.Add(username);
        private void CleanActiveUsers(object sender, ElapsedEventArgs e)
        {
            ActiveUsers = aliveUserSessionRequests.Distinct().ToList();
            aliveUserSessionRequests = new List<string>();
        }


        public event EventHandler<SwitchedSongEventArgs> SwitchedSongEvent;

        private void OnSwitchSong(object sender, SwitchedSongEventArgs e)
        {
            LastSwitchedSongEvent = e;
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

        private void StartTrack(TrackRequest newTrack)
        {
            if (newTrack == null) return;
            CurrentTrack = newTrack;
            StartTimer((newTrack.Song.EndTime * 1000) - (newTrack.Song.StartTime * 1000)); // Convert s to ms
            SwitchedSongEvent(this, new SwitchedSongEventArgs(newTrack, DateTime.Now));
        }

        public void StopPlaying()
        {
            PlaybackTimer.Stop();
            CurrentTrack = null;
            LastSwitchedSongEvent = null;
        }

        public void ConnectPlaylist(Playlist playlist, User user)
        {
            // Adds the playlist to the playback and starts the playback (if it isn't already started)
            // Note that adding one playlist multiple times is allowed and should not cause any issues

            PlaylistController.AddPlaylist(playlist, user);
            if (CurrentTrack == null) StartNextSong();
        }

        public (List<User> usr, List<Playlist> pl) GetConnectedPlaylists()
        {
            // TODO Fix, gør simpler
            return (PlaylistController.ActivePlaylists.Select(x => x.usr).ToList(), PlaylistController.ActivePlaylists.Select(x => x.pl).ToList());
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
        public TrackRequest GetNextSong() => PlaylistController.GetNextTrack();
        public TrackRequest GetPreviousSong() => PlaylistController.GetPreviousTrack();
        // TODO Brug til forhåndsvisning af forrige og næste sang
        public void DisconnectAllPlaylists() => PlaylistController.RemoveAllPlaylists();

        public string GetProgress() => PlaylistController.GetProgress();
    }
}
