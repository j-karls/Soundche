using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Timers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Soundche.Core.BLL;
using Soundche.Core.Domain;
using Soundche.Core.Domain.SongQueueMethod;
using Soundche.Web.Models;

namespace Soundche.Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HangoutController : Controller
    {
        private readonly RoomManager _room;

        public HangoutController(RoomManager room) // TODO: Should get something higher level (backend manager or smt), one that creates multiple rooms
        {
            _room = room;
        }

        public IActionResult Index()
        {
            // Get current user, then get the corresponding playlists
            // Gets the name of the user defined within our authentication cookie
            User user = _room.GetUser(User.Identity.Name);
            if (user is null)
            {
                _room.AddUser(new User(User.Identity.Name));
                user = _room.GetUser(User.Identity.Name);
            }

            return View(new HangoutViewModel { 
                AutoPlay = true,
                Playlists = _room.GetUser(User.Identity.Name).Playlists,
                PlaylistsDropdown = new SelectList(user.Playlists.Select(x => x.Name)),
                SelectedPlaylist = user.Playlists.IsNullOrEmpty() ? null : user.Playlists[0].Name,

                QueueMethodDropdown = new SelectList(Enum.GetNames(typeof(SongQueueMethodEnum))),
                SelectedQueueMethod = SongQueueMethodEnum.Randomize.ToString()
            });
        }

        public IActionResult GetActiveSong()
        {
            // TODO Should be renamed to something akin to GetNewPageInfo...

            // This is how we refresh to get new song information onto the page
            // Because once the page is rendered for the user, we can't communicate with it - it has to communicate with us.
            // We can only try to call client->server with JavaScript, to then see if we should update the current song

            _room.NotifyUserSessionRequest(User.Identity.Name.ToString()); // To keep track of live connections

            var lastEvent = _room.LastSwitchedSongEvent;
            if (lastEvent is null) return Json(new { isActive = false });

            //sends the activeSong as Json, showing info about what song is currently playing and when it was started
            else return Json(new
            {
                isActive = true,
                songName = lastEvent.NewTrackRequest.Song.Name,
                author = lastEvent.NewTrackRequest.Song.Author,
                startTime = lastEvent.NewTrackRequest.Song.StartTime,
                elapsedTime = (DateTime.Now - lastEvent.SwitchedSongTime).TotalSeconds,
                endTime = lastEvent.NewTrackRequest.Song.EndTime,
                youtubeId = lastEvent.NewTrackRequest.Song.YoutubeId,
                currentDj = lastEvent.NewTrackRequest.DJ.Name,
                currentDjIsMe = lastEvent.NewTrackRequest.DJ.Name == User.Identity.Name.ToString(),
                switchedSongTimeTicks = lastEvent.SwitchedSongTime.Ticks,
                activeUsers = _room.ActiveUsers,
                activePlaylistNames = _room.GetConnectedPlaylists().pl.Select(x => x.Name)
            });
        }

        public IActionResult AddPlaylistToQueue(string selectedPlaylistName)
        {
            User user = _room.GetUser(User.Identity.Name);
            Playlist playlist = user.Playlists.Find(x => x.Name == selectedPlaylistName);
            if (playlist is null) throw new DataMisalignedException("A playlist of that name does not exist");
            _room.ConnectPlaylist(playlist, user);
            return Ok();
        }

        public IActionResult RemovePlaylistFromQueue(string selectedPlaylistName) 
        {
            User user = _room.GetUser(User.Identity.Name);
            _room.DisconnectPlaylist(user.Playlists.Find(x => x.Name == selectedPlaylistName), user);
            // TODO Currently you cant remove a playlist whose name you have altered after you added it to the queue, 
            // I should probably use some sort of other identifier? Is there something inherent?
            return Ok();
        }

        public IActionResult NextSong() 
        {
            _room.StartNextSong();
            return Ok();
        }

        public IActionResult PrevSong()
        {
            _room.StartPreviousSong();
            return Ok();
        }

        public IActionResult ApplyNewQueueMethod(string queueMethod)
        {
            var newQueueMethod = Enum.Parse<SongQueueMethodEnum>(queueMethod);
            _room.SwitchQueueMethod(newQueueMethod);
            return Ok();
        }

        public IActionResult DeletePlaylist(string playlistName)
        {
            User usr = _room.GetUser(User.Identity.Name);
            var playlists = usr.Playlists.FindAll(p => p.Name == playlistName);
            if (playlists.IsNullOrEmpty()) throw new DataMisalignedException("No playlist of that name exists");
            else if (playlists.Count != 1) throw new DataMisalignedException("Too many playlists share that name");

            usr.Playlists.Remove(playlists.First()); 
            _room.UpdateUser(usr); // TODO I SHOULD ABSTRACT AWAY THESE UPDATE FUNCTIONS, instead in room have a addplaylist and deleteplaylist methods
            return Ok();
        }

        public IActionResult CloseAddPlaylist()
        {
            return Ok();
        }

        public IActionResult ViewConnectedPlaylists()
        {
            var (usr, pl) = _room.GetConnectedPlaylists();
            return Json( new  { playlistAuthors = usr.Select(x => x.Name),
                                playlistNames = pl.Select(x => x.Name), 
                                playlistSongs = pl.Select(x => x.Tracks),
                                activeSongId = _room.LastSwitchedSongEvent?.NewTrackRequest?.Song?.YoutubeId }); 
        }

        public IActionResult ViewPlaylistProgress()
        {
            return Json( new { val = _room.GetProgress() });
        }

        public IActionResult StopPlayingAll()
        {
            _room.DisconnectAllPlaylists();
            _room.StopPlaying();
            return Ok();
        }

        public IActionResult CloseRoom()
        {
            return Ok();
            //TODO Call backendController, get it to close the current room? 
            // I could use this function as a sort of "reset". Or maybe I dont need it at all. 
        }


        [HttpGet]
        public ActionResult AddPlaylist()
        {
            // Automatically finds and returns the cshtml file corresponding to the function name "AddPlaylist"
            return PartialView("AddPlaylist", new Playlist() { Tracks = new List<Track>() { new Track() } } );
        }

        //[HttpGet]
        public ActionResult EditPlaylist(string selected)
        {
            if (String.IsNullOrEmpty(selected)) throw new NotImplementedException(); 
            return PartialView("AddPlaylist", _room.GetUser(User.Identity.Name).GetPlaylist(selected) );
        }

        [HttpPost]
        public ActionResult AddPlaylist(Playlist playlist)
        {
            if (!ModelState.IsValid) return PartialView("AddPlaylist", playlist);

            User usr = _room.GetUser(User.Identity.Name);
            var existing = usr.Playlists.Find(x => x.Name == playlist.Name);
            if (existing is null) usr.Playlists.Add(playlist);
            else usr.Playlists.ReplaceFirst(x => x == existing, playlist); // Replace existing playlist with the newly updated one

            _room.UpdateUser(usr); // TODO I SHOULD ABSTRACT AWAY THESE UPDATE FUNCTIONS, instead in room have a addplaylist and deleteplaylist methods
            return Ok(); 
        }

        public PartialViewResult Track()
        {
            return PartialView(new Track());
        }
    }
}