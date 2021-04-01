using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public interface IQueueMethod 
    {
        // Currently it's set up in the way where you can remove and add playlists at will, and it will update fine. 
        // But you cannot remove or add songs - you would have to requeue it. It's probably worth just creacting an "updatequeuefunction" or something
        
        public TrackRequest Next();

        // Provides a graceful update of our queue method, one that doesn't reset everything
        public void AddPlaylist(Playlist playlist, User user);
        public void RemovePlaylist(Playlist playlist, User user);
        public string GetProgress();
    }

}
