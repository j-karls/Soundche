using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public interface IDatabaseController
    {
        // Needs to be initialized as a singleton, such that all sessions use the same connection to the database

        void InitializeDb();

        // Change these to "saveplaylist" and "getplaylist". Move rest to playlist controller
        //Playlist CreateNewPlaylist();
        //Playlist AddToPlaylist(Playlist playlist);
        //Playlist GetPlaylist(string name);
        //List<string> GetUserInfo(string username);
        public User GetSpecificUser(string username);

    }
}
