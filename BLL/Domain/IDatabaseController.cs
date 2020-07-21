using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public interface IDatabaseController
    {
        void StartPlayback(Playlist playlist);


        // Needs to be initialized as a singleton, such that all sessions use the same connection to the database


        //public void /*callback type?*/ StartListening(/*callback shit here, some even handler stuff, this function then subscribes those to the finish event*/);

        //public void StartListening();
        //public event SwitchedSongEventHandler;
        event EventHandler<SwitchedSongEventArgs> SwitchedSongEvent; // this should probably be moved to outside this class maybe?

        Playlist CreateNewPlaylist();
        Playlist AddToPlaylist(Playlist playlist);
        Playlist GetPlaylist(string name);
    }
}
