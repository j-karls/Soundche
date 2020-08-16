using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            // Setting temp playlists
            _room.CallStuff();
            _room.ConnectPlaylist(_room.GetUserInfo("Emilen Stabilen").Playlists[0]);
        }

        public IActionResult Index()
        {
            return View(new HangoutViewModel());
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

        public IActionResult CallStuff()
        {
            _room.CallStuff();
            _room.GetStuff();
            return new EmptyResult();
        }

        public IActionResult Play(/*Track track = null*/)
        {
            // track = track ?? new Track("Yirboi", "https://www.youtube.com/embed/rPkzkV1icWY", 1, 5);

            // Add the user's playlist to the playback
            // Send a js query to the embeded video
            // And if nothing is currently playing, then start the playback

            _room.SwitchedSongEvent += OnSwitchSong;
            _room.StartPlayback();
            return Ok("Started Playback");
        }

        private void OnSwitchSong(object sender, SwitchedSongEventArgs e)
        {
            // TODO It seems to switch songs all the time, somehow the timer doesnt work properly
            // Try using an element on screen that I bind the URL property to.

            Console.WriteLine(e);
        }

    }
}
