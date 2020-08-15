using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public enum SongQueueMethodEnum // How do we select the next song?
    {
        Randomize,          // Put all songs from all playlists into a pot, randomly select one 
        RoundRobin,         // Like good ol' plugdj - allow each playlist to play one song, then onto the next one, round and round
        WeightedRoundRobin  // We go around like roundrobin, but each playlist is selected according to how many songs it contains
                            // If one playlist has twice the songs of another, we will skip the short one in half the rounds
    }
}
