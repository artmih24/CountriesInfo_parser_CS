using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Test_task
{
    /// <summary>
    /// класс DB - отвечает за взаимодействие с базой данных
    /// </summary>
    class DB
    {
        /// <summary>
        /// строка подключения к СУБД, считывается из файла конфигурации
        /// </summary>
        private static string connectionString;
        // = "Data Source=ARTEM-PC;Initial Catalog=Countries_DB;Integrated Security=True";

        /// <summary>
        /// отправление запроса в СУБД 
        /// </summary>
        /// <param name="query">строка запроса</param>
        /// <returns>DataTable - таблица данных из БД</returns>
        public static DataTable SendQuery(string query)
        {
            connectionString = ConfigurationManager.GetConnectionString("connection.config");
            // инициализация объекта класса SqlConnection для подключения к БД
            var connection = new SqlConnection(connectionString);
            var result = new DataTable();
            try
            {
                // отправка запроса в СУБД
                connection.Open();
                var command = new SqlCommand(query, connection);
                var adapter = new SqlDataAdapter(command);
                adapter.Fill(result);
            }
            catch (SqlException e)
            {
                Dialog.Message(e.Message);
            }
            catch (TimeoutException e)
            {
                Dialog.Message(e.Message);
            }
            catch (Exception e)
            {
                Dialog.Message(e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return result;
        }
    }
}
