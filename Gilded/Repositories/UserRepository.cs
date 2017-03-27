﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gilded.Models;
using System.Net.Mail;
using Gilded.Exceptions;

namespace Gilded.Repositories
{
    /*
     * Not dealing with concurency issues in here, assuming they would be handled by database.
     */
    public class UserRepository : IUserRepository
    {
        private const string UserRole = "user";
        private static Dictionary<string, User> _apiKeyDictionary = new Dictionary<string, User>();

        private static Dictionary<string, User> _emailDictionaryUsers = new Dictionary<string, User>();
        static UserRepository()
        {
            //a bit hacky.
            var adminUser = new User
            {
                ApiKey = "adminapikey",
                EmailAddress = "cswanso21@gmail.com",
                Balance = 0,
                Role = "admin"
            };
            _apiKeyDictionary.Add(adminUser.ApiKey, adminUser);
            _emailDictionaryUsers.Add(adminUser.EmailAddress, adminUser);
        }

        public void AddBalance(string apiKey, int amountChanged)
        {
            var user = _emailDictionaryUsers[apiKey];
            user.Balance += amountChanged;
        }

        public string CreateUser(string emailAddress)
        {
               MailAddress m = new MailAddress(emailAddress); //validate email
            if (_emailDictionaryUsers.ContainsKey(emailAddress))
                throw new DuplicateUserException();
            var apikey = Guid.NewGuid().ToString();
            var user = new User()
            {
                ApiKey = apikey,
                Balance = 0,
                EmailAddress = emailAddress,
                Role = UserRole
            };
            _apiKeyDictionary[apikey] = user;
            _emailDictionaryUsers[emailAddress] = user;
            return apikey;
        }

        public User GetUser(string apiKey)
        {
            return _apiKeyDictionary[apiKey];
        }
    }
}