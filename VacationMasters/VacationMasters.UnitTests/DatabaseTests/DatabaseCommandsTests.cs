using System.Collections.Generic;
using NUnit.Framework;
using VacationMasters.Essentials;
using VacationMasters.UnitTests.Infrastructure;
using VacationMasters.UserManagement;
using VacationMasters.Wrappers;

namespace VacationMasters.UnitTests.DatabaseTests
{
    class DatabaseCommandsTests
    {
        private IDbWrapper _dbWrapper;
        private IUserManager _userManagement;

        [SetUp]
        public void SetUp()
        {
            _dbWrapper = new DbWrapper();
            _userManagement = new UserManager(_dbWrapper);
        }

        private User CreateRandomUser()
        {
            var user = new User(CreateRandom.String(), CreateRandom.String(), CreateRandom.String(),
                CreateRandom.String(), CreateRandom.String(), false, CreateRandom.String(), CreateRandom.String());

            return user;
        }

        [Test]
        public void LMDShouldNotThrow()
        {
            var password = CreateRandom.String();
            var user = CreateRandomUser();

            Assert.DoesNotThrow(() => _userManagement.AddUser(user, password));
            Assert.DoesNotThrow(() => _userManagement.RemoveUser(user.UserName));
        }

        [Test]
        public void AddUserWithPreferencesShouldNotThrow()
        {
            var password = CreateRandom.String();
            var user = CreateRandomUser();
            var preferences = new List<int>();

            preferences.Add(1);
            preferences.Add(2);

            Assert.DoesNotThrow(() => _userManagement.AddUser(user, password, preferences));
            Assert.DoesNotThrow(() => _userManagement.RemoveUser(user.UserName));
        }

        [Test]
        public void GetUserShouldNotThrow()
        {
            var password = CreateRandom.String();
            var user = CreateRandomUser();

            _userManagement.AddUser(user, password);

            var localUser = _userManagement.GetUser(user.UserName);

            Assert.That(user.UserName == localUser.UserName);

            _userManagement.RemoveUser(user.UserName);
        }

        [Test]
        public void BanUserShouldNotThrow()
        {
            var password = CreateRandom.String();
            var user = CreateRandomUser();
            user.Banned = true;

            _userManagement.AddUser(user, password);

            _userManagement.BanUser(user.UserName);
            var localUser = _userManagement.GetUser(user.UserName);
            Assert.That(localUser.Banned == user.Banned);

            _userManagement.RemoveUser(user.UserName);
        }

        [Test]
        public void UnbanUserShouldNotThrow()
        {
            var password = CreateRandom.String();
            var user = CreateRandomUser();
            user.Banned = false;

            _userManagement.AddUser(user, password);

            _userManagement.UnbanUser(user.UserName);
            var localUser = _userManagement.GetUser(user.UserName);
            Assert.That(localUser.Banned == user.Banned);

            _userManagement.RemoveUser(user.UserName);
        }

        public void MultipleUsersCanLoginAtTheSameTime()
        {
            //TODO:this
        }

        public void ValidUserWithPasswordPassCredentialChecking()
        {
            //TODO:this
        }

        public void ValidUserWithWrongPasswordFailsCredentialChecking()
        {
            //TODO:this
        }

        public void NonExistingUserTriesToLogin()
        {
            //TODO:this
        }

        public void UserCanChangeHisPassword()
        {
            //TODO:this
        }

    }
}
