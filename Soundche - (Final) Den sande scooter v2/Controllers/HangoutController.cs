using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Soundche.Core.BLL;
using Soundche.Core.Domain;
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
                _room.AddUser(new Core.Domain.User(User.Identity.Name)); 
                user = _room.GetUser(User.Identity.Name);
            }
            var ups = new SelectList(user.Playlists.Select(x => x.Name));
            return View(new HangoutViewModel { UserPlaylists = ups, SelectedPlaylist = user.Playlists.IsNullOrEmpty() ? null : user.Playlists[0].Name });
        }

        public IActionResult GetActiveSong()
        {
            // This is how we refresh to get new song information onto the page
            // Because once the page is rendered for the user, we can't communicate with it - it has to communicate with us.
            // We can only try to call client->server with JavaScript, to then see if we should update the current song

            var lastEvent = _room._lastSwitchedSongEvent;
            if (lastEvent is null) return Json(new { active = false });

            //sends the activeSong as Json, showing info about what song is currently playing and when it was started
            else return Json(new
            {
                active = true,
                name = lastEvent.NewTrack.Name,
                startTime = lastEvent.NewTrack.StartTime,
                endTime = lastEvent.NewTrack.EndTime,
                youtubeUrl = lastEvent.NewTrack.YoutubeUrl,
                switchedSongTime = lastEvent.SwitchedSongTimeTicks
            }); 
        }

        public IActionResult Play(HangoutViewModel vm)
        {
            // Add the user's playlist to the playback
            // Send a js query to the embeded video
            // And if nothing is currently playing, then start the playback
            // ?????????????????????

            if (vm.SelectedPlaylist is null) throw new DataMisalignedException("No playlist selected");
            Playlist playlist = _room.GetUser(User.Identity.Name).Playlists.Find(x => x.Name == vm.SelectedPlaylist);
            if (playlist is null) throw new DataMisalignedException("A playlist of that name does not exist");
            _room.ConnectPlaylist(playlist);

            vm.PlaylistOnQueue = (true, vm.SelectedPlaylist);
            vm.UserPlaylists = new SelectList(_room.GetUser(User.Identity.Name).Playlists.Select(x => x.Name));
            return View("index", vm);

            // TODO: Remove this hangouts controller, probably put it in a partialview or something. No need to refresh or change the URL just because we joined the waitlist
        }

        public IActionResult StopPlay(int playlistNr) //TODO DO THIS.
        {
            _room.DisconnectPlaylist(_room.GetUser(User.Identity.Name).Playlists[playlistNr]); 
            return View("index", new HangoutViewModel { PlaylistOnQueue = (false, "") });
        }

        [HttpGet]
        public ActionResult AddPlaylist()
        {
            // Automatically finds and returns the cshtml file corresponding to the function name "AddPlaylist"
            return View(new Playlist() { Tracks = new List<Track>() { /*new Track(), new Track()*/ } }); 
        }

        [HttpPost]
        public ActionResult AddPlaylist(Playlist playlist)
        {
            // do validation
            if (!ModelState.IsValid) return View(playlist);

            // save playlist
            User usr = _room.GetUser(User.Identity.Name);
            usr.Playlists.Add(playlist);
            _room.UpdateUser(usr);

            // redirect
            return Redirect("index");
        }
    }
}