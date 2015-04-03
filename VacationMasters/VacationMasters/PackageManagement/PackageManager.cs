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

        public void AddPackage(Package pack )
        {
            var sql = string.Format(" INSERT INTO Packages(Name,Type,Included,Transport,Price,SearchIndexRating,"
                                      + "BeginDate,EndDate,Picture)"
                                      + "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, '{7}', '{8}', '{9}');",
                                      pack.Name,pack.Type,pack.Included,pack.Transport,pack.Price,pack.SearchIndexRate,
                                      pack.BeginDate,pack.EndDate,pack.Picture);
            _dbWrapper.QueryValue<object>(sql);
                            
        }

       
    }
}
