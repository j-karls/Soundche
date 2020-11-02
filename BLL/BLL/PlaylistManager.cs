﻿using Soundche.Core.Domain;
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

        public Track GetNextTrack() // TODO Brug til forhåndsvisning af næste sang
        {
            return ActivePlaylists.IsNullOrEmpty() ? null : _queueFunc.Next();
        }

        public Track GetPreviousTrack() 
        {
            throw new NotImplementedException();
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
