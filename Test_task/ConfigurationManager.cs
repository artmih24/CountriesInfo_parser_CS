using System.IO;

namespace Test_task
{
    /// <summary>
    /// класс ConfigurationManager - отвечает за применение конфигураций, 
    /// описанных в файлах конфигурации
    /// </summary>
    class ConfigurationManager
    {
        /// <summary>
        /// чтение содержимого текстового файла
        /// </summary>
        /// <param name="filePath">путь к читаемому текстовому файлу</param>
        /// <returns>содержимое текстового файла</returns>
        public static string ReadFile(string filePath)
        {
            var reader = new StreamReader(filePath);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// приведение строки подключения к БД 
        /// к правильному виду
        /// </summary>
        /// <param name="connectionString">
        /// не обработанная строка подключения к БД</param>
        /// <returns>обработанная строка подключения к БД</returns>
        public static string FixConnectionString(string connectionString)
        {
            // добавление ";" перед концом строки
            connectionString = connectionString.Replace("\n", ";\n");
            // удаление ";" если есть лишние
            connectionString = connectionString.Replace(";;", ";");
            return connectionString;
        }

        /// <summary>
        /// получение строки подключения к БД
        /// </summary>
        /// <param name="filePath">путь к файлу конфигурации</param>
        /// <returns>connectionString - строка подключения к БД</returns>
        public static string GetConnectionString(string filePath)
        {
            // чтение строки подключения к БД
            // из файла конфигурации (в виде простого текстового файла)
            var connectionString = ReadFile(filePath);
            // приведение строки к правильному виду
            connectionString = FixConnectionString(connectionString);
            return connectionString;
        }
    }
}
