using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public static class QueueProgressHelper
    {
        public static string CreateStringMatrixFormat(int titleLen, int itemLen, int itemCount) {
            return "{0,-" + titleLen + "} " + String.Join("", Enumerable.Range(1, itemCount).Select(x => $"{{{ x },-{ itemLen }}} ")) + "\n";
        }

        public static string AddLineToMatrix(string matrix, string format, string title, IEnumerable<string> itemValues) {
            var ls = new List<string> { title }; 
            ls.AddRange(itemValues);
            matrix += String.Format(format, ls.ToArray());
            return matrix;
        }

        public static List<string> RoundRobinGetPlaylistAsString(Playlist pl, int trackIdx, bool isCurrentSong, int itemMaxLen)
        {
            // Note, theres a potential fatal error here. If you change the queue method without pressing nextTrack, then currentSong will be true
            // and trackIdx will become negative, causing a failure. Not sure how to fix this one. Because the "old" song is only playing on the 
            // frontend and the backend thinks that we've switched already. So the current workaround is for the frontend to call nextSong 
            // automatically if we switch queueMethod. I guess that's alright. 

            List<string> stringList = pl.Tracks.Select(z => "  " + z.ToReadableString().Truncate(itemMaxLen - 2)).ToList();
            trackIdx = isCurrentSong ? ((trackIdx - 1) < 0 ? pl.Tracks.Count - 1 : trackIdx - 1) : trackIdx; 

            // If it's the currently playing song, the index will have already changed in the eyes of the queueMethod, even though the song is still playing
            // The inner ternary operator is used because we know that if trackIdx looped back around to zero, we are currently playing the last track of the playlist 

            stringList[trackIdx] = (isCurrentSong ? ">>" : "> ") + stringList[trackIdx][2..];
            // There's two kinds of "currentSongs". The one that the individual playlist thinks is the current song, and then the one song that is actually playing out of all playlist's current songs. 
            return stringList;
        }
        public static string AddManyLinesToMatrix(string matrix, string format, string title, List<List<string>> itemValues) {
            matrix = AddLineToMatrix(matrix, format, title, Enumerable.Range(0, itemValues.Count).Select(x => "------"));
            
            for (int i = 0; i < itemValues.Max(x => x.Count); i++)
            {
                var s = new List<string>(itemValues.Count) { " " };
                foreach (var itemValue in itemValues)
                    s.Add(itemValue.ElementAtOrDefault(i));
                matrix += String.Format(format, s.ToArray());
            }
            return matrix;
        }

        public static int GetPlaylistPlayedTime(Playlist playlist, int trackIdx, int currentSongProgress) => 
            playlist.Tracks.GetRange(0, trackIdx + 1).Select(x => x.Exclude ? 0 : x.Playtime).Aggregate((x, y) => x + y) + currentSongProgress;
    }
}
