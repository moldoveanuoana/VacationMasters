using System;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using VacationMasters.Essentials;
using VacationMasters.Wrappers;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace VacationMasters.UserManagement
{
    public class UserManager : IUserManager
    {
        private readonly IDbWrapper _dbWrapper;

        public UserManager(IDbWrapper dbWrapper)
        {
            _dbWrapper = dbWrapper;
        }

        public bool CheckCredentials(string userName, string password)
        {
            if (!CheckIfUserExists(userName))
                return false;

            var sql = string.Format("SELECT password FROM Users UserName = {0}", userName);
            var usrPwd = _dbWrapper.QueryValue<object>(sql);
            string pwd = EncryptPassword(userName, password);

            if (String.Compare(usrPwd.ToString(), pwd) != 0)
                return false;

            return true;
        }

        public void ChangePassword(string userName, string newPassword)
        {
            string newPwd = EncryptPassword(userName, newPassword);
            var sql = string.Format("Update Users Set password ='{0}'", newPwd);
            _dbWrapper.QueryValue<object>(sql);
        }
        public string GetMail(string userName)
        {

            var sql = string.Format("SELECT Email FROM Users where UserName = '{0}';", userName);
            var mail = _dbWrapper.QueryValue<string>(sql);
            return mail;
        }
        public string GetPassword(string userName)
        {
            var sql = string.Format("SELECT Password FROM Users where UserName = '{0}';", userName);
            var password = _dbWrapper.QueryValue<string>(sql);
            return password;
        }

        public User GetUser(string userName)
        {
            var sql = string.Format("SELECT UserName, FirstName, LastName, Email, PhoneNumber, " +
                                    "Banned, Type, KeyWordsSearches from users " +
                                    "where UserName = '{0}';", userName);
            User user = null;

            return _dbWrapper.RunCommand(command =>
            {
                command.CommandText = sql;
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    userName = reader.GetString(0);
                    var firstName = reader.GetString(1);
                    var lastName = reader.GetString(2);
                    var email = reader.GetString(3);
                    var phoneNumber = reader.GetString(4);
                    var banned = reader.GetBoolean(5);
                    var type = reader.GetString(6);
                    var keyWordsSearchers = reader.GetString(7);
                    user = new User(userName, firstName, lastName, email, phoneNumber, banned,
                        type, keyWordsSearchers);
                }
                return user;
            });
        }

        public void AddUser(User user, string password, string type = "User")
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
            _dbWrapper.QueryValue<object>(sql);
        }

        public int GetID(string userName)
        {
            var sql = string.Format("Select ID from Users where UserName = '{0}';", userName);
            var id = _dbWrapper.QueryValue<int>(sql);
            return id;
        }
        public void DeletePreferences(string userName)

        {
            var id = GetID(userName);
            var sql = string.Format("Delete from ChoosePreferences where IDUser = {0};",id);
            _dbWrapper.QueryValue<object>(sql);
        }

        public void DeleteGroups(string userName)
        {
            var id = GetID(userName);
            var sql = string.Format("Delete from ChooseGroups where IDUser = {0};", id);
            _dbWrapper.QueryValue<object>(sql);
        }

        public int GetNewsletter(string userName)
        {
            var sql = string.Format("Select Newsletter from Users where UserName = '{0}';", userName);
            var n = _dbWrapper.QueryValue<int>(sql);
            return n;
        }

        public void UpdateUser(string user, bool newsletter, string email, string password, string passwordConfirm, List<string>preferences, List<string>groups)
        {

            if (email != GetMail(user))
            {
                var sqlemail = string.Format("Update Users set Email = '{0}' where UserName = '{1}';", email, user);
                _dbWrapper.QueryValue<object>(sqlemail);
            }
            if (password != "Password")
            {
                if (password == passwordConfirm)

                {
                    var input = CryptographicBuffer.ConvertStringToBinary(password,
                        BinaryStringEncoding.Utf8);
                    var hasher = HashAlgorithmProvider.OpenAlgorithm("SHA256");
                    var hashed = hasher.HashData(input);
                    var pwd = CryptographicBuffer.EncodeToBase64String(hashed);
                    var sqlpassword = string.Format("Update Users set Password = '{0}' where UserName = '{1}';", pwd, user);
                    _dbWrapper.QueryValue<object>(sqlpassword);
                }
            }

            DeletePreferences(user);
            DeleteGroups(user);
            var idUser = GetID(user);
            
            foreach (var preference in preferences)
            {
                var id = _dbWrapper.QueryValue<int>(string.Format("Select Id from Preferences Where Name='{0}'", preference));
                var sqlpreferences = string.Format("INSERT INTO ChoosePreferences(IDUser,IDPreference) values('{0}','{1}');", idUser, id);
                _dbWrapper.QueryValue<object>(sqlpreferences);
            }

            foreach (var group in groups)
            {
                var id = _dbWrapper.QueryValue<int>(string.Format("Select Id from Groups Where Name='{0}'", group));
                var sqlgroups = string.Format("INSERT INTO ChooseGroups(IDUser,IDGroup) values('{0}','{1}');", idUser, id);
                _dbWrapper.QueryValue<object>(sqlgroups);
            }

            if (newsletter)
            {
                var sqlnewsletter = string.Format("Update Users set Newsletter = 1 where UserName = '{0}';", user);
                _dbWrapper.QueryValue<object>(sqlnewsletter);
            }
            else
            {
                var sqlnewsletter = string.Format("Update Users set Newsletter = 0 where UserName = '{0}';", user);
                _dbWrapper.QueryValue<object>(sqlnewsletter);
            }
        }

        public List<string> GetOrders(string userName)
        {
            var idUser = GetID(userName);
            return _dbWrapper.RunCommand(command =>
            {
                command.CommandText = string.Format("Select Status from Orders where IDUser = {0} ;", idUser);
                   
                var reader = command.ExecuteReader();
                var list = new List<String>();
                while (reader.Read())
                {
                    var name = reader.GetString(0);

                    list.Add(name);
                }
                return list;
            });
        }
        public bool CheckIfUserExists(string userName)
        {
            var sql = string.Format("Select ID From Users Where UserName = {0};", userName);
            if (_dbWrapper.QueryValue<int>(sql) == 0) return false;
            return true;
        }
        public bool CheckIfEmailExists(string email)
        {
            var sql = string.Format("Select ID From Users Where Email = {0};", email);
            if (_dbWrapper.QueryValue<int>(sql) == 0) return false;
            return true;
        }
        public void AddUser(User user, string password, List<string> preferences, List<string> groups, string type = "User")
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
            sql = string.Empty;
            foreach (var preference in preferences)
            {
                var id = _dbWrapper.QueryValue<int>(string.Format("Select Id from Preferences Where Name='{0}'", preference));
                sql += string.Format("INSERT INTO ChoosePreferences(IDUser,IDPreference) values('{0}','{1}');", idUser, id);
            }
            foreach (var group in groups)
            {
                var id = _dbWrapper.QueryValue<int>(string.Format("Select Id from Groups Where Name='{0}'", group));
                sql += string.Format("INSERT INTO ChooseGroups(IDUser,IDGroup) values('{0}','{1}');", idUser, id);
            }
            _dbWrapper.QueryValue<object>(sql);
        }

        public List<String> GetAllEmails()
        {
            return _dbWrapper.RunCommand(command =>
            {
                command.CommandText = "Select Email from Users;";
                var reader = command.ExecuteReader();
                var list = new List<String>();
                while (reader.Read())
                {
                    var email = reader.GetString(0);
                    list.Add(email);
                }
                return list;
            });
        }
        public List<String> GetPreferencesCountryUser(string userName)
        {
            return _dbWrapper.RunCommand(command =>
            {
                command.CommandText = string.Format("Select Name from Preferences, ChoosePreferences, Users where " +
                    "Preferences.ID = ChoosePreferences.IDPreference and Category = 'Country' and " +
                    "ChoosePreferences.IDUser = Users.ID and UserName = '{0}'; ", userName);
                var reader = command.ExecuteReader();
                var list = new List<String>();
                while (reader.Read())
                {
                    var name = reader.GetString(0);

                    list.Add(name);
                }
                return list;
            });
        }

        public List<String> GetPreferencesTypeUser(string userName)
        {
            return _dbWrapper.RunCommand(command =>
            {
                command.CommandText = string.Format("Select Name from Preferences, ChoosePreferences, Users where " +
                                                    "Preferences.ID = ChoosePreferences.IDPreference and Category = 'Type' and " +
                                                    "ChoosePreferences.IDUser = Users.ID and UserName = '{0}'; ",
                    userName);
                var reader = command.ExecuteReader();
                var list = new List<String>();
                while (reader.Read())
                {
                    var name = reader.GetString(0);

                    list.Add(name);
                }
                return list;
            });
        }

        public List<string> GetStrings(string sql)
        {
            return _dbWrapper.RunCommand(command =>
            {
                command.CommandText = sql;
                var reader = command.ExecuteReader();
                var list = new List<String>();
                while (reader.Read())
                {
                    var email = reader.GetString(0);
                    list.Add(email);
                }
                return list;
            });
        }

        public void AddPreference(Preference preference)
        {
            var sql = string.Format("INSERT INTO Preferences(Name,Category) values ('{0}','{1}');", preference.Name, preference.Category);
            _dbWrapper.QueryValue<object>(sql);
        }

        public void RemoveUser(string userName)
        {
            var sql = string.Format("DELETE FROM ChoosePreferences" +
                                    " WHERE IDUser = (SELECT ID FROM Users Where UserName = {0});",
                                    userName);
            sql += string.Format("DELETE FROM ChooseGroups" +
                                   " WHERE IDUser = (SELECT ID FROM Users Where UserName = {0});",
                                   userName);
            sql += string.Format("Delete from Users where UserName = '{0}';", userName);
            _dbWrapper.QueryValue<object>(sql);
        }

        public void BanUser(string userName)
        {
            var sql = string.Format("UPDATE Users set Banned = true " +
                                    "WHERE UserName = '{0}';", userName);
            _dbWrapper.QueryValue<object>(sql);
        }

        public void UnbanUser(string userName)
        {
            var sql = string.Format("UPDATE Users set Banned = false " +
                                   "WHERE UserName = '{0}';", userName);
            _dbWrapper.QueryValue<object>(sql);
        }

        public void Login(string userName, string password)
        {
            if (CanLogin(userName, password))
                MainPage.CurrentUser = GetUser(userName);
        }

        public bool CanLogin(string userName, string password)
        {
            bool fieldsCompleted = userName != String.Empty && password!= String.Empty;
            if (MainPage.CurrentUser == null && fieldsCompleted && CheckIfUserExists(userName) == true)
            {
                var currUser = GetUser(userName);
                if (currUser.Banned)
                {
                    String message = "You have been baned!";
                    DisplayPopup(message);
                    return false;
                }

                if (CheckCredentials(userName, password) == false)
                {
                    String message = "Incorrect username or password!";
                    DisplayPopup(message);
                    return false;
                }
                return true;
            }
        
            if (!fieldsCompleted)
            {
                String message = "All fields are mandatory!";
                DisplayPopup(message);
                return false;
            }

          if (CheckIfUserExists(userName) == true)
          {
              String message = "Incorrect credentials!";
              DisplayPopup(message);
              return false;
          }

          return false;
        }

        private void DisplayPopup(string message)
        {
            Popup validationPopup = new Popup();
            validationPopup.VerticalOffset = 200;
            validationPopup.HorizontalOffset = 300;
            
             TextBlock popupText = new TextBlock();
             popupText.Text = message;
             validationPopup.Child = popupText;
        }

        private string EncryptPassword(string userName, string password)
        {
            var input = CryptographicBuffer.ConvertStringToBinary(password,
            BinaryStringEncoding.Utf8);
            var hasher = HashAlgorithmProvider.OpenAlgorithm("SHA256");
            var hashed = hasher.HashData(input);
            var pwd = CryptographicBuffer.EncodeToBase64String(hashed);

            return pwd.ToString();
        }

    }
}
