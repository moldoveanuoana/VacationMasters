using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using VacationMasters.Wrappers;

namespace VacationMasters.UnitTests.DatabaseTests
{
    class DatabaseCommandsTests
    {
        private IDbWrapper _dbWrapper;

        [SetUp]
        public void SetUp()
        {
            _dbWrapper = new DbWrapper();
        }

        [Test]
        public void InsertNewUserShouldNotThrow()
        {
           //I need classes that match DB schema
        }
    }
}
