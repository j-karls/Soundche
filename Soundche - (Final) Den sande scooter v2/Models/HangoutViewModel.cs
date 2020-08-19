using Soundche.Core.Domain;
using System;
using System.Timers;

namespace Soundche.Web.Models
{
    public class HangoutViewModel
    {
        public string CurrentSong { get; set; } = "https://www.youtube.com/embed/CgxqXXgc_LM";
        public bool AutoPlay { get; set; } = true;
        public Track RealCurrentSong { get; set; } = new Track("♂ Leave the Gachimuchi on ♂", "https://www.youtube.com/watch?v=BH726JXRok0", 0, 5);
        //RealCurrentSong = new Track("♂️ AssClap ♂️ (Right version) REUPLOAD", "https://www.youtube.com/watch?v=NdqbI0_0GsM", 4, 11);
    }
}
