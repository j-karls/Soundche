using Core.Domain;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.BLL
{
    public class LiteDbController : IDatabaseController
    {
        // LiteDB Documentation at https://www.litedb.org/docs/getting-started/

        public string DbPath { get; private set; }
        private ILiteCollection<User> _users;

        public LiteDbController(string dbPath = "lite.db")
        {
            DbPath = dbPath;
            InitializeDb();
        }

        private void InitializeDb() 
        { 
            // Open database (or create if doesn't exist)
            using(var db = new LiteDatabase(DbPath))
            {
                // Get a collection (or create, if doesn't exist) 
                _users = db.GetCollection<User>("users");

                // TRY THIS SHIT with IDS's SUCH THAT I CAN GET A SINGLE ITEM LATER
                // users.Insert(new BsonValue(BsonType.Int32), hardcodeduser); 

                ////////////// TESTING BOIIII
                // User hardcodeduser = GetShitHardcodedUserBoi(); 
                // _users.Insert(hardcodeduser);
                // User emilen = QueryUsersTemp()[0];
                // Console.WriteLine(emilen);
            }
        }

        public void UpdateUser(User user) => _users.Update(user);

        public User GetUser(string username)
        {
            // Returns null if no fitting user is found
            return _users.FindOne(x => x.Name == username); 
        }

        public void AddUser(User user)
        {
            _users.Insert(user);
        }

        public void InsertShitHardcodedUserBoi()
        {
            _users.Insert(User.CreateUser("Emilen Stabilen", new List<Playlist> { new Playlist("cancerlisten", new List<Track> {
                new Track("♂ Leave the Gachimuchi on ♂", "https://www.youtube.com/watch?v=BH726JXRok0", 0, 300),
                new Track("♂️ AssClap ♂️ (Right version) REUPLOAD", "https://www.youtube.com/watch?v=NdqbI0_0GsM", 15, 100)
            })}));

            Console.WriteLine("Got it");
        }

        /*
        public User GetShitHardcodedUserBoi()
        {
            return User.CreateUser("Emilen Stabilen", new List<Playlist> { new Playlist("cancerlisten", new List<Track> { 
                new Track("♂ Leave the Gachimuchi on ♂", "https://www.youtube.com/watch?v=BH726JXRok0", 0, 300),
                new Track("♂️ AssClap ♂️ (Right version) REUPLOAD", "https://www.youtube.com/watch?v=NdqbI0_0GsM", 15, 100)
            })});
        }

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
