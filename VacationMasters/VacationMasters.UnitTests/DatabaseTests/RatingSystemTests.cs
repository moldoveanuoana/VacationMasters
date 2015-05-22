using NUnit.Framework;
using VacationMasters.PackageManagement;
using VacationMasters.UserManagement;
using VacationMasters.Wrappers;

namespace VacationMasters.UnitTests.DatabaseTests
{
    public class RatingSystemTests
    {
        private IDbWrapper _dbWrapper;
        private IUserManager _userManagement;
        private PackageManager _packageManager;

        [SetUp]
        public void SetUp()
        {
            _dbWrapper = new DbWrapper();
            _userManagement = new UserManager(_dbWrapper);
            _packageManager = new PackageManager(_dbWrapper);
        }

        [Test]
        public void DummyTest()
        {

        }

    }
}
