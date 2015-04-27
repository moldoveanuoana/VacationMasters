using System.Collections.Generic;
using VacationMasters.Essentials;

namespace VacationMasters.UserManagement
{
    public interface IUserManager
    {
        User CurrentUser { get; set; }

        /// <summary>
        /// checks whether a given user satisfies the login conditions
        /// Conditions: existing and not banned
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool CanLogin(User user);

        /// <summary>
        /// Checks whether the combination UserName/Password worked or not
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool CheckCredentials(string userName, string password);

        /// <summary>
        /// Change password of an user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        void ChangePassword(string userName, string newPassword);

        /// <summary>
        /// Returns an user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        User GetUser(string userName);

        /// <summary>
        /// Add new user without preferences to the database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="type"></param>
        void AddUser(User user, string password, string type = "User");

        /// <summary>
        /// Adds new user with preferences to the database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="preferencesId"></param>
        /// <param name="type"></param>
        void AddUser(User user, string password, List<int> preferencesId, string type = "User");

        /// <summary>
        /// Remove user from database
        /// </summary>
        /// <param name="userName"></param>
        void RemoveUser(string userName);

        /// <summary>
        /// Bans a user, restrict access
        /// </summary>
        /// <param name="userName"></param>
        void BanUser(string userName);

        /// <summary>
        /// Lifts ban for a user, give access back
        /// </summary>
        /// <param name="userName"></param>
        void UnbanUser(string userName);
    }
}
