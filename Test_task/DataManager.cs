using System;
using System.Data;

namespace Test_task
{
    /// <summary>
    /// класс DataManager - отвечает за получение данных из БД, 
    /// а также добавление и изменение этих данных 
    /// </summary>
    class DataManager
    {
        /// <summary>
        /// форма, на которой находится таблица для вывода записей
        /// </summary>
        private Form1 form;
        /// <summary>
        /// объект класса DataViewer для обращения к нему с целью вывода информации
        /// </summary>
        private DataViewer dataViewer;
        /// <summary>
        /// объект класса UIManager для обращения к нему 
        /// с целью изменения свойств элементов интерфейса
        /// </summary>
        private UIManager uiManager;

        /// <summary>
        /// конструктор класса DataManager, создание объектов классов, с которыми он взаимодействует
        /// </summary>
        /// <param name="form">форма, на которой находится таблица для вывода записей</param>
        public DataManager(Form1 form)
        {
            this.form = form;
            dataViewer = new DataViewer(form);
            uiManager = new UIManager(form);
        }

        /// <summary>
        /// вывод количества записей из таблицы в БД
        /// </summary>
        /// <param name="table">таблица, кол-во записей в которой требуется посчитать</param>
        /// <param name="column">столбец, по которому производится поиск</param>
        /// <param name="value">значение, по которому производится поиск</param>
        /// <returns>количество записей с требуемым значением в требуемом столбце</returns>
        public static int Count(string table, string column, string value)
        {
            string fixedValue = value.Replace("'", "");
            try
            {
                DataTable dataTable = DB.SendQuery("SELECT * FROM " + table + 
                    " WHERE " + column + " = '" + fixedValue + "';");
                return dataTable.Rows.Count;
            }
            catch (Exception e)
            {
                Dialog.Message("Не удалось получить данные из базы данных\n" + e.Message);
                return -1;
            }
        }

        /// <summary>
        /// добавление записи в БД
        /// </summary>
        /// <param name="table">таблица, в которую добавляется запись</param>
        /// <param name="column">столбец, в который записывается значение</param>
        /// <param name="value">значение, которое записывается в столбец в таблице</param>
        public static void Insert(string table, string column, string value)
        {
            string fixedValue = value.Replace("'", "");
            try
            {
                DB.SendQuery("INSERT INTO " + table + " (" + column + ") VALUES ('" + fixedValue + "');");
            }
            catch (Exception e)
            {
                Dialog.Message("Не удалось добавить запись в базу данных\n" + e.Message);
            }
        }

        /// <summary>
        /// получение ID записи в таблице в БД
        /// </summary>
        /// <param name="table">таблица, в которой производится поиск записи</param>
        /// <param name="column">столбец, в котором производится поиск требуемого значения</param>
        /// <param name="value">значение, по которому производится поиск</param>
        /// <returns>ID записи с найденным значением</returns>
        public static int GetElementId(string table, string column, string value)
        {
            string fixedValue = value.Replace("'", "");
            try
            {
                DataTable dataTable = DB.SendQuery("SELECT * FROM " + table + 
                    " WHERE " + column + " = '" + fixedValue + "';");
                return Convert.ToInt32(dataTable.Rows[0].ItemArray[0]);
            }
            catch (Exception e)
            {
                Dialog.Message("Не удалось получить данные из базы данных\n" + e.Message);
                return -1;
            }
        }

        /// <summary>
        /// добавление записи о стране в таблицу стран
        /// </summary>
        /// <param name="country">страна, запись о которой добавляется в таблицу</param>
        /// <param name="city">ID столицы страны (из таблицы городов)</param>
        /// <param name="region">ID региона, в котором находится страна (из таблицы регионов)</param>
        public static void InsertCountry(Country country, int city, int region)
        {
            try
            {
                DB.SendQuery("INSERT INTO dbo.Countries " +
                    "(CountryName, CountryCode, CountryCapital, CountryArea, CountryPopulation, CountryRegion) " +
                    "VALUES ('" + country.name.Replace("'", "") + "', '" + country.alpha3Code + "', " + city + ", " +
                    ((country.area >= 0) ? "CONVERT(FLOAT, REPLACE('" + country.area + "', ',', '.'))" : "NULL") +
                    ", " + country.population + ", " + region + ");");
            }
            catch (Exception e)
            {
                Dialog.Message("Не удалось добавить запись в базу данных\n" + e.Message);
            }
        }

