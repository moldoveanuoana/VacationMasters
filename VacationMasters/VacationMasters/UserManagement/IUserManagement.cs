using VacationMasters.Essentials;

namespace VacationMasters.UserManagement
{
    public interface IUserManagement
    {
        /// <summary>
        /// checks whether a given user satisfies the login conditions
        /// Conditions: existing and not banned
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool CanLogin(User user);

        /// <summary>
        /// Add new user to the database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        void AddUser(User user, string password);

        /// <summary>
        /// Remove user from database
        /// </summary>
        /// <param name="userName"></param>
        void RemoveUser(string userName);

    }
}
