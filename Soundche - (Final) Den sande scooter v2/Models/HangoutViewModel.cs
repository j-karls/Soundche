using Microsoft.AspNetCore.Mvc.Rendering;
using Soundche.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Soundche.Web.Models
{
    public class HangoutViewModel
    {
        public string CurrentSong { get; set; } = "https://www.youtube.com/embed/CgxqXXgc_LM";
        public bool AutoPlay { get; set; } = true;
        public Track RealCurrentSong { get; set; } = new Track("♂ Leave the Gachimuchi on ♂", "XD", "https://www.youtube.com/watch?v=BH726JXRok0", 0, 5);
        //RealCurrentSong = new Track("♂️ AssClap ♂️ (Right version) REUPLOAD", "https://www.youtube.com/watch?v=NdqbI0_0GsM", 4, 11);

        public (bool, string) PlaylistOnQueue { get; set; } = (false, "");

        public HangoutViewModel()
        {
            //UserPlaylists = userPlaylist.Select(x => x.Name).ToList();

            //asdfghjkl = new SelectList(UserPlaylists, "PlaylistId", "PlaylistName", SelectedPlaylistId);
            //var userPlaylistDropDownListOptions = userPlaylist.Select(x => x.Name);
            //UserPlaylistDropDownListOptions = UserPlaylists.Select(x => new SelectListItem(x, x));
            //UserPlaylistDropDownListOptions[0].Selected = true;
            //SelectedPlaylistId = 1;
        }

        public string SelectedPlaylist { get; set; }
        public IEnumerable<SelectListItem> UserPlaylists { get; set; }
    }
}
