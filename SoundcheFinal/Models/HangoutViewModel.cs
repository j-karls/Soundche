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
        // Contains all relevant user and room settings necessary to render this particular hangout room

        // User settings
        public bool AutoPlay { get; set; }
        public List<Playlist> Playlists { get; set; }
        public IEnumerable<SelectListItem> PlaylistsDropdown { get; set; }
        public string SelectedPlaylist { get; set; }

        // Room configurations
        public IEnumerable<SelectListItem> QueueMethodDropdown { get; set; }
        public string SelectedQueueMethod { get; set; }
        // TODO More, probably something like room codes

        public HangoutViewModel() { }
    }
}
