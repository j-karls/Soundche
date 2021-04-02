using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public class QueueRandomize : IQueueMethod
    {
        private List<TrackRequest> _tracks = new List<TrackRequest>();
        private List<(Playlist pl, User usr)> _playlists;
        private Random _rand = new Random();

        public QueueRandomize(List<(Playlist pl, User usr)> playlists)
        {
            _playlists = playlists;
            ReloadPlaylists();
        }

        private void ReloadPlaylists()
        {
            foreach ((Playlist playlist, User user) in _playlists)
            {
                if (playlist.Tracks.IsNullOrEmpty()) continue;
                _tracks.AddRange(playlist.Tracks.Select(x => new TrackRequest(x, user, playlist.Name)));
            }
        }

        public TrackRequest Next()
        {
            TrackRequest t = _tracks[_rand.Next(_tracks.Count)];
            return t.Song.Exclude ? Next() : t;
        }

        public void AddPlaylist(Playlist playlist, User user)
        {
            _playlists.Add((playlist, user));
            ReloadPlaylists();
        }

        public void RemovePlaylist(Playlist playlist, User user)
        {
            int i = _playlists.IndexOf((playlist, user));
            if (i < 0) throw new InvalidOperationException("No such playlist was found");
            _playlists.RemoveAt(i);
        }

        public string GetProgress(TrackRequest currentSong)
        {
            return "Randomize details:\nSelects next song randomly from the pool.\n--------------------\n\n" + 
                String.Join("\n", _tracks.Select(x => x.Song.ToReadableString()));
        }
    }
}
