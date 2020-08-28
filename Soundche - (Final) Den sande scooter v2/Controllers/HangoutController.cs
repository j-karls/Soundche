using System;
using System.Collections.Generic;
using System.Linq;
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

        // TODO: Remove everywhere where it says EMILEN STABILEN

        public HangoutController(RoomManager room) // TODO: Should get something higher level (backend manager or smt), one that creates multiple rooms
        {
            _room = room;
            // Setting temp playlists
            _room.CallStuff(); //TODO REMOVE
        }

        public IActionResult Index()
        {
            /*var up = _room.GetUserInfo("Emilen Stabilen").Playlists;
            up.Add(new Playlist() { Name = "penis" });
            var ups = new SelectList(up.Select(x => x.Name));

            return View(new HangoutViewModel { UserPlaylists = ups, SelectedPlaylist = up[1].Name });*/

            var up = _room.GetUserInfo("Emilen Stabilen").Playlists;
            var ups = new SelectList(up.Select(x => x.Name));
            return View(new HangoutViewModel { UserPlaylists = ups, SelectedPlaylist = up[0].Name });
        }

        public IActionResult PostHangout()
        {
            var test = new HangoutViewModel();
            test.CurrentSong = "https://www.youtube.com/embed/rPkzkV1icWY";
            return View("index", test);
        }

        public IActionResult GetHangout(string name)
        {
            HttpContext.Response.StatusCode = 404;

            //var test = new HangoutViewModel(_room.GetUserInfo("Emilen Stabilen").Playlists);
            //test.CurrentSong = "https://www.youtube.com/embed/rPkzkV1icWY";
            return Ok("Hej");
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

        public IActionResult CallStuff()
        {
            _room.CallStuff();
            _room.GetStuff();
            return new EmptyResult();
        }

        [HttpPost]
        public IActionResult Play(HangoutViewModel vm)
        {
            // Add the user's playlist to the playback
            // Send a js query to the embeded video
            // And if nothing is currently playing, then start the playback
            // ?????????????????????

            if (vm.SelectedPlaylist is null) throw new DataMisalignedException("No playlist selected");
            Playlist playlist = _room.GetUserInfo("Emilen Stabilen").Playlists.Find(x => x.Name == vm.SelectedPlaylist);
            if (playlist is null) throw new DataMisalignedException("A playlist of that name does not exist");
            _room.ConnectPlaylist(playlist);
            ////////// TODO: Get user info through cookie or something here

            vm.PlaylistOnQueue = (true, vm.SelectedPlaylist);
            vm.UserPlaylists = new SelectList(_room.GetUserInfo("Emilen Stabilen").Playlists.Select(x => x.Name));
            return View("index", vm);
        }

        public IActionResult StopPlay(int playlistNr) //TODO DO THIS.
        {
            _room.DisconnectPlaylist(_room.GetUserInfo("Emilen Stabilen").Playlists[playlistNr]); 
            return View("index", new HangoutViewModel { PlaylistOnQueue = (false, "") });
        }
    }
}