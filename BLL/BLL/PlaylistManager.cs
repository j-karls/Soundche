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
        public SongQueueMethodEnum SongQueueType { get; private set; }
        private IQueueMethod _queueFunc;
        public List<Playlist> ActivePlaylists { get; private set; }

        public PlaylistManager(SongQueueMethodEnum queueType = SongQueueMethodEnum.Randomize /*TODO WeightedRoundRobin should be default*/)
        {
            ActivePlaylists = new List<Playlist>();
            SwitchSongQueueMethod(queueType);
        }

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

        public Track GetNextTrack()
        {
            return ActivePlaylists.IsNullOrEmpty() ? null : _queueFunc.Next();
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
