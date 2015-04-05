using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationMasters.Essentials;
using VacationMasters.Wrappers;


namespace VacationMasters.PackageManagement
{
    class PackageManager
    {
        private readonly IDbWrapper _dbWrapper;

         public PackageManager(IDbWrapper dbWrapper)
        {
            _dbWrapper = dbWrapper;
        }

        public void AddPackage(Package package)
        {
            var sql = string.Format("INSERT INTO Packages(Name, Type, Included, Transport, Price, SearchIndexRating,"
                                      + "BeginDate, EndDate, Picture) "
                                      + "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, '{7}', '{8}', '{9}');",
                                      package.Name, package.Type, package.Included, package.Transport, package.Price,
                                      package.SearchIndexRate, package.BeginDate, package.EndDate, package.Picture);
            _dbWrapper.QueryValue<object>(sql);
        }

       
    }
}
