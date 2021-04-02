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
                foreach (Track track in playlist.Tracks)
                {
                    if (!track.Exclude) _tracks.Add(new TrackRequest(track, user, playlist.Name));
                }
            }
        }

        public TrackRequest Next()
        {
            return _tracks[_rand.Next(_tracks.Count)];
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
            // TODO
            throw new NotImplementedException();
        }
    }
}
