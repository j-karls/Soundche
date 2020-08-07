using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Core.Domain.SongQueueMethod
{
    public class QueueWeightedRoundRobin : IQueueMethod
    {
        private List<(Playlist playlist, int trackIdx, double cumsumSongPercentage)> _tuple;
        Random _rand = new Random();

        public QueueWeightedRoundRobin(List<Playlist> playlists)
        {
            double totalNumTracks = playlists.Select(x => x.Tracks.Count).Aggregate((x,y) => x + y);
            IEnumerable<double> cumsum = playlists.Select(x => x.Tracks.Count / totalNumTracks).CumulativeSum();
            _tuple = playlists.Zip(cumsum).Select(x => (playlist: x.First, trackIdx: 0, cumsumSongPercentage: x.Second)).ToList();

            // Cumsumsongpercentage is the cumulative chance for the playlist to be selected to play next. 
        }

        public Track Next()
        {
            // Get next track
            int rand = _rand.Next(100); /*
            var tup = _tuple[ _playlistIdx];
            Track nextTrack = tup.playlist.Tracks[tup.trackIdx];

            // Update indexes
            _playlistIdx = (_playlistIdx + 1) % _tuple.Count;
            tup.trackIdx = (tup.trackIdx + 1) % tup.playlist.Tracks.Count;

            return nextTrack;*/

            throw new NotImplementedException(); // almost done fuck
        }
    }
}
