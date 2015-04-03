using System;

namespace VacationMasters.Essentials
{
    public class User
    {
        public enum AuthTypes
        {
            User,
            Administrator
        };

        public string PhoneNumber { get; set; }
        public string KeyWordSearches { get; set; }
        public string UserName { get; private set; }
        public string Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Banned { get; set; }
        public string Email { get; set; }
        public AuthTypes AuthType { get; set; }

        public User(string userName, string firstName, string lastName, string email, string phoneNumber,
            bool banned, string type, string keyWordSearches)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Banned = banned;
            Type = type;
            KeyWordSearches = keyWordSearches;
        }

    }
}
