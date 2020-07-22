using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.BLL
{
    public class SessionController
    {
        // One session is one person

        // Has a list of playlists (or maybe just one, in the case of our MVP)
        // Each playlist is a list of songs
        // It has a OnSwitchedSong function

        public void SwitchCurrentPlayback(SwitchedSongEventArgs arg)
        {
            // This function is subscribed to the switchedsong event on our singleton
            // This function changes the variable that our HTML5 iframe is bound to

            throw new NotImplementedException();
        }
    }
}
