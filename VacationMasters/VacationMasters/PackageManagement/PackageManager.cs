using System.Collections.Generic;
using VacationMasters.Essentials;
using VacationMasters.UserManagement;
using VacationMasters.Wrappers;
using System.Linq;


namespace VacationMasters.PackageManagement
{
    public class PackageManager
    {

        private readonly IDbWrapper _dbWrapper;
        private readonly IUserManager _userManager;

        public PackageManager(IDbWrapper dbWrapper)
        {
            _dbWrapper = dbWrapper;
            _userManager = new UserManager(_dbWrapper);
        }

        public void AddPackage(Package package)
        {
            var sql = string.Format("INSERT INTO Packages(Name, Type, Included, Transport, Price, SearchIndex, Rating,"
                                      + "BeginDate, EndDate, Picture) "
                                      + "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, '{7}', '{8}', '{9}');",
                                      package.Name, package.Type, package.Included, package.Transport, package.Price,
                                      package.SearchIndex, package.Rating, package.BeginDate.ToString("yyyy-MM-dd HH:mm:ss"), package.EndDate.ToString("yyyy-MM-dd HH:mm:ss"), package.Picture);
            _dbWrapper.QueryValue<object>(sql);
        }

        public void RemovePackage(Package package)
        {
            var sql = string.Format("DELETE FROM Packages WHERE ID = {0} ", package.ID);
            _dbWrapper.QueryValue<object>(sql);
        }

        public List<Package> GetPackagesByPreferences()
        {
            User loggedUser = _userManager.CurrentUser;
            List<Package> packagesByPrefrences = new List<Package>();
            foreach (var pref in loggedUser.Preferences)
            {
                string sql = string.Format("Select * from packages where Type == {0}", pref.Category);
                List<Package> results = _dbWrapper.RunCommand(command =>
                {
                    command.CommandText = sql;
                    return _dbWrapper.ReadPackages(command);
                });

                foreach (var item in results)
                    packagesByPrefrences.Add(item);

                sql = string.Format("Select * from packages p join ChooseDestinations cd on (p.ID = cd.IDPackage)" +
                "join destinations on (cd.IDDestination = d.ID) where d.Name ={0};", pref.Category);
                results = _dbWrapper.RunCommand(command =>
                {
                    command.CommandText = sql;
                    return _dbWrapper.ReadPackages(command);
                });

                foreach (var item in results)
                    packagesByPrefrences.Add(item);
            }

            var sortedPackagesByPrefrences = packagesByPrefrences.OrderBy(p => p.Rating).ThenBy(p => p.SearchIndex).ToList<Package>();

            return sortedPackagesByPrefrences;
        }

        public List<Package> GetPackagesByHistoric()
        {
            User loggedUser = _userManager.CurrentUser;
            List<Package> packagesByHistoric = new List<Package>();

            string sql = "Select * from packages p join choosepackage cp on(p.ID = cp.IDPackage)" +
                "join Order o on (cp.IDOrder = o.ID)";

            List<Package> results = _dbWrapper.RunCommand(command =>
            {
                command.CommandText = sql;
                return _dbWrapper.ReadPackages(command);
            });

            var typeList = results.Select(p => p.Type);

            foreach(var res in results){
                sql = string.Format("Select Name from destinations d join ChooseDestinations cd on (cd.IDDestination = d.ID)" +
               "join packages p (p.ID = cd.IDPackage) where p.ID ={0};", res.ID);

                var results2 = _dbWrapper.RunCommand(command =>
                {
                
                    command.CommandText = sql;
                    return _dbWrapper.ReadPackages(command);
                });
                
                foreach(var dest in results2)
                    packagesByHistoric.Add(dest);
            }


            var nameList = packagesByHistoric.Select(p => p.Name);
            List<int> frequencyNameArray = new List<int>();
            foreach (var name in nameList)
                frequencyNameArray.Add(nameList.Count(nume => nume == name));
           
            List<int> frequencyTypeArray = new List<int>();
            foreach (var type in typeList)
                frequencyTypeArray.Add(typeList.Count(tip => tip == type));

            int index = 0;

            foreach (var i in frequencyTypeArray)
                if (i == frequencyTypeArray.Max())
                    index = frequencyTypeArray.IndexOf(i);

            var typeMax = frequencyTypeArray.ElementAt(index);

            foreach(var i in frequencyNameArray)
                if(i == frequencyNameArray.Max())
                    index = frequencyNameArray.IndexOf(i);

            var nameMax = frequencyNameArray.ElementAt(index);

            sql = string.Format("Select from preferences d join ChooseDestinations cd on (cd.IDDestination = d.ID)" +
               "join packages p (p.ID = cd.IDPackage) where p.Type = {0} or d.Name = {1};", typeMax, nameMax);

            var selectedPackages = _dbWrapper.RunCommand(command =>
            {

                command.CommandText = sql;
                return _dbWrapper.ReadPackages(command);
            });

            List<Package> selected2Packages = new List<Package>();

            foreach (var item in selectedPackages)
                if (!results.Contains(item))
                    selected2Packages.Add(item);

            selected2Packages.OrderBy(p => p.Rating).ThenBy(p => p.SearchIndex);

            return selected2Packages;

        }

       


    }
}
