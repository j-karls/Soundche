using Microsoft.AspNetCore.Mvc.Rendering;
using Soundche.Core.Domain;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Soundche.Web.Models
{
    public class HangoutViewModel
    {
        public string CurrentSong { get; set; } = "https://www.youtube.com/embed/CgxqXXgc_LM";
        public bool AutoPlay { get; set; } = true;
        public Track RealCurrentSong { get; set; } = new Track("♂ Leave the Gachimuchi on ♂", "https://www.youtube.com/watch?v=BH726JXRok0", 0, 5);
        //RealCurrentSong = new Track("♂️ AssClap ♂️ (Right version) REUPLOAD", "https://www.youtube.com/watch?v=NdqbI0_0GsM", 4, 11);

        public (bool, int) PlaylistOnQueue { get; set; } = (false, 0);

        public SelectList UserPlaylists { get; set; }
        public string SelectedPlaylist { get; set; }

        // TODO: Make the gender example from "https://stackoverflow.com/questions/27901175/how-to-get-dropdownlist-selectedvalue-in-controller-in-mvc"
        // Use Playlist type, and have a getPlaylistSelectItems() function

        public static IEnumerable<SelectListItem> GetPlaylistSelectItems()
        {
            var lst = new List<Playlist>();
            foreach (Playlist playlist in lst)
                yield return new SelectListItem { Text = playlist.Name };
        } //TODO USE THIS
    }
}
