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
        DataViewer dataviewer;
        DataManager datamanager;
        UIManager UImanager;

        public Form1()
        {
            InitializeComponent();
            dataviewer = new DataViewer(this);
            datamanager = new DataManager(this);
            UImanager = new UIManager(this);
        }

        // поиск по стране
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

        private void DataBaseButton_Click(object sender, EventArgs e)
        {
            UImanager.SetSortComboBoxText("В порядке добавления");
            datamanager.GetUnsortedData();
        }

        private void SortComboBox_SelectedIndexChanged_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            datamanager.GetSortedData();
        }
    }

    // класс Country - для десериализации данных, полученных в формате JSON
    class Country
    {
        public string name { get; set; }
        public string alpha3Code { get; set; }
        public string capital { get; set; }
        public double area { get; set; }
        public long population { get; set; }
        public string region { get; set; }
    }

    // класс DB - отвечает за взаимодействие с базой данных
    class DB
    {
        private static String ConnectionString;// = "Data Source=ARTEM-PC;Initial Catalog=Countries_DB;Integrated Security=True";


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

    // класс Dataviewer - отвечает за вывод информации в таблицу
    class DataViewer
    {
        Form1 form;

        public DataViewer(Form1 form_)
        {
            form = form_;
        }

        public void SetColumnWidth(int column_, int width_)
        {
            // изменение ширины столбца
            DataGridViewColumn column = form.dataGridView1.Columns[column_];
            column.Width = width_;
        }

        public void SetColumnsWidth()
        {
            // изменение ширины столбцов
            SetColumnWidth(0, 250); // для названия страны - пошире
            SetColumnWidth(1, 40); // для кода страны можно сделать узкий столбец
            SetColumnWidth(2, 140); // для названия столицы страны тоже можно сделать столбец пошире
            SetColumnWidth(5, 85); // для региона страны можно сделать столбец поуже
        }

        public void ClearColumns()
        {
            form.dataGridView1.Columns.Clear();
        }

        public void ClearAllCells()
        {
            // удаление столбцов и лишних строк
            form.dataGridView1.DataSource = null;
            form.dataGridView1.RowCount = 1;
            form.dataGridView1.Columns.Clear();
        }

        public void CreateColumns()
        {
            // добавление столбцов в таблицу
            form.dataGridView1.Columns.Add("name", "Название страны");
            form.dataGridView1.Columns.Add("alpha3Code", "Код");
            form.dataGridView1.Columns.Add("capital", "Столица");
            form.dataGridView1.Columns.Add("area", "Площадь");
            form.dataGridView1.Columns.Add("population", "Население");
            form.dataGridView1.Columns.Add("region", "Регион");
            SetColumnsWidth();
        }

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

        public void ShowTable(DataTable datatable_)
        {
            form.dataGridView1.DataSource = datatable_;
            SetColumnsWidth();
        }
    }

    // класс Dialog - отвечает за вывод диалоговых окон
    class Dialog
    {
        public static void Message(string message_) 
        {
            MessageBox.Show(message_);
        }

        public static DialogResult AskSaveOrNot()
        {
            return MessageBox.Show("Сохранить данные в базу данных?", "Сохранение", MessageBoxButtons.YesNo);
        }
    }

    // класс DataManager - отвечает за получение данных из БД
    class DataManager
    {
        Form1 form;
        DataViewer dataviewer;
        UIManager UImanager;

        public DataManager(Form1 form_)
        {
            form = form_;
            dataviewer = new DataViewer(form_);
            UImanager = new UIManager(form_);
        }

        public static int GetCount(string table_, string column_, string value_)
        {
            DataTable Data = DB.SendQuery("SELECT * FROM " + table_ + " WHERE " + column_ + " = '" + value_ + "';");
            return Data.Rows.Count;
        }

        public static void Insert(string table_, string column_, string value_)
        {
            DB.SendQuery("INSERT INTO " + table_ + " (" + column_ + ") VALUES ('" + value_ + "');");
        }

        public static int GetElementId(string table_, string column_, string value_)
        {
            DataTable Data = DB.SendQuery("SELECT * FROM " + table_ + " WHERE " + column_ + " = '" + value_ + "';");
            return Convert.ToInt32(Data.Rows[0].ItemArray[0]);
        }

        public static void InsertCountry(Country country_, int city_, int region_)
        {
            DB.SendQuery("INSERT INTO dbo.Countries (CountryName, CountryCode, CountryCapital, CountryArea, CountryPopulation, CountryRegion) " +
                "VALUES ('" + country_.name + "', '" + country_.alpha3Code + "', " + city_ + ", CONVERT(FLOAT, REPLACE('" + country_.area + "', ',', '.')), " +
                country_.population + ", " + region_ + ");");
        }

        public static void UpdateCountry(Country country_, int city_, int region_)
        {
            DB.SendQuery("UPDATE dbo.Countries SET CountryName = '" + country_.name + "', CountryCode = '" + country_.alpha3Code + 
                "', CountryCapital = " + city_ + ", " + "CountryArea = CONVERT(FLOAT, REPLACE('" + country_.area + "', ',', '.')), CountryPopulation = " + 
                country_.population + ", " + "CountryRegion = " + region_ + " WHERE CountryCode = '" + country_.alpha3Code + "';");
        }

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

        public void GetUnsortedData()
        {
            SortDataBy("", "", false);
        }

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

    // класс UIManager - класс, отвечающий за изменения свойств прочих элементов графического интерфейса
    class UIManager
    {
        Form1 form;

        public UIManager(Form1 form_)
        {
            form = form_;
        }

        public void ClearCountryTextBox()
        {
            form.CountryTextBox.Text = "";
        }

        public void ClearSortComboBox()
        {
            form.SortComboBox.Text = "";
        }

        public void SetSortComboBoxText(string text_)
        {
            form.SortComboBox.Text = text_;
        }

        public void CheckRadioButton(RadioButton rb_)
        {
            rb_.Checked = true;
        }
    }

    // класс DataReceiver - класс, отвечающий за получение данных
    class DataReceiver
    {
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
