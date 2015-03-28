using System;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using VacationMasters.Essentials;
using VacationMasters.Wrappers;

namespace VacationMasters.UserManagement
{
    public class UserManagement : IUserManagement
    {
        private readonly IDbWrapper _dbWrapper;

        public UserManagement(IDbWrapper dbWrapper)
        {
            _dbWrapper = dbWrapper;
        }

        public bool CanLogin(User user)
        {
            //check for: existing user
            return !user.Banned;
        }

        public void AddUser(User user, string password)
        {
            var input = CryptographicBuffer.ConvertStringToBinary(password,
           BinaryStringEncoding.Utf8);
            var hasher = HashAlgorithmProvider.OpenAlgorithm("SHA256");
            var hashed = hasher.HashData(input);
            var pwd = CryptographicBuffer.EncodeToBase64String(hashed);
            string type = "User";
            var sql = string.Format("INSERT INTO Users(UserName,FirstName,LastName,Email,PhoneNumber," +
                                    "Password,Banned,Type,KeyWordsSearches) " +
                                    "values({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8});", 
                user.UserName, user.FirstName, user.LastName, user.Email, user.PhoneNumber, pwd,
                false, type, user.KeyWordSearches);
            _dbWrapper.QueryValue<object>(sql);
        }

        public void RemoveUser(string userName)
        {

        }
    }
}
