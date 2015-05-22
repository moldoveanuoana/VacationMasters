using System.Collections.Generic;
using NUnit.Framework;
using VacationMasters.Essentials;
using VacationMasters.UnitTests.Infrastructure;
using VacationMasters.UserManagement;
using VacationMasters.Wrappers;
using VacationMasters.PackageManagement;
using System.Linq;
using System;

namespace VacationMasters.UnitTests.DatabaseTests
{
    class DatabaseCommandsTests
    {
        private IDbWrapper _dbWrapper;
        private IUserManager _userManagement;
        private PackageManager _packageManager;
        private DbWrapper _db;

        [SetUp]
        public void SetUp()
        {
            _dbWrapper = new DbWrapper();
            _db = new DbWrapper();
            _userManagement = new UserManager(_dbWrapper);
            _packageManager = new PackageManager(_dbWrapper);
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
        public void AddUserWithPreferencesAndGroupsShouldNotThrow()
        {
            var password = CreateRandom.String();
            var user = CreateRandomUser();
            var preferences = new List<string>();
            preferences.Add("Germania");
            preferences.Add("Citibreak");
            var groups = new List<string>{"Grupul iubitorilor de mare"};
            Assert.DoesNotThrow(() => _userManagement.AddUser(user, password, preferences,groups));
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

        [Test]
        public void MultipleUsersCanLoginAtTheSameTime()
        {
            var password1 = CreateRandom.String();
            var user1 = CreateRandomUser();

            var password2 = CreateRandom.String();
            var user2 = CreateRandomUser();

            _userManagement.AddUser(user1, password1);
            _userManagement.AddUser(user2, password2);

            Assert.DoesNotThrow(() => _userManagement.Login(user1.UserName, password1));
            Assert.DoesNotThrow(() => _userManagement.Login(user2.UserName, password2));
            Assert.IsTrue(_userManagement.CurrentUser.UserName == user1.UserName);
            Assert.IsFalse(_userManagement.CurrentUser.UserName == user2.UserName);

            _userManagement.RemoveUser(user1.UserName);
            _userManagement.RemoveUser(user2.UserName);
        }

        [Test]
        public void ValidUserWithPasswordPassCredentialChecking()
        {
            var password = CreateRandom.String();
            var user = CreateRandomUser();

            _userManagement.AddUser(user, password);

            Assert.DoesNotThrow(() => _userManagement.Login(user.UserName, password));
            Assert.NotNull(_userManagement.CurrentUser);

            _userManagement.RemoveUser(user.UserName);
        }

        [Test]
        public void ValidUserWithWrongPasswordFailsCredentialChecking()
        {
            var password = CreateRandom.String();
            var password1 = CreateRandom.String();
            var user = CreateRandomUser();

            Assert.DoesNotThrow(() => _userManagement.Login(user.UserName, password1));
            Assert.IsNull(_userManagement.CurrentUser);
        }
        
        [Test]
        public void NonExistingUserTriesToLogin()
        {
           var password = CreateRandom.String();
           var user = CreateRandomUser();

           Assert.DoesNotThrow(() => _userManagement.Login(user.UserName, password));
           Assert.IsNull(_userManagement.CurrentUser);
        }

        public void UserCanChangeHisPassword()
        {
            //TODO:this
        }

        
        public Package CreateTestPackage()
        {
            var package = new Package("testpack", "croaziera", "chestii", "vapor", 7000.0, 2.0, 4.0,new DateTime(2015,7,16),new DateTime(2015,7,26),null);
            return package;
        }

        [Test]
        public void InsertingNewPackageShouldNotThrow()
        {
            var pack = CreateTestPackage();

            Assert.DoesNotThrow(()=>_packageManager.AddPackage(pack));
            Assert.DoesNotThrow(()=>_packageManager.RemovePackage(pack));
        }

        [Test]
        public void GivenNameSelectPackageByName()
        {
            var pack = CreateTestPackage();

            _packageManager.AddPackage(pack);

            var list = _db.GetPackagesByName(pack.Name);
            Assert.That(list.FirstOrDefault().Name == pack.Name);

            _packageManager.RemovePackage(pack);

        }

        [Test]
        public void GivenPriceSelectPackageByPrice()
        {
            var pack = CreateTestPackage();

            _packageManager.AddPackage(pack);

            var list = _db.GetPackagesByPrice(1000.0, 8000.0);
            foreach(Package item in list)
            {
                if (item.Name.Equals("testpack"))
                    Assert.That(item.Price == pack.Price);
            }

            _packageManager.RemovePackage(pack);
        }

        [Test]
        public void GivenDateSelectPackageByDate()
        {
            var pack = CreateTestPackage();

            _packageManager.AddPackage(pack);

            var list = _db.GetPackagesByDate(new DateTime(2015, 7, 16), new DateTime(2015, 7, 26));
            foreach (Package item in list)
            {
                if (item.Name.Equals("testpack"))
                    Assert.That(item.BeginDate == pack.BeginDate && item.EndDate == pack.EndDate);
            }

            _packageManager.RemovePackage(pack);
        }

        [Test]
        public void GivenTypeSelectPackageByType()
        {
            var pack = CreateTestPackage();

            _packageManager.AddPackage(pack);
            var list = _db.getPackagesByType("croaziera");
            foreach (Package item in list)
            {
                if (item.Name.Equals("testpack"))
                    Assert.That(item.Type == pack.Type);
            }

            _packageManager.RemovePackage(pack);
        }

        [Test]
        public void DummyTest()
        {
            
        }


    }
}
