using System;
using MySql.Data.MySqlClient;

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
            var msgDlg = new Windows.UI.Popups.MessageDialog(msg) { DefaultCommandIndex = 1 };
            await msgDlg.ShowAsync();
        }

        private void RunCommand(Action<MySqlCommand> func)
        {
            RunCommand(c =>
            {
                func(c);
                return true;
            });
        }

        private T RunCommand<T>(Func<MySqlCommand, T> func)
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
                MessageBox("An error occured! Please check your connection!");
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
    }
}
