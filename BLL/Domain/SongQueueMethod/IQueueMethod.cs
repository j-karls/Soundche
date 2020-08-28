using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public interface IQueueMethod
    {
        public Track Next();

        // Provides a graceful update of our queue method, one that doesn't reset everything
        public void AddPlaylist(Playlist playlist);
        public void RemovePlaylist(Playlist playlist);
    }

}
