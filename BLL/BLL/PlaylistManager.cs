using Core.Domain;
using Core.Domain.SongQueueMethod;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.BLL
{
    public class PlaylistManager
    {
        public SongQueueMethodEnum SongQueueType { get; private set; }
        private IQueueMethod _queueFunc;
        public List<Playlist> ActivePlaylists { get; private set; }

        public PlaylistManager(List<Playlist> playlists, SongQueueMethodEnum queueType = SongQueueMethodEnum.WeightedRoundRobin)
        {
            ActivePlaylists = playlists;
            SwitchSongQueueMethod(queueType);
        }

        public void SwitchSongQueueMethod(SongQueueMethodEnum queueType)
        {
            switch (queueType)
            {
                case SongQueueMethodEnum.Randomize: _queueFunc = new QueueRandomize(ActivePlaylists);
                    break;
                case SongQueueMethodEnum.RoundRobin: _queueFunc = new QueueRoundRobin(ActivePlaylists);
                    break;
                case SongQueueMethodEnum.WeightedRoundRobin: _queueFunc = new QueueWeightedRoundRobin(ActivePlaylists);
                    break;
                default: throw new NotImplementedException();
            }
        }

        public Track GetNextTrack()
        {
            if (ActivePlaylists.IsNullOrEmpty()) throw new DataMisalignedException();
            return _queueFunc.Next();
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
    }
}
