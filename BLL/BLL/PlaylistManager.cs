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
        public List<(Playlist pl, User usr)> ActivePlaylists { get; private set; } = new List<(Playlist pl, User usr)>();
        public List<TrackRequest> PreviousSongs { get; private set; } = new List<TrackRequest>();
        public TrackRequest CurrentSong = null;

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

        public TrackRequest GetNextTrack() // TODO Brug til forhåndsvisning af næste sang. Det er ikke helt så simpelt. Men jeg bør nok reworke mine songqueuefuncs alligevel
        {
            if(CurrentSong != null) PreviousSongs.Add(CurrentSong);
            if (PreviousSongs.Count > 10) PreviousSongs.RemoveAt(0);
            CurrentSong = ActivePlaylists.Select(x => x.pl).ToList().IsNullOrEmpty() ? null : _queueFunc.Next();
            return CurrentSong;
        }

        public TrackRequest GetPreviousTrack() 
        {
            if (PreviousSongs.IsNullOrEmpty()) return null;
            TrackRequest last =  PreviousSongs.Last();
            PreviousSongs.RemoveAt(PreviousSongs.Count - 1);
            return last;
        }

        public void AddPlaylist(Playlist playlist, User user)
        {
            // Add a playlist to the total playback and reload our queue method
            ActivePlaylists.Add((playlist, user));
            SwitchSongQueueMethod(SongQueueType);

            // TODO: This likely has some problems regarding which song we got to. So we should probably like 
            // to add the playlist gracefully to an existing songqueuemethod somehow. Probably add 
            // "AddPlaylist" to the IQueueMethod interface 
        }

        public void RemovePlaylist(Playlist playlist, User user)
        {
            bool success = false;

            for (int i = 0; i < ActivePlaylists.Count; i++)
            {
                if(ActivePlaylists[i].usr.Name == user.Name && ActivePlaylists[i].pl.Name == playlist.Name)
                {
                    ActivePlaylists.RemoveAt(i);
                    success = true;
                }
            }

            if (!success) throw new DataMisalignedException("Playlist could not be removed");
        }

        public void RemoveAllPlaylists()
        {
            ActivePlaylists = new List<(Playlist pl, User usr)>();
        }
    }
}
