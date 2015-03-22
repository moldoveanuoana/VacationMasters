using MySql.Data.MySqlClient;

namespace VacationMasters.Wrappers
{
    public interface IDbWrapper
    {
        MySqlConnection GetConnection();
        MySqlCommand GetCommand(MySqlConnection connectionWrapper);
        T QueryValue<T>(string sqlQuery);
    }
}
