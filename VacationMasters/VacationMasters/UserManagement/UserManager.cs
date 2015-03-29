using System;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using VacationMasters.Essentials;
using VacationMasters.Wrappers;
using System.Collections.Generic;

namespace VacationMasters.UserManagement
{
    public class UserManager : IUserManager
    {
        private readonly IDbWrapper _dbWrapper;

        public UserManager(IDbWrapper dbWrapper)
        {
            _dbWrapper = dbWrapper;
        }

        public bool CanLogin(User user)
        {
            //TODO: check for existing user
            return !user.Banned;
        }

        public bool CheckCredentials(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(string userName, string newPassword)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string userName)
        {
            throw new NotImplementedException();
        }

        public void AddUser(User user, string password, List<int> preferencesIds, string type = "User")
        {
            var input = CryptographicBuffer.ConvertStringToBinary(password,
           BinaryStringEncoding.Utf8);
            var hasher = HashAlgorithmProvider.OpenAlgorithm("SHA256");
            var hashed = hasher.HashData(input);
            var pwd = CryptographicBuffer.EncodeToBase64String(hashed);
            var sql = string.Format("INSERT INTO Users(UserName, FirstName, LastName, Email, PhoneNumber," +
                                    "Password, Banned, Type, KeyWordsSearches) " +
                                    "values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, '{7}', '{8}');",
                user.UserName, user.FirstName, user.LastName, user.Email, user.PhoneNumber, pwd,
                false, type, user.KeyWordSearches);
            sql += "SELECT LAST_INSERT_ID();";
            var idUser = _dbWrapper.QueryValue<int>(sql);
            foreach (var id in preferencesIds)
            {
                sql = string.Format("INSERT INTO ChoosePreferences(IDUser,IDPreference) values('{0}','{1}');", idUser, id);
            }
            _dbWrapper.QueryValue<object>(sql);
        }

        public void AddPreference(Preference preference)
        {
            var sql = string.Format("INSERT INTO Preferences(Name,Category) values ('{0}','{1}');", preference.Name, preference.Category);
            _dbWrapper.QueryValue<object>(sql);
        }

        public void RemoveUser(string userName)
        {
            var sql = string.Format("Delete from Users where UserName = {0}", userName);
            _dbWrapper.QueryValue<object>(sql);
        }
    }
}
