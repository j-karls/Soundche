using Core.Domain;
using Microsoft.Data.Sqlite;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

public class SQLiteDbController : IDatabaseController
{
    public string DbPath { get; set; }

    public SQLiteDbController(string dbPath = "sqlite.db")
    {
        DbPath = dbPath;
        if (!System.IO.File.Exists(DbPath)) InitializeDb();
    }

    public void InitializeDb()
    {
        /*using (var connection = new SqliteConnection($"Data Source={DbPath}"))
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
        }*/
    }

    //public void CreateTable(string )
    //https://stackoverflow.com/questions/15292880/create-sqlite-database-and-table

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
