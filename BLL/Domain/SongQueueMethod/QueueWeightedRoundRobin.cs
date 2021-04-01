using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public class QueueWeightedRoundRobin : IQueueMethod
    {
        private List<(Playlist playlist, User user, int trackIdx, double cumsumSongPercentage)> _tuple;
        private Random _rand = new Random();

        public QueueWeightedRoundRobin(List<(Playlist pl, User usr)> playlists) => Initialize(playlists.Select(x => x.pl).ToList(), playlists.Select(x => x.usr).ToList());

        private void Initialize(List<Playlist> playlists, List<User> users, List<int> trackStartIdxs = null)
        {
            if (playlists.IsNullOrEmpty()) return;

            trackStartIdxs = trackStartIdxs ?? playlists.Select(x => 0).ToList(); 
            double totalNumTracks = playlists.Select(x => x.Tracks.Count).Aggregate((x, y) => x + y);
            List<double> cumsum = playlists.Select(x => x.Tracks.Count / totalNumTracks).CumulativeSum().ToList();
            _tuple = (from i 
                      in Enumerable.Range(0, playlists.Count) 
                      select (playlist: playlists[i], user: users[i], trackIdx: trackStartIdxs[i], cumsumSongPercentage: cumsum[i])).ToList();

            // Cumsumsongpercentage is the cumulative chance for the playlist to be selected to play next. 
        }

        public TrackRequest Next()
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

            TrackRequest nextTrack = new TrackRequest(tup.playlist.Tracks[tup.trackIdx], tup.user);

            // Update the track index of the corresponding playlist 
            tup.trackIdx = (tup.trackIdx + 1) % tup.playlist.Tracks.Count;
            _tuple[chosenIdx] = tup;

            return nextTrack.Song.Exclude ? Next() : nextTrack; // if the next song is excluded, then just find the next one again
        }

        public void AddPlaylist(Playlist playlist, User user)
        {
            List<Playlist> playlists = _tuple.Select(x => x.playlist).ToList();
            List<User> users = _tuple.Select(x => x.user).ToList();
            List<int> trackIdxs = _tuple.Select(x => x.trackIdx).ToList();
            playlists.Add(playlist);
            users.Add(user);
            trackIdxs.Add(0);
            // Adds the new playlist to the collected playlists, and initializes it to play its 0th song when selected
            Initialize(playlists, users, trackIdxs);
        }

        public void RemovePlaylist(Playlist playlist, User user)
        {
            List<Playlist> playlists = _tuple.Select(x => x.playlist).ToList();
            List<User> users = _tuple.Select(x => x.user).ToList();
            List<int> trackIdxs = _tuple.Select(x => x.trackIdx).ToList();
            int i = playlists.IndexOf(playlist);
            playlists.RemoveAt(i);
            users.RemoveAt(i);
            trackIdxs.RemoveAt(i);
            Initialize(playlists, users, trackIdxs);
        }

        public string GetProgress()
        {
            /*
            Playlist:    A    B    C
            Size:        28   98   76
            Finished %:  20%  19%  25%
            Songs:       X1   X2   X3
                        -Y1   Y2   Y3
                         Z1  -Z2  -Z3
            */
        }
    }
}
