using Core.Domain;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

public class SQLiteDbController : IDatabaseController
{
    public string DbPath { get; set; }

    SQLiteDbController(string dbPath = "sqlite.db")
    {
        DbPath = dbPath;
    }

    public void InitializeDb()
    {
        // https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli
        // make new documentbased db if none exists, establish connection


        /*
         using (var connection = new SqliteConnection("Data Source=hello.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT name
                FROM user
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", id);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var name = reader.GetString(0);

                    Console.WriteLine($"Hello, {name}!");
                }
            }
        }
        */

        throw new NotImplementedException();
    }

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

    public List<string> GetUserInfo(string username)
    {
        // gets playlist names and other misc publically available stuff
        throw new NotImplementedException();
    }
}
