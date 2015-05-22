using System;
using MySql.Data.MySqlClient;
using VacationMasters.Essentials;
using System.Collections.Generic;

namespace VacationMasters.Wrappers
{
    public class DbWrapper : IDbWrapper
    {
        private MySqlConnection CreateConnection()
        {
            var connection = new MySqlConnection("server=galactica.emanuelscirlet.com;database=vacationmasters;uid=sa;password=vacationmasters12;");
            return connection;
        }

        private MySqlCommand CreateCommand(MySqlConnection connection)
        {
            var command = new MySqlCommand { Connection = connection };
            return command;
        }

        public MySqlConnection GetConnection()
        {
            var connection = CreateConnection();
            connection.Open();
            return connection;
        }

        public MySqlCommand GetCommand(MySqlConnection connection)
        {
            var command = CreateCommand(connection);
            command.Parameters.Clear();
            command.CommandType = CommandType.Text;
            return command;
        }

        private async void MessageBox(string msg)
        {
            //var msgDlg = new Windows.UI.Popups.MessageDialog(msg) { DefaultCommandIndex = 1 };
            //await msgDlg.ShowAsync();
        }

        private void RunCommand(Action<MySqlCommand> func)
        {
            RunCommand(c =>
            {
                func(c);
                return true;
            });
        }

        public T RunCommand<T>(Func<MySqlCommand, T> func)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    using (var command = GetCommand(connection))
                    {
                        return func(command);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox("An error occured!!\n" + e.Message);
                return default(T);
            }
        }

