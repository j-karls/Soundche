using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public static class QueueProgressHelper
    {
        public static string CreateStringMatrixFormat(int titleLen, int itemLen, int itemCount) {
            return "{0,-" + titleLen + "} " + String.Join("", Enumerable.Range(1, itemCount).Select(x => $"{{{ itemLen },-25}} ")) + "\n";
        }

        public static string AddLineToMatrix(string matrix, string format, string title, IEnumerable<string> itemValues) {
            var ls = new List<string> { title }; 
            ls.AddRange(itemValues);
            matrix += String.Format(format, ls.ToArray());
        }

        public static List<string> GetPlaylistAsString(Playlist pl, int plIdx, bool isCurrentSong)
        {
            List<string> stringList = pl.Tracks.Select(z => "  " + z.ToReadableString().Truncate(23)).ToList();
            stringList[plIdx] = (isCurrentSong ? ">>" : "> ") + stringList[plIdx][2..];
            return stringList;
        }
        public static string AddManyLinesToMatrix(string matrix, string format, string title, List<List<string>> itemValues) {
            matrix = AddLineToMatrix(matrix, format, title, Enumerable.Range(0, itemValues.Count).Select(x => "------"));
            
            for (int i = 0; i < itemValues.Max(x => x.Count); i++)
            {
                var s = new List<string>(_tuple.Count) { " " };
                foreach (var itemValue in itemValues)
                    s.Add(itemValue.ElementAtOrDefault(i));
                matrix += String.Format(format, s.ToArray());
            }
            return matrix;
        }

        public static string CreateStringMatrix(int items) {
            
        }
    }
}
