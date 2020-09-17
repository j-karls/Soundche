using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public class QueueRandomize : IQueueMethod
    {
        private List<Playlist> _playlists;
        Random _rand = new Random();

        public QueueRandomize(List<Playlist> playlists)
        {
            _playlists = playlists;
        }

        public Track Next()
        {
            // Flatten list, then get random element
            Track[] flat = _playlists.SelectMany(x => x.Tracks).Where(x => !x.Exclude).ToArray();
            return flat[_rand.Next(flat.Length)];
        }

        public void AddPlaylist(Playlist playlist)
        {
            _playlists.Add(playlist);
        }

        public void RemovePlaylist(Playlist playlist)
        {
            bool res = _playlists.Remove(playlist);
            if (res is false) throw new InvalidOperationException("No such playlist was found");
        }
    }
}
