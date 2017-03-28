namespace Gilded.Models
{
    public class User
    {
        //will be unique identifier
        public string EmailAddress { get; set; }
        public string ApiKey { get; set; }
        public int Balance { get; set; }
        public string Role { get; set; }
    }
}