        public T QueryValue<T>(string sqlQuery)
        {
            return RunCommand(command =>
            {
                command.CommandText = sqlQuery;
                var result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    return default(T);
                }
                Type t = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                return (T)Convert.ChangeType(result, t);
            });
        }
        public List<Preference> GetAllPreferences()
        {
            return RunCommand(command =>
            {
                command.CommandText = "Select * from Preferences";
                var reader = command.ExecuteReader();
                var list = new List<Preference>();
                while (reader.Read())
                {
                    var preference = new Preference();
                    preference.ID = reader.GetInt32(0);
                    preference.Name = reader.GetString(1);
                    preference.Category = reader.GetString(2);
                    list.Add(preference);
                }
                return list;
            });
        }

        /* public List<Package> GetAllPackages()
         {
             return RunCommand(command =>
                 {
                     command.CommandText = "SELECT * FROM Packages";
                     var reader = command.ExecuteReader();
                     var list = new List<Package>();
                     while(reader.Read())
                     {
                         var pack = new Package();
                         pack.ID = reader.GetInt32(0);
                         pack.Name = reader.GetString(1);
                         pack.Type = reader.GetString(2);
                         pack.Included = reader.GetString();
                         pack.Transport = reader.GetString();
                         pack.Price = reader.GetDouble();
                         pack.SearchIndexRate = reader.GetDouble();
                         pack.BeginDate = reader.GetDateTime();
                         pack.EndDate = reader.GetDateTime();
                         pack.Picture = reader.GetBytes();
                         list.Add(pack);
                        
                     }
                     return list;
                 });
         }*/


        public List<String> GetTypes()
        {
            return RunCommand(command =>
            {
                command.CommandText = " SELECT DISTINCT type FROM Packages";
                var reader = command.ExecuteReader();
                var list = new List<String>();
                while (reader.Read())
                {
                    String t = reader.GetString(0);
                    list.Add(t);
                }
                return list;
            });
        }

        public List<Package> ReadPackages(MySqlCommand command)
        {
            var reader = command.ExecuteReader();
            var list = new List<Package>();
            while (reader.Read())
            {
                var pack = new Package();
                pack.ID = reader.GetInt32(0);
                pack.Name = reader.GetString(1);
                pack.Type = reader.GetString(2);
                pack.Included = reader.GetString(3);
                pack.Transport = reader.GetString(4);
                pack.Price = reader.GetDouble(5);
                pack.SearchIndex = reader.GetDouble(6);
                pack.Rating = reader.GetDouble(7);
                pack.BeginDate = reader.GetDateTime(8);
                pack.EndDate = reader.GetDateTime(9);
                pack.Picture = reader[10] as byte[];
                list.Add(pack);
            }
            return list;
        }

        public List<Package> GetPackagesByName(String name)
        {
            return RunCommand(command =>
                {
                    command.CommandText = "SELECT * FROM Packages WHERE Name = '" + name + "'";
                    return ReadPackages(command);
                });
        }

        public List<Package> GetPackagesByPrice(double minPrice, double maxPrice)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE Price BETWEEN " + minPrice + " AND " + maxPrice;
                return ReadPackages(command);
            });
        }



        public List<Package> GetPackagesByDate(DateTime beginDate, DateTime endDate)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE BeginDate = " + beginDate.ToString("yyyy-MM-dd") + " AND EndDate = " + endDate.ToString("yyyy-MM-dd");
                return ReadPackages(command);
            });
        }

        public List<Package> getPackagesByType(String type)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE Type = '" + type + "'";
                return ReadPackages(command);
            });

        }

        public List<Package> getPackagesByPriceDate(double minPrice, double maxPrice, DateTime beginDate, DateTime endDate)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE BeginDate = " + beginDate + " AND EndDate = " + endDate
                                      + " AND Price BETWEEN " + minPrice + " AND " + maxPrice;
                return ReadPackages(command);
            });
        }

        public List<Package> getPackagesByPriceType(double minPrice, double maxPrice, String type)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE Price BETWEEN " + minPrice + " AND " + maxPrice
                                      + " AND Type = '" + type + "'";
                return ReadPackages(command);
            });
        }

        public List<Package> getPackagesByDateType(DateTime beginDate, DateTime endDate, String type)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE BeginDate = " + beginDate + " AND EndDate = " + endDate
                                       + " AND Type = '" + type + "'";
                return ReadPackages(command);
            });
        }

        public List<Package> getPackagesByNamePrice(String name, double minPrice, double maxPrice)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE Name = '" + name + "'"
                                      + " AND Price BETWEEN " + minPrice + " AND " + maxPrice;
                return ReadPackages(command);
            });
        }

        public List<Package> getPackagesByNameDate(String name, DateTime beginDate, DateTime endDate)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE Name = '" + name + "'"
                                      + " AND BeginDate = " + beginDate + " AND EndDate = " + endDate;
                return ReadPackages(command);
            });
        }

        public List<Package> getPackagesByNameType(String name, String type)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE Name = '" + name + "'"
                                      + " AND Type = '" + type + "'";
                return ReadPackages(command);
            });
        }

        public List<Package> getPackagesByNamePriceDate(String name, double minPrice, double maxPrice, DateTime beginDate, DateTime endDate)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE Name = '" + name + "'"
                                       + " AND Price BETWEEN " + minPrice + " AND " + maxPrice
                                       + " AND BeginDate = " + beginDate + " AND EndDate = " + endDate;
                return ReadPackages(command);
            });
        }

        public List<Package> getPackagesByNamePriceType(String name, double minPrice, double maxPrice, String type)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE Name = '" + name + "'"
                                       + " AND Price BETWEEN " + minPrice + " AND " + maxPrice
                                       + " AND Type = '" + type + "'";
                return ReadPackages(command);
            });
        }

        public List<Package> getPackagesByNameDateType(String name, DateTime beginDate, DateTime endDate, String type)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE Name = '" + name + "'"
                                        + " AND BeginDate = " + beginDate + " AND EndDate = " + endDate
                                       + " AND Type = '" + type + "'";
                return ReadPackages(command);
            });
        }

        public List<Package> getPackagesByPriceDateType(double minPrice, double maxPrice, DateTime beginDate, DateTime endDate, String type)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE Price BETWEEN " + minPrice + " AND " + maxPrice
                                      + " AND BeginDate = " + beginDate + " AND EndDate = " + endDate
                                      + " AND Type = '" + type + "'";
                return ReadPackages(command);
            });
        }

        public List<Package> getPackagesByAll(String name, double minPrice, double maxPrice, DateTime beginDate, DateTime endDate, String type)
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages WHERE Name = '" + name + "'"
                                       + " AND Price BETWEEN " + minPrice + " AND " + maxPrice
                                       + " AND BeginDate = " + beginDate + " AND EndDate = " + endDate
                                       + " AND Type = '" + type + "'";
                return ReadPackages(command);
            });
        }

        public void UpdateRating(int ID, int starNum)
        {
            RunCommand(command =>
            {
                command.CommandText = string.Format("UPDATE Packages SET Rating = ((Rating * TotalVotes) + {0})/(TotalVotes+1),TotalVotes = TotalVotes+1 ", starNum);
            });
        }

        public List<Package> getRandomPackages()
        {
            return RunCommand(command =>
            {
                command.CommandText = "SELECT * FROM Packages ORDER BY SearchIndex DESC";
                return ReadPackages(command);
            });
        }

    }
}
