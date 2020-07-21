using Core.Domain;
using MongoDB.Driver;
using System;
using System.Timers;

public class SQLiteDbController : IDatabaseController
{
    public Timer PlaybackTimer = null;
    public Track CurrentTrack = null;

    // TODO: decouple database and playlistcontroller

    SQLiteDbController()
    {
        InitializeDb(); 
    }

    private void InitializeDb()
    {
        //var client = new MongoClient();
        // make new documentbased db if none exists, establish connection
        throw new NotImplementedException();
    }

    public event EventHandler<SwitchedSongEventArgs> SwitchedSongEvent;

    public void StartPlayback(Playlist playlist)
    {
        if (CurrentTrack is null) // If no song is running - NOTE, this assumes that there is actually a playlist
        {
            StartNewSong();
        }
    }

    private void StartNewSong()
    {
        Track track = GetNewTrack();
        CurrentTrack = track;
        StartTimer(track.EndTime - track.StartTime);
        SwitchedSongEvent(this, new SwitchedSongEventArgs(track.YoutubeUrl, track.StartTime));
    }

    private Track GetNewTrack() // Get new track from a playlist
    {
        // Put this into some "collected playlists manager" class
        return new Track("Yir Boi", "urlboi", 0, 10);
        // return new NotImplementedException();
    }

    private void StartTimer(double interval)
    {
        PlaybackTimer = new Timer(interval);
        PlaybackTimer.Elapsed += OnTimerElapsed; //try using short lambda syntax here
        PlaybackTimer.AutoReset = false;
        PlaybackTimer.Enabled = true;
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e) => StartNewSong();

    public Playlist CreateNewPlaylist()
    {
        throw new NotImplementedException();
    }

    public Playlist AddToPlaylist(Playlist playlist)
    {
        throw new NotImplementedException();
    }

    public Playlist GetPlaylist(string name)
    {
        throw new NotImplementedException();
    }
}
