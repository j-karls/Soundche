using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain
{
    public class Track
    {
        public string Name { get; set; }
        // Todo required
        public string YoutubeId { get; set; }
        public string YoutubeUrl => "youtube.com/" + YoutubeId;
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public bool Exclude { get; set; }

        public Track() { }
        public Track(string title, string author, string youtubeId, int startTime, int endTime)
        {
            Name = title;
            YoutubeId = youtubeId; // regex filtering is done client-side
            StartTime = startTime; 
            EndTime = endTime; 
        }
    }
}
