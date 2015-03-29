﻿using NUnit.Framework;
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
            _userManagement = new UserManagement.UserManager(_dbWrapper);
        }

        [Test]
        public void LMDShouldNotThrow()
        {
            var password = CreateRandom.String();
            var user = new User(CreateRandom.String(), CreateRandom.String(), CreateRandom.String(),
                CreateRandom.String(), CreateRandom.String(), false, CreateRandom.String(), CreateRandom.String());

            _userManagement.AddUser(user, password);
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
