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
using Soundche.Core.Domain.SongQueueMethod;
using Soundche.Web.Models;

namespace Soundche.Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HangoutController : Controller
    {
        // todo
        //figure out how to get dropddownvalues inside JS, for when I do my calls	
        //make a cronjob for the server to backup our db every day
        // fix grid inside track

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
            // This is how we refresh to get new song information onto the page
            // Because once the page is rendered for the user, we can't communicate with it - it has to communicate with us.
            // We can only try to call client->server with JavaScript, to then see if we should update the current song

            var lastEvent = _room._lastSwitchedSongEvent;
            if (lastEvent is null) return Json(new { active = false });

            //sends the activeSong as Json, showing info about what song is currently playing and when it was started
            else return Json(new
            {
                isActive = true,
                songName = lastEvent.NewTrack.Name,
                author = lastEvent.NewTrack.Author,
                startTime = lastEvent.NewTrack.StartTime,
                endTime = lastEvent.NewTrack.EndTime,
                youtubeId = lastEvent.NewTrack.YoutubeId,
                switchedSongTime = lastEvent.SwitchedSongTimeTicks
            });
        }

        public IActionResult Play(string selectedPlaylistName)
        {
            Playlist playlist = _room.GetUser(User.Identity.Name).Playlists.Find(x => x.Name == selectedPlaylistName);
            if (playlist is null) throw new DataMisalignedException("A playlist of that name does not exist");
            _room.ConnectPlaylist(playlist);

            return Ok();
        }

        public IActionResult StopPlay(string selectedPlaylistName) 
        {
            _room.DisconnectPlaylist(_room.GetUser(User.Identity.Name).Playlists.Find(x => x.Name == selectedPlaylistName));
            // TODO Currently you cant remove a playlist whose name you have altered after you added it to the queue
            return Ok();
        }

        public IActionResult NextSong() 
        {
            _room.SkipSong();
            return Ok();
        }

        public IActionResult PrevSong()
        {
            throw new NotImplementedException();
            //_room.PrevSong();
            //return Ok();
        }

        public IActionResult ApplyNewQueueMethod()
        {
            throw new NotImplementedException();
            //return Ok();
        }

        public IActionResult DeletePlaylist()
        {
            throw new NotImplementedException();
            //return Ok();
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
        public ActionResult EditPlaylist(Playlist playlist) => AddPlaylist(playlist);

        [HttpPost]
        public ActionResult AddPlaylist(Playlist playlist)
        {
            // do validation
            if (!ModelState.IsValid) return PartialView("AddPlaylist", playlist);

            // save playlist
            User usr = _room.GetUser(User.Identity.Name);
            usr.Playlists.Add(playlist); // TODO Make a check and edit playlist if it's not a new one
            _room.UpdateUser(usr);


            return new EmptyResult(); 
        }

        public PartialViewResult Track()
        {
            return PartialView(new Track());
        }
    }
}