        /// <summary>
        /// обновление записи о стране в таблице стран
        /// </summary>
        /// <param name="country">страна, запись о которой обновляется в таблице</param>
        /// <param name="city">ID столицы страны (из таблицы городов)</param>
        /// <param name="region">ID региона, в котором находится страна (из таблицы регионов)</param>
        public static void UpdateCountry(Country country, int city, int region)
        {
            try
            {
                DB.SendQuery("UPDATE dbo.Countries SET CountryName = '" + country.name.Replace("'", "") +
                    "', CountryCode = '" + country.alpha3Code + "', CountryCapital = " + city + ", CountryArea = " +
                    ((country.area >= 0) ? "CONVERT(FLOAT, REPLACE('" + country.area + "', ',', '.'))" : "NULL") +
                    ", CountryPopulation = " + country.population + ", " + "CountryRegion = " + region +
                    " WHERE CountryCode = '" + country.alpha3Code + "';");
            }
            catch (Exception e)
            {
                Dialog.Message("Не удалось обновить запись в базе данных\n" + e.Message);
            }
        }

        /// <summary>
        /// вывод данных в отсортированном или неотсортированном виде
        /// </summary>
        /// <param name="column_">столбец, по которому производится сортировка</param>
        /// <param name="order_">порядок сортировки (по возрастанию/по убыванию)</param>
        /// <param name="sort_">производится ли сортировка (true/false)</param>
        public void SortDataBy(string column_, string order_, bool sort_ = true)
        {
            dataViewer.ClearColumns();
            try
            {
                DataTable AllCountriesData = DB.SendQuery("SELECT " +
                    "cnt.CountryName AS 'Название страны', cnt.CountryCode AS 'Код', ct.CityName AS 'Столица', " +
                    "cnt.CountryArea AS 'Площадь', cnt.CountryPopulation AS 'Население', r.RegionName AS 'Регион' " +
                    "FROM dbo.Countries cnt INNER JOIN dbo.Cities ct ON cnt.CountryCapital = ct.CityId " +
                    "INNER JOIN dbo.Regions r ON cnt.CountryRegion = r.RegionId" +
                    ((sort_ == true) ? (" ORDER BY " + column_ + " " + order_) : "") + ";");
                if (AllCountriesData.Rows.Count != 0)
                {
                    dataViewer.ShowTable(AllCountriesData);
                }
                else
                {
                    Dialog.Message("В базе данных нет ни одной записи.");
                }
            }
            catch (Exception e)
            {
                Dialog.Message("Не удалось получить данные из базы данных\n" + e.Message);
            }
        }

        /// <summary>
        /// вывод данных в неотсортированном виде
        /// </summary>
        public void GetUnsortedData()
        {
            SortDataBy("", "", false);
        }

        /// <summary>
        /// вывод данных в отсортированном виде
        /// </summary>
        public void GetSortedData()
        {
            switch (form.sortComboBox.SelectedItem)
            {
                case "По названию страны":
                    if (form.ascRadioButton.Checked == false && form.descRadioButton.Checked == false)
                    {
                        uiManager.CheckRadioButton(form.ascRadioButton);
                    }
                    SortDataBy("cnt.CountryName", form.ascRadioButton.Checked ? "ASC" : "DESC");
                    break;
                case "По коду страны":
                    if (form.ascRadioButton.Checked == false && form.descRadioButton.Checked == false)
                    {
                        uiManager.CheckRadioButton(form.ascRadioButton);
                    }
                    SortDataBy("cnt.CountryCode", form.ascRadioButton.Checked ? "ASC" : "DESC");
                    break;
                case "По названию столицы":
                    if (form.ascRadioButton.Checked == false && form.descRadioButton.Checked == false)
                    {
                        uiManager.CheckRadioButton(form.ascRadioButton);
                    }
                    SortDataBy("ct.CityName", form.ascRadioButton.Checked ? "ASC" : "DESC");
                    break;
                case "По площади страны":
                    if (form.ascRadioButton.Checked == false && form.descRadioButton.Checked == false)
                    {
                        uiManager.CheckRadioButton(form.descRadioButton);
                    }
                    SortDataBy("cnt.CountryArea", form.ascRadioButton.Checked ? "ASC" : "DESC");
                    break;
                case "По населению страны":
                    if (form.ascRadioButton.Checked == false && form.descRadioButton.Checked == false)
                    {
                        uiManager.CheckRadioButton(form.descRadioButton);
                    }
                    SortDataBy("cnt.CountryPopulation", form.ascRadioButton.Checked ? "ASC" : "DESC");
                    break;
                case "По названию региона":
                    if (form.ascRadioButton.Checked == false && form.descRadioButton.Checked == false)
                    {
                        uiManager.CheckRadioButton(form.ascRadioButton);
                    }
                    SortDataBy("r.RegionName", form.ascRadioButton.Checked ? "ASC" : "DESC");
                    break;
                default:
                    throw new InvalidOperationException("Unexpected item " + form.sortComboBox.SelectedItem);
            }
        }
    }
}
