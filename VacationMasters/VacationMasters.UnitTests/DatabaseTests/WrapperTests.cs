using FluentAssertions;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using VacationMasters.Wrappers;

namespace VacationMasters.UnitTests.DbWrapperTests
{
    [TestFixture]
    public class WrapperTests
    {
        private IDbWrapper _dbWrapper;

        [SetUp]
        public void SetUp()
        {
            _dbWrapper = new DbWrapper();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void QueryWrapperTest()
        {
            var sql = "Select ID from teste";
            var x = _dbWrapper.QueryValue<string>(sql);
            x.Should().Be("100");
        }

        [Test]
        public void GivenNormalConditionsConnectionToMySqlServerShouldNotFail()
        {
            var connection = _dbWrapper.GetConnection();
            var connected = connection.State == ConnectionState.Open;
            Assert.IsTrue(connected);
        }

        [Test]
        public void ConnectionDisconnectionShouldNotFail()
        {
            var connection = _dbWrapper.GetConnection();
            var connected = connection.State == ConnectionState.Open;
            Assert.IsTrue(connected);
            connection.Close();
            var disconnected = connection.State == ConnectionState.Closed;
            Assert.IsTrue(disconnected);
        }
    }
}
