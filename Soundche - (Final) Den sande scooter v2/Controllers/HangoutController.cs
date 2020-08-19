using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private SwitchedSongEventArgs _activeSong = new SwitchedSongEventArgs(new Track("♂️ AssClap ♂️ (Right version) REUPLOAD", "https://www.youtube.com/watch?v=NdqbI0_0GsM", 4, 11), DateTime.Now);

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

        public IActionResult Test()
        {
            var testvm = new HangoutViewModel();
            //var tim = new System.Timers.Timer(5000);
            //tim.AutoReset = false;
            //tim.Start();
            ViewBag.Name = "Penis";
            return View("index", testvm); // try something with partial view here?
        }

        public IActionResult GetActiveSong()
        {
            //sends the activeSong as Json, showing info about what song is currently playing and when it was started
            return Json(new
            {
                name = _activeSong.NewTrack.Name,
                startTime = _activeSong.NewTrack.StartTime,
                endTime = _activeSong.NewTrack.EndTime,
                youtubeUrl = _activeSong.NewTrack.YoutubeUrl,
                switchedSongTime = _activeSong.SwitchedSongTime
            }); 
        }


        //[HttpPost]
        public IActionResult RefreshPage()
        {
            // This is how we refresh to get new song information onto the page? 
            // Because once the page is rendered for the user, we can't communicate with it - it has to communicate with us.

            // We can only try to call server with JavaScript, see when we should update??????
            

            //if (_room.CurrentTrack == ViewBag.CurrentTrack) return PartialView("_AudioPlayer", new HangoutViewModel());
            //else return View("index", new HangoutViewModel() { CurrentSong = _room.CurrentTrack.YoutubeUrl });
            

            if (_room.CurrentTrack != ViewBag.CurrentTrack) return PartialView("_AudioPlayer", new HangoutViewModel());
            else return PartialView("_AudioPlayer", new HangoutViewModel() { RealCurrentSong = new Track("♂️ AssClap ♂️ (Right version) REUPLOAD", "https://www.youtube.com/watch?v=NdqbI0_0GsM", 4, 11) }); // CurrentSong = _room.CurrentTrack.YoutubeUrl

            
            /// TRY TO MAKE JAVASCRIPT CALL THIS EVERY SECOND
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
            _activeSong = e;
        }

    }
}
