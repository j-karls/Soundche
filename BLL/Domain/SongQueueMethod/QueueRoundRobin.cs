using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public class QueueRoundRobin : IQueueMethod
    {
        private List<(Playlist playlist, User user, int trackIdx)> _tuple;
        private int _playlistIdx = 0;

        public QueueRoundRobin(List<(Playlist pl, User usr)> playlists)
        {
            _tuple = playlists.Select(x => (playlist: x.pl, user: x.usr, trackIdx: 0)).ToList();
        }

        public TrackRequest Next()
        {
            // Return null if all playlists are empty
            if (_tuple.All(x => x.playlist.Tracks.IsNullOrEmpty())) return null;
            
            // Get next track
            var tup = _tuple[_playlistIdx];
            TrackRequest nextTrack = tup.playlist.Tracks.IsNullOrEmpty() ? null : new TrackRequest(tup.playlist.Tracks[tup.trackIdx], tup.user);

            // Update indexes
            if (nextTrack != null) // If playlist didn't contain a track, there's no need to update inner trackIdx
                tup.trackIdx = (tup.trackIdx + 1) % tup.playlist.Tracks.Count;
            _tuple[_playlistIdx] = tup;
            _playlistIdx = (_playlistIdx + 1) % _tuple.Count;

            return (nextTrack is null || nextTrack.Song.Exclude) ? Next() : nextTrack; // if the next song is excluded, then just find the next one again
        }

        public void AddPlaylist(Playlist playlist, User user)
        {
            _tuple.Add((playlist, user, 0));
        }

        public void RemovePlaylist(Playlist playlist, User user)
        {
            _tuple.RemoveAll(x => x.playlist == playlist && x.user == user);
        }
    }
}
