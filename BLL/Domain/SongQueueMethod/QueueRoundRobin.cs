using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public class QueueRoundRobin : IQueueMethod
    {
        private List<(Playlist playlist, int trackIdx)> _tuple;
        private int _playlistIdx = 0;

        public QueueRoundRobin(List<Playlist> playlists)
        {
            _tuple = playlists.Select(x => (playlist: x, trackIdx: 0)).ToList();
        }

        public Track Next()
        {
            // Get next track
            var tup = _tuple[_playlistIdx];
            Track nextTrack = tup.playlist.Tracks[tup.trackIdx];

            // Update indexes
            _playlistIdx = (_playlistIdx + 1) % _tuple.Count;
            tup.trackIdx = (tup.trackIdx + 1) % tup.playlist.Tracks.Count;

            return nextTrack.Exclude ? Next() : nextTrack; // if the next song is excluded, then just find the next one again
        }

        public void AddPlaylist(Playlist playlist)
        {
            _tuple.Add((playlist, 0));
        }

        public void RemovePlaylist(Playlist playlist)
        {
            _tuple.RemoveAll(x => x.playlist == playlist);
        }
    }
}
