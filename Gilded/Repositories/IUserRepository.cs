using Gilded.Models;

namespace Gilded.Repositories
{
    public interface IUserRepository
    {
        /*
         * Creates a new user with this email address and a zero balance. Throws an exception on duplicate email addresses.
         * Returns the api key for the user.
         */
        string CreateUser(string emailAddress);

        /*
         * Gets a user based of their api key
         */
        User GetUser(string apiKey);

        /*
         * Updates a balance for a user, based on their primary key email address. Can also decrement.
         */
        void AddBalance(string apiKey, int amountChanged);
    }
}
