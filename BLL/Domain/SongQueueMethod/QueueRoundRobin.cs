using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public class QueueRoundRobin : IQueueMethod
    {
        private readonly List<(Playlist playlist, User user, int trackIdx)> _tuple;
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
            TrackRequest nextTrack = new TrackRequest(tup.playlist.Tracks[tup.trackIdx], tup.user, tup.playlist.Name);

            // Update indexes
            if (nextTrack != null) // If playlist didn't contain a track, there's no need to update inner trackIdx
                tup.trackIdx = (tup.trackIdx + 1) % tup.playlist.Tracks.Count;
            _tuple[_playlistIdx] = tup;
            _playlistIdx = (_playlistIdx + 1) % _tuple.Count;

            return nextTrack.Song.Exclude ? Next() : nextTrack; // if the next song is excluded, then just find the next one again
        }

        public void AddPlaylist(Playlist playlist, User user)
        {
            _tuple.Add((playlist, user, 0));
        }

        public void RemovePlaylist(Playlist playlist, User user)
        {
            _tuple.RemoveAll(x => x.playlist == playlist && x.user == user);
        }

        public string GetProgress(TrackRequest currentSong, int currentSongProgress = 0) 
        {
            // TODO We currently dont have a way to get the elapsed time of the current song in the backend. 
            // I would need to use a different timer class, or get the info from the frontend (alright solution) 
            string matrix = "";
            string format = QueueProgressHelper.CreateStringMatrixFormat(15, 25, _tuple.Count);
            matrix = QueueProgressHelper.AddLineToMatrix(matrix, format, "Playlist:", _tuple.Select(x => x.playlist.Name));
            matrix = QueueProgressHelper.AddLineToMatrix(matrix, format, "Size:",     _tuple.Select(x => x.playlist.Tracks.Count.ToString()));
            matrix = QueueProgressHelper.AddLineToMatrix(matrix, format, "Finished:", _tuple.Select(x => (100 * ((double) x.trackIdx / x.playlist.Tracks.Count)).ToString("N1") + "%"));
            
            int playedTime = 0;
            var playlistStrings = new List<List<string>>();
            foreach (var (playlist, user, trackIdx) in _tuple)
            {
                var playlistContainsCurrentSong = currentSong.DJ == user && currentSong.PlaylistName == playlist.Name;
                playlistStrings.Add(QueueProgressHelper.RoundRobinGetPlaylistAsString(playlist, trackIdx, playlistContainsCurrentSong, 25));
                playedTime += QueueProgressHelper.GetPlaylistPlayedTime(playlist, trackIdx, playlistContainsCurrentSong ? currentSongProgress : 0);
            }

            matrix = QueueProgressHelper.AddManyLinesToMatrix(matrix, format, "Songs:", playlistStrings);
            var totalPlaytime = _tuple.Select(x => x.playlist.Playtime).Aggregate((x, y) => x + y);
            return $"RoundRobin details:\nTODO Description\nTotal progress { playedTime }/{ totalPlaytime } \n--------------------\n\n" + matrix;
        }
    }
}
