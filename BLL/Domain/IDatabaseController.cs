using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public interface IDatabaseController
    {
        // Needs to be initialized as a singleton, such that all sessions use the same connection to the database

        public User GetUser(string username);
        public void UpdateUser(User user); 
        public void AddUser(User user);
        public void InsertShitHardcodedUserBoi();
    }
}
