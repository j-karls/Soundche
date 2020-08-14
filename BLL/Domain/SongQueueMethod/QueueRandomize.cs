using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Domain.SongQueueMethod
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
            Track[] flat = _playlists.SelectMany(x => x.Tracks).ToArray();
            return flat[_rand.Next(flat.Length)];
        }
    }
}
