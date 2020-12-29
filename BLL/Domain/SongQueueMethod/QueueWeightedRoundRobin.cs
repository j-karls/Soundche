using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public class QueueWeightedRoundRobin : IQueueMethod
    {
        private List<(Playlist playlist, int trackIdx, double cumsumSongPercentage)> _tuple;
        Random _rand = new Random();

        public QueueWeightedRoundRobin(List<Playlist> playlists) => Initialize(playlists);

        private void Initialize(List<Playlist> playlists, List<int> trackStartIdxs = null)
        {
            // if (playlists.IsNullOrEmpty())
            // TODO DO NULL CHECKS

            trackStartIdxs = trackStartIdxs ?? playlists.Select(x => 0).ToList(); 
            double totalNumTracks = playlists.Select(x => x.Tracks.Count).Aggregate((x, y) => x + y);
            List<double> cumsum = playlists.Select(x => x.Tracks.Count / totalNumTracks).CumulativeSum().ToList();
            _tuple = (from i 
                      in Enumerable.Range(0, playlists.Count) 
                      select (playlist: playlists[i], trackIdx: trackStartIdxs[i], cumsumSongPercentage: cumsum[i])).ToList();

            // Cumsumsongpercentage is the cumulative chance for the playlist to be selected to play next. 
        }

        public Track Next()
        {
            // Return null if all playlists are empty
            if (_tuple.All(x => x.playlist.Tracks.IsNullOrEmpty())) return null;

            // Get next track
            double decimalRand = ((double) _rand.Next(100)) / 100; // Values range from 0.00 to 0.99
            int chosenIdx = _tuple.FindIndex(x => decimalRand < x.cumsumSongPercentage);
            var tup = _tuple[chosenIdx];
            // By the nature of cumulative sums, there is always going to be one playlist with a cumsumsongpercentage of 1.00
            // meaning that we always select one playlist at random
            // the more tracks a playlist, the more likely it is to be selected

            Track nextTrack = tup.playlist.Tracks[tup.trackIdx];

            // Update the track index of the corresponding playlist 
            tup.trackIdx = (tup.trackIdx + 1) % tup.playlist.Tracks.Count;
            _tuple[chosenIdx] = tup;

            return nextTrack.Exclude ? Next() : nextTrack; // if the next song is excluded, then just find the next one again
        }

        public void AddPlaylist(Playlist playlist)
        {
            List<Playlist> playlists = _tuple.Select(x => x.playlist).ToList();
            List<int> trackIdxs = _tuple.Select(x => x.trackIdx).ToList();
            playlists.Add(playlist);
            trackIdxs.Add(0);
            Initialize(playlists, trackIdxs);
        }

        public void RemovePlaylist(Playlist playlist)
        {
            List<Playlist> playlists = _tuple.Select(x => x.playlist).ToList();
            List<int> trackIdxs = _tuple.Select(x => x.trackIdx).ToList();
            playlists.Add(playlist);
            trackIdxs.Add(0);
            // Adds the new playlist to the collected playlists, and initializes it to play its 0th song when selected
            Initialize(playlists, trackIdxs);
        }
    }
}
