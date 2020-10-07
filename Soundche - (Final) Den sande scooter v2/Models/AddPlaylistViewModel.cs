using Soundche.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Soundche.Web.Models
{
    public class AddPlaylistViewModel
    {
        public Playlist PlaylistModel { get; set; }
        public string NewTrackRequestJson { get; set; }
    }
}
