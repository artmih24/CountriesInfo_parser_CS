using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_task
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// объект класса DataViewer для обращения к нему с целью вывода информации
        /// </summary>
        DataViewer dataviewer;
        /// <summary>
        /// объект класса UIManager для обращения к нему с целью изменения свойств элементов интерфейса
        /// </summary>
        UIManager UImanager;
        /// <summary>
        /// объект класса DataManager для обращения к нему с целью управления данными в БД
        /// </summary>
        DataManager datamanager;

        /// <summary>
        /// инициализация формы и создание объектов классов
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            dataviewer = new DataViewer(this);
            datamanager = new DataManager(this);
            UImanager = new UIManager(this);
        }

        /// <summary>
        /// поиск по стране
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, EventArgs e)
        {
            UImanager.ClearSortComboBox();
            if (CountryTextBox.Text != "")
            {
                try
                {
                    // получение данные о стране
                    Country country = DataReceiver.GetInfo(CountryTextBox.Text);

                    // вывод полученных данных в таблицу
                    dataviewer.Show(country);

                    // предложение пользователю сохранить данные в базе данных
                    DialogResult dialogresult = Dialog.AskSaveOrNot();
                    // если пользователь хочет сохранить данные в базе данных
                    if (dialogresult == DialogResult.Yes)
                    {
                        try
                        {
                            int CityId, RegionId;
                            // проверка наличия города в таблице городов
                            int Cities = DataManager.GetCount("dbo.Cities", "CityName", country.capital);
                            if (Cities != 0)
                                // если город есть в таблице городов, то просто берем его ID
                                CityId = DataManager.GetElementId("dbo.Cities", "CityName", country.capital);
                            else
                            {
                                // если города нет в таблице городов, то добавляем, и затем берем его ID
                                DataManager.Insert("dbo.Cities", "CityName", country.capital);
                                CityId = DataManager.GetElementId("dbo.Cities", "CityName", country.capital);
                            }
                            // проверка наличия региона в таблице регионов
                            int Regions = DataManager.GetCount("dbo.Regions", "RegionName", country.region);
                            if (Regions != 0)
                                // если регион есть в таблице регионов, то просто берем его ID
                                RegionId = DataManager.GetElementId("dbo.Regions", "RegionName", country.region);
                            else
                            {
                                // если региона нет в таблице регионов, то добавляем, и затем берем его ID
                                DataManager.Insert("dbo.Regions", "RegionName", country.region);
                                RegionId = DataManager.GetElementId("dbo.Regions", "RegionName", country.region);
                            }
                            // проверка наличия страны в таблице стран
                            int Countries = DataManager.GetCount("dbo.Countries", "CountryCode", country.alpha3Code);
                            if (Countries == 0)
                            {
                                // если страны нет в таблице стран, то добавляем ее в таблицу
                                DataManager.InsertCountry(country, CityId, RegionId);
                                Dialog.Message("Запись успешно добавлена");
                            }
                            else
                            {
                                // если страна есть в таблице стран, обновляем данные о ней
                                DataManager.UpdateCountry(country, CityId, RegionId);
                                Dialog.Message("Запись успешно обновлена");
                            }
                        }
                        catch (Exception ex)
                        {
                            Dialog.Message("Возникла ошибка:\n" + ex.Message);
                        }
                    }
                }
                catch (WebException webexception)
                {
                    WebExceptionStatus status = webexception.Status;
                    if (status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse httpresponse = (HttpWebResponse)webexception.Response;
                        Dialog.Message("Не удалось получить данные.\nОшибка " + (int)httpresponse.StatusCode + " - " + httpresponse.StatusCode);
                    }
                    UImanager.ClearCountryTextBox();
                }
                catch (Exception ex)
                {
                    Dialog.Message("Возникла ошибка:\n" + ex.Message);
                    UImanager.ClearCountryTextBox();
                }
            }
            else
                Dialog.Message("Строка поиска страны пустая.");
        }

        /// <summary>
        /// вывод всех записей из базы данных в порядке добавления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataBaseButton_Click(object sender, EventArgs e)
        {
            UImanager.SetSortComboBoxText("В порядке добавления");
            datamanager.GetUnsortedData();
        }

        /// <summary>
        /// вывод всех записей из базы данных с указанной сортировкой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortComboBox_SelectedIndexChanged_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            datamanager.GetSortedData();
        }
    }

    /// <summary>
    /// класс Country (страна) - для десериализации данных, полученных в формате JSON
    /// </summary>
    class Country
    {
        /// <summary>
        /// название страны
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// трехбуквенный код страны
        /// </summary>
        public string alpha3Code { get; set; }
        /// <summary>
        /// название столицы страны
        /// </summary>
        public string capital { get; set; }
        /// <summary>
        /// площадь территории страны [км^2]
        /// </summary>
        public double area { get; set; }
        /// <summary>
        /// население страны [чел]
        /// </summary>
        public long population { get; set; }
        /// <summary>
        /// регион страны (Европа, Азия, Америка и т.д.)
        /// </summary>
        public string region { get; set; }
    }

    /// <summary>
    /// класс DB - отвечает за взаимодействие с базой данных
    /// </summary>
    class DB
    {
        /// <summary>
        /// строка подключения к СУБД, считывается из файла конфигурации
        /// </summary>
        private static String ConnectionString;// = "Data Source=ARTEM-PC;Initial Catalog=Countries_DB;Integrated Security=True";

        /// <summary>
        /// отправление запроса в СУБД 
        /// </summary>
        /// <param name="query">строка запроса</param>
        /// <returns>DataTable - таблица данных</returns>
        public static DataTable SendQuery(string query)
        {
            // чтение XML-файла
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load("connection.config");
            // считывание строки подключения
            foreach (XmlNode node in xmldoc.DocumentElement)
            {
                ConnectionString = node.Attributes[2].Value;
            }
            // инициализация объекта класса SqlConnection для подключения к БД
            SqlConnection connection = new SqlConnection(ConnectionString);
            DataTable result = new DataTable();
            try
            {
                // отправка запроса в СУБД
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(result);
            }
            catch (Exception ex)
            {
                Dialog.Message(ex.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            return result;
        }
    }
 
    /// <summary>
    /// класс Dataviewer - отвечает за вывод информации в таблицу
    /// </summary>
    class DataViewer
    {
        /// <summary>
        /// форма, на которой находится таблица для вывода записей
        /// </summary>
        Form1 form;

        /// <summary>
        /// конструктор класса DataViewer
        /// </summary>
        /// <param name="form_">форма, на которой находится таблица для вывода записей</param>
        public DataViewer(Form1 form_)
        {
            form = form_;
        }

        /// <summary>
        /// изменение ширины столбца
        /// </summary>
        /// <param name="column_">номер столбца, ширину которого требуется изменить, начинается с 0</param>
        /// <param name="width_">ширина столбца, которую требуется установить</param>
        public void SetColumnWidth(int column_, int width_)
        {
            DataGridViewColumn column = form.dataGridView1.Columns[column_];
            column.Width = width_;
        }

        /// <summary>
        /// изменение ширины столбцов
        /// </summary>
        public void SetColumnsWidth()
        {
            SetColumnWidth(0, 250); // для названия страны - пошире
            SetColumnWidth(1, 40); // для кода страны можно сделать узкий столбец
            SetColumnWidth(2, 140); // для названия столицы страны тоже можно сделать столбец пошире
            SetColumnWidth(5, 85); // для региона страны можно сделать столбец поуже
        }

        /// <summary>
        /// удаление столбцов
        /// </summary>
        public void ClearColumns()
        {
            form.dataGridView1.Columns.Clear();
        }

        /// <summary>
        /// удаление столбцов и лишних строк
        /// </summary>
        public void ClearAllCells()
        {
            form.dataGridView1.DataSource = null;
            form.dataGridView1.RowCount = 1;
            form.dataGridView1.Columns.Clear();
        }

        /// <summary>
        /// добавление столбцов в DataGridView
        /// </summary>
        public void CreateColumns()
        {
            form.dataGridView1.Columns.Add("name", "Название страны");
            form.dataGridView1.Columns.Add("alpha3Code", "Код");
            form.dataGridView1.Columns.Add("capital", "Столица");
            form.dataGridView1.Columns.Add("area", "Площадь");
            form.dataGridView1.Columns.Add("population", "Население");
            form.dataGridView1.Columns.Add("region", "Регион");
            SetColumnsWidth();
        }

        /// <summary>
        /// вывод информации о стране в datagridview
        /// </summary>
        /// <param name="country_">информация о стране</param>
        public void Show(Country country_)
        {
            // если в datagridview есть столбцы - удаление столбцов и лишних строк
            if (form.dataGridView1.Columns.Count != 0)
                ClearAllCells();
            CreateColumns();
            // заполнение ячеек таблицы полученными данными
            form.dataGridView1.Rows[0].Cells["name"].Value = country_.name;
            form.dataGridView1.Rows[0].Cells["alpha3Code"].Value = country_.alpha3Code;
            form.dataGridView1.Rows[0].Cells["capital"].Value = country_.capital;
            form.dataGridView1.Rows[0].Cells["area"].Value = country_.area;
            form.dataGridView1.Rows[0].Cells["population"].Value = country_.population;
            form.dataGridView1.Rows[0].Cells["region"].Value = country_.region;
        }

        /// <summary>
        /// вывод информации о всех странах в базе данных
        /// </summary>
        /// <param name="datatable_">данные из БД</param>
        public void ShowTable(DataTable datatable_)
        {
            form.dataGridView1.DataSource = datatable_;
            SetColumnsWidth();
        }
    }

    /// <summary>
    /// класс Dialog - отвечает за вывод диалоговых окон
    /// </summary>
    class Dialog
    {
        /// <summary>
        /// вывод простого MessageBox
        /// </summary>
        /// <param name="message_">текст, который будет отображен в MessageBox</param>
        public static void Message(string message_) 
        {
            MessageBox.Show(message_);
        }

        /// <summary>
        /// вывод MessageBox с кнопками Да/Нет с целью предложения сохранить полученные данные в БД
        /// </summary>
        /// <returns>DialogResult - какая кнопка была нажата</returns>
        public static DialogResult AskSaveOrNot()
        {
            return MessageBox.Show("Сохранить данные в базу данных?", "Сохранение", MessageBoxButtons.YesNo);
        }
    }

    /// <summary>
    /// класс DataManager - отвечает за получение данных из БД, а также добавление и изменение этих данных 
    /// </summary>
    class DataManager
    {
        /// <summary>
        /// форма, на которой находится таблица для вывода записей
        /// </summary>
        Form1 form;
        /// <summary>
        /// объект класса DataViewer для обращения к нему с целью вывода информации
        /// </summary>
        DataViewer dataviewer;
        /// <summary>
        /// объект класса UIManager для обращения к нему с целью изменения свойств элементов интерфейса
        /// </summary>
        UIManager UImanager;

        /// <summary>
        /// конструктор класса DataManager, создание объектов классов, с которыми он взаимодействует
        /// </summary>
        /// <param name="form_">форма, на которой находится таблица для вывода записей</param>
        public DataManager(Form1 form_)
        {
            form = form_;
            dataviewer = new DataViewer(form_);
            UImanager = new UIManager(form_);
        }

        /// <summary>
        /// вывод количества записей из таблицы в БД
        /// </summary>
        /// <param name="table_">таблица, кол-во записей в которой требуется посчитать</param>
        /// <param name="column_">столбец, по которому производится поиск</param>
        /// <param name="value_">значение, по которому производится поиск</param>
        /// <returns>количество записей с требуемым значением в требуемом столбце</returns>
        public static int GetCount(string table_, string column_, string value_)
        {
            DataTable Data = DB.SendQuery("SELECT * FROM " + table_ + " WHERE " + column_ + " = '" + value_ + "';");
            return Data.Rows.Count;
        }

        /// <summary>
        /// добавление записи в БД
        /// </summary>
        /// <param name="table_">таблица, в которую добавляется запись</param>
        /// <param name="column_">столбец, в который записывается значение</param>
        /// <param name="value_">значение, которое записывается в столбец в таблице</param>
        public static void Insert(string table_, string column_, string value_)
        {
            DB.SendQuery("INSERT INTO " + table_ + " (" + column_ + ") VALUES ('" + value_ + "');");
        }

        /// <summary>
        /// получение ID записи в таблице в БД
        /// </summary>
        /// <param name="table_">таблица, в которой производится поиск записи</param>
        /// <param name="column_">столбец, в котором производится поиск требуемого значения</param>
        /// <param name="value_">значение, по которому производится поиск</param>
        /// <returns>ID записи с найденным значением</returns>
        public static int GetElementId(string table_, string column_, string value_)
        {
            DataTable Data = DB.SendQuery("SELECT * FROM " + table_ + " WHERE " + column_ + " = '" + value_ + "';");
            return Convert.ToInt32(Data.Rows[0].ItemArray[0]);
        }

        /// <summary>
        /// добавление записи о стране в таблицу стран
        /// </summary>
        /// <param name="country_">страна, запись о которой добавляется в таблицу</param>
        /// <param name="city_">ID столицы страны (из таблицы городов)</param>
        /// <param name="region_">ID региона, в котором находится страна (из таблицы регионов)</param>
        public static void InsertCountry(Country country_, int city_, int region_)
        {
            DB.SendQuery("INSERT INTO dbo.Countries (CountryName, CountryCode, CountryCapital, CountryArea, CountryPopulation, CountryRegion) " +
                "VALUES ('" + country_.name + "', '" + country_.alpha3Code + "', " + city_ + ", CONVERT(FLOAT, REPLACE('" + country_.area + "', ',', '.')), " +
                country_.population + ", " + region_ + ");");
        }

        /// <summary>
        /// обновление записи о стране в таблице стран
        /// </summary>
        /// <param name="country_">страна, запись о которой обновляется в таблице</param>
        /// <param name="city_">ID столицы страны (из таблицы городов)</param>
        /// <param name="region_">ID региона, в котором находится страна (из таблицы регионов)</param>
        public static void UpdateCountry(Country country_, int city_, int region_)
        {
            DB.SendQuery("UPDATE dbo.Countries SET CountryName = '" + country_.name + "', CountryCode = '" + country_.alpha3Code + 
                "', CountryCapital = " + city_ + ", " + "CountryArea = CONVERT(FLOAT, REPLACE('" + country_.area + "', ',', '.')), CountryPopulation = " + 
                country_.population + ", " + "CountryRegion = " + region_ + " WHERE CountryCode = '" + country_.alpha3Code + "';");
        }

        /// <summary>
        /// вывод данных в отсортированном или неотсортированном виде
        /// </summary>
        /// <param name="column_">столбец, по которому производится сортировка</param>
        /// <param name="order_">порядок сортировки (по возрастанию/по убыванию)</param>
        /// <param name="sort_">производится ли сортировка (true/false)</param>
        public void SortDataBy(string column_, string order_, bool sort_ = true)
        {
            dataviewer.ClearColumns();
            DataTable AllCountriesData = DB.SendQuery("SELECT cnt.CountryName AS 'Название страны', cnt.CountryCode AS 'Код', ct.CityName AS 'Столица', " +
                "cnt.CountryArea AS 'Площадь', cnt.CountryPopulation AS 'Население', r.RegionName AS 'Регион' " +
                "FROM dbo.Countries cnt INNER JOIN dbo.Cities ct ON cnt.CountryCapital = ct.CityId INNER JOIN dbo.Regions r ON cnt.CountryRegion = r.RegionId" +
                ((sort_ == true) ? (" ORDER BY " + column_ + " " + order_) : "") + ";");
            if (AllCountriesData.Rows.Count != 0)
                dataviewer.ShowTable(AllCountriesData);
            else
                Dialog.Message("В базе данных нет ни одной записи.");
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
            switch (form.SortComboBox.SelectedItem)
            {
                case "По названию страны":
                    if (form.AscRadioButton.Checked == false && form.DescRadioButton.Checked == false)
                        UImanager.CheckRadioButton(form.AscRadioButton);
                    SortDataBy("cnt.CountryName", form.AscRadioButton.Checked ? "ASC" : "DESC");
                    break;
                case "По коду страны":
                    if (form.AscRadioButton.Checked == false && form.DescRadioButton.Checked == false)
                        UImanager.CheckRadioButton(form.AscRadioButton);
                    SortDataBy("cnt.CountryCode", form.AscRadioButton.Checked ? "ASC" : "DESC");
                    break;
                case "По названию столицы":
                    if (form.AscRadioButton.Checked == false && form.DescRadioButton.Checked == false)
                        UImanager.CheckRadioButton(form.AscRadioButton);
                    SortDataBy("ct.CityName", form.AscRadioButton.Checked ? "ASC" : "DESC");
                    break;
                case "По площади страны":
                    if (form.AscRadioButton.Checked == false && form.DescRadioButton.Checked == false)
                        UImanager.CheckRadioButton(form.DescRadioButton);
                    SortDataBy("cnt.CountryArea", form.AscRadioButton.Checked ? "ASC" : "DESC");
                    break;
                case "По населению страны":
                    if (form.AscRadioButton.Checked == false && form.DescRadioButton.Checked == false)
                        UImanager.CheckRadioButton(form.DescRadioButton);
                    SortDataBy("cnt.CountryPopulation", form.AscRadioButton.Checked ? "ASC" : "DESC");
                    break;
                case "По названию региона":
                    if (form.AscRadioButton.Checked == false && form.DescRadioButton.Checked == false)
                        UImanager.CheckRadioButton(form.AscRadioButton);
                    SortDataBy("r.RegionName", form.AscRadioButton.Checked ? "ASC" : "DESC");
                    break;
            }
        }
    }

    /// <summary>
    /// класс UIManager - класс, отвечающий за изменения свойств прочих элементов графического интерфейса 
    /// </summary>
    class UIManager
    {
        /// <summary>
        /// форма, на которой находится таблица для вывода записей
        /// </summary>
        Form1 form;

        /// <summary>
        /// конструктор класса UIManager
        /// </summary>
        /// <param name="form_">форма, на которой находится таблица для вывода записей</param>
        public UIManager(Form1 form_)
        {
            form = form_;
        }

        /// <summary>
        /// очистка строки поиска страны
        /// </summary>
        public void ClearCountryTextBox()
        {
            form.CountryTextBox.Text = "";
        }

        /// <summary>
        /// очистка текста в ComboBox для сортировки
        /// </summary>
        public void ClearSortComboBox()
        {
            form.SortComboBox.Text = "";
        }

        /// <summary>
        /// установка текста в ComboBox для сортировки
        /// </summary>
        /// <param name="text_">текст сообщения, выводимый в MessageBox</param>
        public void SetSortComboBoxText(string text_)
        {
            form.SortComboBox.Text = text_;
        }

        /// <summary>
        /// установка RadioButton в состояние Checked
        /// </summary>
        /// <param name="rb_">RadioButton, который требуется возвести в Checked</param>
        public void CheckRadioButton(RadioButton rb_)
        {
            rb_.Checked = true;
        }
    }

    /// <summary>
    /// класс DataReceiver - класс, отвечающий за получение данных 
    /// </summary>
    class DataReceiver
    {
        /// <summary>
        /// получение информации о стране
        /// </summary>
        /// <param name="country_">название страны, информацию о которой требуется найти</param>
        /// <returns>данные о стране</returns>
        public static Country GetInfo(string country_)
        {
            WebClient webclient = new WebClient();
            // получение данных
            var JSON_String = webclient.DownloadString("https://restcountries.eu/rest/v2/name/" + country_);
            // преобразование данных из JSON в тип данных Country
            List<Country> countries = JsonSerializer.Deserialize<List<Country>>(JSON_String);
            // поскольку возвращается список, содержащий данные об одной стране, возвращаем первую (нулевую) запись
            return countries[0];
        }
    }
}
