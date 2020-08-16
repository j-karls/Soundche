using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain
{
    public interface IDatabaseManager : IDisposable
    {
        // Needs to be initialized as a singleton, such that all sessions use the same connection to the database

        public User GetUser(string username);
        public void UpdateUser(User user); 
        public void AddUser(User user);
        public void InsertShitHardcodedUserBoi();
        public User GetShit();
    }
}
