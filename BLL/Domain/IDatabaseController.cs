using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public interface IDatabaseController
    {
        // Needs to be initialized as a singleton, such that all sessions use the same connection to the database

        void InitializeDb();
        public User GetUser(string username);
        public void UpdateUser(User user); 
    }
}
