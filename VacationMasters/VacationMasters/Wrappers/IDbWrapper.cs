using System;
using MySql.Data.MySqlClient;
using VacationMasters.Essentials;
using System.Collections.Generic;

namespace VacationMasters.Wrappers
{
    public interface IDbWrapper
    {
        /// <summary>
        /// Get an opened connection
        /// </summary>
        /// <returns></returns>
        MySqlConnection GetConnection();

        /// <summary>
        /// Get a clear valid command
        /// </summary>
        /// <param name="connectionWrapper"></param>
        /// <returns></returns>
        MySqlCommand GetCommand(MySqlConnection connectionWrapper);

        /// <summary>
        /// Execute a valid SQL query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        T QueryValue<T>(string sqlQuery);

        /// <summary>
        /// Run a sql command
        /// </summary>
        /// <param name="func"></param>
        T RunCommand<T>(Func<MySqlCommand, T> func);

        /// <summary>
        /// Gets packages 
        /// </summary>
        /// <param name="sqlQuery"></param>
        List<Package> ReadPackages(MySqlCommand sqlQuery);

        /// <summary>
        /// Gets all packages 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<Package> GetPackagesByName(String name);

        /// <summary>
        /// Gets all packages that 
        /// </summary>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        List<Package> GetPackagesByPrice(double minPrice, double maxPrice);

        List<Package> GetPackagesByDate(DateTime beginDate, DateTime endDate);

        List<Package> getPackagesByType(String type);

        List<Package> getPackagesByPriceDate(double minPrice, double maxPrice, DateTime beginDate, DateTime endDate);

        List<Package> getPackagesByPriceType(double minPrice, double maxPrice, String type);

        List<Package> getPackagesByDateType(DateTime beginDate, DateTime endDate, String type);

        List<Package> getPackagesByNamePrice(String name, double minPrice, double maxPrice);

        List<Package> getPackagesByNameDate(String name, DateTime beginDate, DateTime endDate);

        List<Package> getPackagesByNameType(String name, String type);

        List<Package> getPackagesByNamePriceDate(String name, double minPrice, double maxPrice, DateTime beginDate,
            DateTime endDate);

        List<Package> getPackagesByNamePriceType(String name, double minPrice, double maxPrice, String type);

        List<Package> getPackagesByNameDateType(String name, DateTime beginDate, DateTime endDate, String type);

        List<Package> getPackagesByPriceDateType(double minPrice, double maxPrice, DateTime beginDate, DateTime endDate,
            String type);

        List<Package> getPackagesByAll(String name, double minPrice, double maxPrice, DateTime beginDate,
            DateTime endDate, String type);

        List<String> GetTypes();
    }
}
