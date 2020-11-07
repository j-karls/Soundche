using Soundche.Core.Domain;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.BLL
{
    public class LiteDbManager : IDatabaseManager
    {
        // LiteDB Documentation at https://www.litedb.org/docs/getting-started/

        public string DbPath { get; private set; }
        private LiteDatabase _db;
        private ILiteCollection<User> _users;
        // private ILiteCollection<Room> _rooms;

        public LiteDbManager(string dbPath = "lite.db")
        {
            DbPath = dbPath;
            InitializeDb();
        }

        private void InitializeDb()
        {
            // Open database (or create if doesn't exist)
            _db = new LiteDatabase(DbPath);
            // Get a collection (or create, if doesn't exist) 
            _users = _db.GetCollection<User>("users");
        }

        // public void UpdateUser(User user) => _users.Update(user);

        public void UpdateUser(User user) => _users.Update(user);

        public User GetUser(string username)
        {
            // Returns null if no fitting user is found
            return _users.FindOne(x => x.Name == username); 
        }

        public void AddUser(User user) => _users.Insert(user);

        public void Dispose() => _db.Dispose();

        /*
        public List<User> QueryUsersTemp()
        {
            // Use LINQ to query documents with more complex search queries
            return _users.Query()
                .Where(x => x.Name.StartsWith("Emilen"))
                .OrderBy(x => x.Name)
                .ToList();
        }
        */
    }
}
