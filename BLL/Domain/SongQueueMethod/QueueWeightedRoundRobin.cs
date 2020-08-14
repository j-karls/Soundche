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
            double decimalRand = _rand.Next(100) / 100; // Values range from 0.00 to 0.99
            var tup = _tuple.First(x => decimalRand < x.cumsumSongPercentage); 
            // By the nature of cumulative sums, there is always going to be one playlist with a cumsumsongpercentage of 1.00
            // meaning that we always select one playlist at random
            // the more tracks a playlist, the more likely it is to be selected

            Track nextTrack = tup.playlist.Tracks[tup.trackIdx];

            // Update the track index of the corresponding playlist 
            tup.trackIdx = (tup.trackIdx + 1) % tup.playlist.Tracks.Count;

            return nextTrack;
        }
    }
}
