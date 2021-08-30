using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace Test_task
{
    /// <summary>
    /// класс DataReceiver - класс, отвечающий за получение данных 
    /// </summary>
    class DataReceiver
    {
        /// <summary>
        /// получение информации о стране
        /// </summary>
        /// <param name="country">название страны, информацию о которой требуется найти</param>
        /// <returns>список данных о странах - 
        /// название, код, столица, площадь, население, регион 
        /// найденных стран</returns>
        public static List<Country> GetInfo(string country)
        {
            var webClient = new WebClient();
            try
            {
                // получение данных
                var jsonString = webClient.DownloadString("https://restcountries.eu/rest/v2/name/" + country);
                // замена null на -1 для успешной десериализации JSON,
                // далее в datagridview и в БД будет записано значение null
                jsonString = jsonString.Replace("\"area\":null", "\"area\":-1");
                // замена буквы "S с запятой" ('ș'), которую "не понимает" SQL-сервер
                jsonString = jsonString.Replace("ș", "s");
                try
                {
                    // преобразование данных из JSON в тип данных Country
                    List<Country> countries = JsonSerializer.Deserialize<List<Country>>(jsonString);
                    return countries;
                }
                catch (Exception e)
                {
                    Dialog.Message("Возникла ошибка при десериализации JSON\n" + e.Message);
                    return null;
                }
            }
            catch (WebException webException)
            {
                var status = webException.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    var httpResponse = (HttpWebResponse)webException.Response;
                    Dialog.Message("Возникла ошибка при получении данных " +
                        (int)httpResponse.StatusCode + " - " + httpResponse.StatusCode);
                }
                return null;
            }
            catch (Exception e)
            {
                Dialog.Message("Возникла ошибка при получении данных\n" + e.Message);
                return null;
            }
        }
    }
}
