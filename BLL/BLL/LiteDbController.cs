using Core.Domain;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.BLL
{
    public class LiteDbController : IDatabaseController
    {
        public string DbPath { get; set; }

        private ILiteCollection<User> users;

        public LiteDbController(string dbPath = "lite.db")
        {
            DbPath = dbPath;
            InitializeDb();
        }

        public void InitializeDb() 
        { 
            // Open database (or create if doesn't exist)
            using(var db = new LiteDatabase(DbPath))
            {
                // Get a collection (or create, if doesn't exist) 
                users = db.GetCollection<User>("users");




                User hardcodeduser = GetShitHardcodedUserBoi(); //////////////TODO BOIIII




                // users.Insert(new BsonValue(BsonType.Int32), hardcodeduser); TRY THIS SHIT with IDS's SUCH THAT I CAN GET A SINGLE ITEM LATER

                // Insert new customer document (Id will be auto-incremented)
                users.Insert(hardcodeduser);


                /*
                // Let's create an index in phone numbers (using expression). It's a multikey index
                col.EnsureIndex(x => x.Phones); 

                // and now we can query phones
                var r = col.FindOne(x => x.Phones.Contains("8888-5555"));
                 */

                User emilen = QueryUsersTemp()[0];
                Console.WriteLine(emilen);
            }
        }

        private User GetShitHardcodedUserBoi()
        {
            return User.CreateUser("Emilen Stabilen", new List<Playlist> { Playlist.CreatePlaylist("cancerlisten", new List<Track> { 
                new Track("♂ Leave the Gachimuchi on ♂", "https://www.youtube.com/watch?v=BH726JXRok0", 0, 300),
                new Track("♂️ AssClap ♂️ (Right version) REUPLOAD", "https://www.youtube.com/watch?v=NdqbI0_0GsM", 15, 100)
            })});
        }

        public void UpdateUser(User user)
        {
            users.Update(user);
        }

        public List<User> QueryUsersTemp()
        {
            // Use LINQ to query documents (filter, sort, transform)
            return users.Query()
                .Where(x => x.Name.StartsWith("Emilen"))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public User GetSpecificUser(string username)
        {
            //users.FindById();
            throw new NotImplementedException();
        }


    }
}
