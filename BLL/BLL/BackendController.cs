using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.BLL
{
    public class BackendController
    {
        public PlaylistController playlistController { get; set; }
        public IDatabaseController databaseController { get; set; }

        // RoomController ??


        // This class is the singleton
        // It is the top level class that manages the database, the playlistmanager (which controlls which song to play) and 
        // the room (the people that are inside, wanting to listen)


        // It also raises the database event to the top, so that the individual clients can react to it


        // timer
        // current song
        // start playback
        // connect playlist
        // save playlist ?
        // load playlist ?
        // Aggregates playlist manager and database manager and maybe a room manager, if this ends up making sense
    }
}
