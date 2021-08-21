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
        public Form1()
        {
            InitializeComponent();
        }

        class Country
        {
            public string name { get; set; }
            public string alpha3Code { get; set; }
            public string capital { get; set; }
            public double area { get; set; }
            public long population { get; set; }
            public string region { get; set; }
        }

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
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
                return result;
            }
        }

        void SetColumnsWidth()
        {
            // изменение ширины столбцов
            DataGridViewColumn namecolumn = dataGridView1.Columns[0];
            namecolumn.Width = 250; // для названия страны - пошире
            DataGridViewColumn codecolumn = dataGridView1.Columns[1];
            codecolumn.Width = 40; // для кода страны можно сделать узкий столбец
            DataGridViewColumn capitalcolumn = dataGridView1.Columns[2];
            capitalcolumn.Width = 140; // для названия столицы страны тоже можно сделать столбец пошире
            DataGridViewColumn regioncolumn = dataGridView1.Columns[5];
            regioncolumn.Width = 85; // для региона страны можно сделать столбец поуже
        }

        // поиск по стране
        private void SearchButton_Click(object sender, EventArgs e)
        {
            SortComboBox.Text = "";
            if (CountryTextBox.Text != "")
            {
                WebClient webclient = new WebClient();
                try
                {
                    // получение данных
                    var JSON_String = webclient.DownloadString("https://restcountries.eu/rest/v2/name/" + CountryTextBox.Text);

                    // преобразование данных из JSON в тип данных Country
                    List<Country> countries = JsonSerializer.Deserialize<List<Country>>(JSON_String);
                    // поскольку возвращается список, содержащий данные об одной стране, берем первую (нулевую) запись
                    Country country = countries[0];

                    // если в datagridview есть столбцы - удаление столбцов и лишних строк
                    if (dataGridView1.Columns.Count != 0)
                    {
                        dataGridView1.DataSource = null;
                        dataGridView1.RowCount = 1;
                        dataGridView1.Columns.Clear();
                    }

                    // добавление столбцов
                    dataGridView1.Columns.Add("name", "Название страны");
                    dataGridView1.Columns.Add("alpha3Code", "Код");
                    dataGridView1.Columns.Add("capital", "Столица");
                    dataGridView1.Columns.Add("area", "Площадь");
                    dataGridView1.Columns.Add("population", "Население");
                    dataGridView1.Columns.Add("region", "Регион");
                    SetColumnsWidth();

                    // заполнение ячеек таблицы полученными данными
                    dataGridView1.Rows[0].Cells["name"].Value = country.name;
                    dataGridView1.Rows[0].Cells["alpha3Code"].Value = country.alpha3Code;
                    dataGridView1.Rows[0].Cells["capital"].Value = country.capital;
                    dataGridView1.Rows[0].Cells["area"].Value = country.area;
                    dataGridView1.Rows[0].Cells["population"].Value = country.population;
                    dataGridView1.Rows[0].Cells["region"].Value = country.region;

                    // предложение пользователю сохранить данные в базе данных
                    DialogResult dialogresult = MessageBox.Show("Сохранить данные в базу данных?", "Сохранение", MessageBoxButtons.YesNo);
                    // если пользователь хочет сохранить данные в базе данных
                    if (dialogresult == DialogResult.Yes)
                    {
                        // проверка наличия столицы в таблице городов
                        DataTable CitiesData = DB.SendQuery("SELECT * FROM dbo.Cities WHERE CityName = '" + country.capital + "';");
                        int CityId = 0;
                        if (CitiesData.Rows.Count != 0)
                            // если город есть в таблице, то берем его ID
                            CityId = Convert.ToInt32(CitiesData.Rows[0].ItemArray[0]);
                        else
                        {
                            // если города нет в таблице, то добавляем его в таблицу
                            DB.SendQuery("INSERT INTO dbo.Cities (CityName) VALUES ('" + country.capital + "');");
                            CitiesData = DB.SendQuery("SELECT * FROM dbo.Cities WHERE CityName = '" + country.capital + "';");
                            CityId = Convert.ToInt32(CitiesData.Rows[0].ItemArray[0]);
                        }
                        // проверка наличия регионов в таблице регионов
                        DataTable RegionsData = DB.SendQuery("SELECT * FROM dbo.Regions WHERE RegionName = '" + country.region + "';");
                        int RegionId = 0;
                        if (RegionsData.Rows.Count != 0)
                            // если регион есть в таблице, берем его номер
                            RegionId = Convert.ToInt32(RegionsData.Rows[0].ItemArray[0]);
                        else
                        {
                            // если региона нет в таблице, то добавляем его в таблицу
                            DB.SendQuery("INSERT INTO dbo.Regions (RegionName) VALUES ('" + country.region + "');");
                            RegionsData = DB.SendQuery("SELECT * FROM dbo.Regions WHERE RegionName = '" + country.region + "';");
                            RegionId = Convert.ToInt32(RegionsData.Rows[0].ItemArray[0]);
                        }
                        // проверка наличия страны в таблице стран
                        DataTable CountriesData = DB.SendQuery("SELECT * FROM dbo.Countries WHERE CountryCode = '" + country.alpha3Code + "';");
                        if (CountriesData.Rows.Count == 0)
                            // если страны нет в таблице - добавляем ее в таблицу
                           DB.SendQuery("INSERT INTO dbo.Countries (CountryName, CountryCode, CountryCapital, CountryArea, CountryPopulation, CountryRegion) " +
                                "VALUES ('" + country.name + "', '" + country.alpha3Code + "', " + CityId + ", CONVERT(FLOAT, REPLACE('" + country.area + "', ',', '.')), " + 
                                country.population + ", " + RegionId + ");");
                        else
                            // если страна есть в таблице - обновляем данные о ней
                            DB.SendQuery("UPDATE dbo.Countries SET CountryName = '" + country.name + "', CountryCapital = " + CityId + ", " +
                                "CountryArea = CONVERT(FLOAT, REPLACE('" + country.area + "', ',', '.')), CountryPopulation = " + country.population + ", " +
                                "CountryRegion = " + RegionId + " WHERE CountryCode = '" + country.alpha3Code + "';");
                    }
                }
                catch (WebException webexception)
                {
                    WebExceptionStatus status = webexception.Status;
                    if (status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse httpresponse = (HttpWebResponse)webexception.Response;
                        MessageBox.Show("Не удалось получить данные.\nОшибка " + (int)httpresponse.StatusCode + " - " + httpresponse.StatusCode);
                    }
                    CountryTextBox.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось получить данные.\nОшибка: " + ex.Message);
                    CountryTextBox.Text = "";
                }
            }
            else
                MessageBox.Show("Строка поиска страны пустая.");
        }

        private void DataBaseButton_Click(object sender, EventArgs e)
        {
            SortComboBox.Text = "В порядке добавления";
            dataGridView1.Columns.Clear();
            DataTable AllCountriesData = DB.SendQuery("SELECT cnt.CountryName AS 'Название страны', cnt.CountryCode AS 'Код', ct.CityName AS 'Столица', " +
                "cnt.CountryArea AS 'Площадь', cnt.CountryPopulation AS 'Население', r.RegionName AS 'Регион' " +
                "FROM dbo.Countries cnt INNER JOIN dbo.Cities ct ON cnt.CountryCapital = ct.CityId INNER JOIN dbo.Regions r ON cnt.CountryRegion = r.RegionId;");
            dataGridView1.DataSource = AllCountriesData;
            SetColumnsWidth();
        }

        void SortDataBy(string column_, string order_) {
            dataGridView1.Columns.Clear();
            DataTable AllCountriesData = DB.SendQuery("SELECT cnt.CountryName AS 'Название страны', cnt.CountryCode AS 'Код', ct.CityName AS 'Столица', " +
                "cnt.CountryArea AS 'Площадь', cnt.CountryPopulation AS 'Население', r.RegionName AS 'Регион' " +
                "FROM dbo.Countries cnt INNER JOIN dbo.Cities ct ON cnt.CountryCapital = ct.CityId INNER JOIN dbo.Regions r ON cnt.CountryRegion = r.RegionId " +
                "ORDER BY " + column_ + " " + order_  + ";");
            dataGridView1.DataSource = AllCountriesData;
            SetColumnsWidth();
        }

        private void SortComboBox_SelectedIndexChanged_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            switch (SortComboBox.SelectedItem)
            {
                case "По названию страны":
                    if (AscRadioButton.Checked == false && DescRadioButton.Checked == false)
                        AscRadioButton.Checked = true;
                    if (AscRadioButton.Checked == true)
                        SortDataBy("cnt.CountryName", "ASC");
                    else
                        SortDataBy("cnt.CountryName", "DESC");
                    break;
                case "По коду страны":
                    if (AscRadioButton.Checked == false && DescRadioButton.Checked == false)
                        AscRadioButton.Checked = true;
                    if (AscRadioButton.Checked == true)
                        SortDataBy("cnt.CountryCode", "ASC");
                    else
                        SortDataBy("cnt.CountryCode", "DESC");
                    break;
                case "По названию столицы":
                    if (AscRadioButton.Checked == false && DescRadioButton.Checked == false)
                        AscRadioButton.Checked = true;
                    if (AscRadioButton.Checked == true)
                        SortDataBy("ct.CityName", "ASC");
                    else
                        SortDataBy("ct.CityName", "DESC");
                    break;
                case "По площади страны":
                    if (AscRadioButton.Checked == false && DescRadioButton.Checked == false)
                        DescRadioButton.Checked = true;
                    if (AscRadioButton.Checked == true)
                        SortDataBy("cnt.CountryArea", "ASC");
                    else
                        SortDataBy("cnt.CountryArea", "DESC");
                    break;
                case "По населению страны":
                    if (AscRadioButton.Checked == false && DescRadioButton.Checked == false)
                        DescRadioButton.Checked = true;
                    if (AscRadioButton.Checked == true)
                        SortDataBy("cnt.CountryPopulation", "ASC");
                    else
                        SortDataBy("cnt.CountryPopulation", "DESC");
                    break;
                case "По названию региона":
                    if (AscRadioButton.Checked == false && DescRadioButton.Checked == false)
                        AscRadioButton.Checked = true;
                    if (AscRadioButton.Checked == true)
                        SortDataBy("r.RegionName", "ASC");
                    else
                        SortDataBy("r.RegionName", "DESC");
                    break;
            }
        }
    }
}
