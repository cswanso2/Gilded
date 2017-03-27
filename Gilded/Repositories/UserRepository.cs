using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gilded.Models;

namespace Gilded.Repositories
{
    public class UserRepository : IUserRepository
    {
        public void AddBalance(string emailAddress, int amountChanged)
        {
            throw new NotImplementedException();
        }

        public string CreateUser(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string apiKey)
        {
            throw new NotImplementedException();
        }
    }
}