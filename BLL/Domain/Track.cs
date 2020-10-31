using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Soundche.Core.Domain
{
    public class Track
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Author { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string YoutubeId { get; set; }
        [Required]
        public int StartTime { get; set; }
        [Required]
        public int EndTime { get; set; }
        public bool Exclude { get; set; }

        public Track() { }
        public Track(string title, string author, string youtubeId, int startTime, int endTime)
        {
            Name = title;
            Author = author;
            YoutubeId = youtubeId; // regex filtering is done client-side
            StartTime = startTime; 
            EndTime = endTime; 
        }
    }
}
