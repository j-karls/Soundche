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
        private SwitchedSongEventArgs _activeSong = new SwitchedSongEventArgs(new Track("♂️ AssClap ♂️ (Right version) REUPLOAD", "https://www.youtube.com/watch?v=NdqbI0_0GsM", 4, 11), DateTime.Now);
        // TODO: remove this hardcoded switchedsongeventargs, and have it all go through the room's playlist instead. 

        // TODO: Remove everywhere where it says EMILEN STABILEN

        public HangoutController(RoomManager room) // TODO: Should get something higher level (backend manager or smt), one that creates multiple rooms
        {
            _room = room;
            _room.SwitchedSongEvent += OnSwitchSong; // Switch song when the room does the same

            // Setting temp playlists
            _room.CallStuff();
            _room.ConnectPlaylist(_room.GetUserInfo("Emilen Stabilen").Playlists[0]);
        }

        public IActionResult Index()
        {
            var up = _room.GetUserInfo("Emilen Stabilen").Playlists;
            up.Add(new Playlist() { Name = "penis" });
            var ups = up.Select(x => x.Name);
            return View(new HangoutViewModel() { UserPlaylists = new SelectList(ups) });
            //return View(new HangoutViewModel() { UserPlaylists = new SelectList(_room.GetUserInfo("Emilen Stabilen").Playlists.Select(x => x.Name)) });
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

            var test = new HangoutViewModel();
            test.CurrentSong = "https://www.youtube.com/embed/rPkzkV1icWY";
            return Ok("Hej");
        }

        public IActionResult GetActiveSong()
        {
            // TODO: This function should only be called if we've called "play" before. 


            // This is how we refresh to get new song information onto the page
            // Because once the page is rendered for the user, we can't communicate with it - it has to communicate with us.
            // We can only try to call server with JavaScript, see when to then see if we should update

            //sends the activeSong as Json, showing info about what song is currently playing and when it was started
            return Json(new
            {
                name = _activeSong.NewTrack.Name,
                startTime = _activeSong.NewTrack.StartTime,
                endTime = _activeSong.NewTrack.EndTime,
                youtubeUrl = _activeSong.NewTrack.YoutubeUrl,
                switchedSongTime = _activeSong.SwitchedSongTimeTicks
            }); 
        }



        public IActionResult CallStuff()
        {
            _room.CallStuff();
            _room.GetStuff();
            return new EmptyResult();
        }

        public IActionResult Play(HangoutViewModel vm)
        {
            // Add the user's playlist to the playback
            // Send a js query to the embeded video
            // And if nothing is currently playing, then start the playback
            // ?????????????????????

            //return Ok("Started Playback");
            _room.ConnectPlaylist(_room.GetUserInfo("Emilen Stabilen").Playlists[Int32.Parse(vm.SelectedPlaylist)]); ////////// TODO: Get user info through cookie or something here
            
            return View("index", new HangoutViewModel { PlaylistOnQueue = (true, Int32.Parse(vm.SelectedPlaylist)) });
            //return View("hangout", new HangoutViewModel { PlaylistOnQueue = (true, playlistNr) });
        }

        public IActionResult StopPlay(int playlistNr)
        {
            _room.DisconnectPlaylist(_room.GetUserInfo("Emilen Stabilen").Playlists[playlistNr]); ////////// TODO: Get user info through cookie or something here
            return View("index", new HangoutViewModel { PlaylistOnQueue = (false, 0) });
        }

        private void OnSwitchSong(object sender, SwitchedSongEventArgs e)
        {
            _activeSong = e;
        }

    }
}