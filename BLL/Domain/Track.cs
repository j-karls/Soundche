﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain
{
    public class Track
    {
        public string Name { get; set; }
        public string YoutubeUrl { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }

        public Track() { }
        public Track(string name, string youtubeUrl, int startTime, int endTime)
        {
            Name = name;
            YoutubeUrl = youtubeUrl; // TODO: We should have some regex filtering or shortening of URL here
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
