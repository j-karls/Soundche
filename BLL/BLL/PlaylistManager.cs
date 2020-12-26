using Soundche.Core.Domain;
using Soundche.Core.Domain.SongQueueMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soundche.Core.BLL
{
    public class PlaylistManager
    {
        public SongQueueMethodEnum SongQueueType { get; private set; } = SongQueueMethodEnum.Randomize;
        private IQueueMethod _queueFunc;
        public List<Playlist> ActivePlaylists { get; private set; } = new List<Playlist>();
        public List<Track> PreviousSongs { get; private set; } = new List<Track>();
        public Track CurrentSong = null;

        public PlaylistManager() { }

        public void SwitchSongQueueMethod(SongQueueMethodEnum queueType)
        {
            _queueFunc = queueType switch
            {
                SongQueueMethodEnum.Randomize => new QueueRandomize(ActivePlaylists),
                SongQueueMethodEnum.RoundRobin => new QueueRoundRobin(ActivePlaylists),
                SongQueueMethodEnum.WeightedRoundRobin => new QueueWeightedRoundRobin(ActivePlaylists),
                _ => throw new NotImplementedException(),
            };
        }

        public Track GetNextTrack() // TODO Brug til forhåndsvisning af næste sang. Det er ikke helt så simpelt. Men jeg bør nok reworke mine songqueuefuncs alligevel
        {
            if(CurrentSong != null) PreviousSongs.Add(CurrentSong);
            if (PreviousSongs.Count > 10) PreviousSongs.RemoveAt(0);
            CurrentSong = ActivePlaylists.IsNullOrEmpty() ? null : _queueFunc.Next();
            return CurrentSong;
        }

        public Track GetPreviousTrack() 
        {
            if (PreviousSongs.IsNullOrEmpty()) return null;
            Track last =  PreviousSongs.Last();
            PreviousSongs.RemoveAt(PreviousSongs.Count - 1);
            return last;
        }

        public void AddPlaylist(Playlist playlist)
        {
            // Add a playlist to the total playback and reload our queue method
            ActivePlaylists.Add(playlist);
            SwitchSongQueueMethod(SongQueueType);

            // TODO: This likely has some problems regarding which song we got to. So we should probably like 
            // to add the playlist gracefully to an existing songqueuemethod somehow. Probably add 
            // "AddPlaylist" to the IQueueMethod interface 
        }

        public void RemovePlaylist(Playlist playlist)
        {
            bool success = ActivePlaylists.Remove(playlist);
            if (!success) throw new DataMisalignedException("Playlist could not be removed");
        }
    }
}
