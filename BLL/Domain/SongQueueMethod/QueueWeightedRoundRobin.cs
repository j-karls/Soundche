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
        private readonly Random _rand = new Random();

        public QueueWeightedRoundRobin(List<(Playlist pl, User usr)> playlists) => Initialize(playlists.Select(x => x.pl).ToList(), playlists.Select(x => x.usr).ToList());

        private void Initialize(List<Playlist> playlists, List<User> users, List<int> trackStartIdxs = null)
        {
            if (playlists.IsNullOrEmpty()) return;

            trackStartIdxs ??= playlists.Select(x => 0).ToList(); 
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

            TrackRequest nextTrack = new TrackRequest(tup.playlist.Tracks[tup.trackIdx], tup.user, tup.playlist.Name);

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

        public string GetProgress(TrackRequest currentSong, int currentSongProgress = 0) 
        {
            // TODO Rewrite this such that it looks like ordinary round robin's getProgress. 

            // Creates an ascii matrix containing progress info from all the playlists
            string format = "{0,-15} " + String.Join("", Enumerable.Range(1, _tuple.Count).Select(x => $"{{{ x },-25}} ")) + "\n";
            var ls1 = new List<string> { "Playlist: "   }; ls1.AddRange(_tuple.Select(x => x.playlist.Name));
            var ls2 = new List<string> { "Size: "       }; ls2.AddRange(_tuple.Select(x => x.playlist.Tracks.Count.ToString()));
            var ls3 = new List<string> { "Cumsum: "     }; ls3.AddRange(_tuple.Select(x => x.cumsumSongPercentage.ToString()));
            var ls4 = new List<string> { "Finished: "   }; ls4.AddRange(_tuple.Select(x => (100 * ((double) x.trackIdx / x.playlist.Tracks.Count)).ToString("N1") + "%"));
            var ls5 = new List<string> { "Songs: "      }; ls5.AddRange(Enumerable.Range(0, _tuple.Count).Select(x => "------"));

            string data = "";
            foreach (var ls in new [] { ls1, ls2, ls3, ls4, ls5 })
                data += String.Format(format, ls.ToArray());

            int playedTime = 0;
            var playlistStrings = new List<List<string>>();
            foreach (var (playlist, user, trackIdx, _) in _tuple)
            {
                var playlistContainsCurrentSong = currentSong.DJ == user && currentSong.PlaylistName == playlist.Name;
                playlistStrings.Add(QueueProgressHelper.RoundRobinGetPlaylistAsString(playlist, trackIdx, playlistContainsCurrentSong, 25));
                playedTime += QueueProgressHelper.GetPlaylistPlayedTime(playlist, trackIdx, playlistContainsCurrentSong ? currentSongProgress : 0);
            }

            for (int i = 0; i < _tuple.Max(x => x.playlist.Tracks.Count); i++)
            {
                var s = new List<string>(_tuple.Count) { " " };
                foreach (var playlistString in playlistStrings)
                    s.Add(playlistString.ElementAtOrDefault(i));
                data += String.Format(format, s.ToArray());
            }

            var totalPlaytime = _tuple.Select(x => x.playlist.Playtime).Aggregate((x, y) => x + y);
            return $"WeightedRoundRobin details:\nTODO Description\nTotal progress { playedTime }/{ totalPlaytime } \n--------------------\n\n" + data;
        }
    }
}
