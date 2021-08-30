using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace Test_task
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// объект класса DataViewer для обращения к нему с целью вывода информации
        /// </summary>
        private DataViewer dataViewer;
        /// <summary>
        /// объект класса UIManager для обращения к нему с целью изменения свойств элементов интерфейса
        /// </summary>
        private UIManager uiManager;
        /// <summary>
        /// объект класса DataManager для обращения к нему с целью управления данными в БД
        /// </summary>
        private DataManager dataManager;

        /// <summary>
        /// инициализация формы и создание объектов классов
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            dataViewer = new DataViewer(this);
            dataManager = new DataManager(this);
            uiManager = new UIManager(this);
        }

        /// <summary>
        /// поиск по стране
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, EventArgs e)
        {
            uiManager.ClearSortComboBox();
            if (countryTextBox.Text != "")
            {
                try
                {
                    // получение данных о стране
                    List<Country> countries = DataReceiver.GetInfo(countryTextBox.Text);

                    // вывод полученных данных в таблицу
                    dataViewer.Show(countries);

                    // предложение пользователю сохранить данные в базе данных
                    var dialogResult = Dialog.AskSaveOrNot();
                    
                    if (dialogResult == DialogResult.Yes)
                    {
                        // если пользователь хочет сохранить данные в базе данных
                        try
                        {
                            foreach (Country country in countries)
                            {
                                int cityId,  // идентификатор записи о городе в таблице городов
                                    regionId, // идентификатор записи о регионе в таблице регионов
                                    citiesCount, // кличество записей о городе в таблице городов
                                    regionsCount, // количество записей о регионе в таблице регионов
                                    countriesCount; // количество записей о стране в таблице стран
                                // проверка наличия города в таблице городов
                                citiesCount = DataManager.Count("dbo.Cities", "CityName", country.capital);
                                if (citiesCount != 0)
                                {
                                    // если город есть в таблице городов, то просто берем его ID
                                    cityId = DataManager.GetElementId("dbo.Cities", "CityName", country.capital);
                                }
                                else
                                {
                                    // если города нет в таблице городов, то добавляем, и затем берем его ID
                                    DataManager.Insert("dbo.Cities", "CityName", country.capital);
                                    cityId = DataManager.GetElementId("dbo.Cities", "CityName", country.capital);
                                }
                                // проверка наличия региона в таблице регионов
                                regionsCount = DataManager.Count("dbo.Regions", "RegionName", country.region);
                                if (regionsCount != 0)
                                {
                                    // если регион есть в таблице регионов, то просто берем его ID
                                    regionId = DataManager.GetElementId("dbo.Regions", "RegionName", country.region);
                                }
                                else
                                {
                                    // если региона нет в таблице регионов, то добавляем, и затем берем его ID
                                    DataManager.Insert("dbo.Regions", "RegionName", country.region);
                                    regionId = DataManager.GetElementId("dbo.Regions", "RegionName", country.region);
                                }
                                // проверка наличия страны в таблице стран
                                countriesCount = DataManager.Count("dbo.Countries", "CountryCode", country.alpha3Code);
                                if (countriesCount == 0)
                                {
                                    // если страны нет в таблице стран, то добавляем ее в таблицу
                                    DataManager.InsertCountry(country, cityId, regionId);
                                }
                                else
                                {
                                    // если страна есть в таблице стран, обновляем данные о ней
                                    DataManager.UpdateCountry(country, cityId, regionId);
                                }
                            }
                            Dialog.Message("Данные успешно обновлены!");
                        }
                        catch (Exception ex)
                        {
                            Dialog.Message("Возникла ошибка:\n" + ex.Message);
                        }
                    }
                }
                catch (WebException webException)
                {
                    var status = webException.Status;
                    if (status == WebExceptionStatus.ProtocolError)
                    {
                        var httpResponse = (HttpWebResponse)webException.Response;
                        Dialog.Message("Не удалось получить данные.\nОшибка " + 
                            (int)httpResponse.StatusCode + " - " + httpResponse.StatusCode);
                    }
                    uiManager.ClearCountryTextBox();
                }
                catch (Exception ex)
                {
                    Dialog.Message("Возникла ошибка:\n" + ex.Message);
                    uiManager.ClearCountryTextBox();
                }
            }
            else
            {
                Dialog.Message("Строка поиска страны пустая.");
            }
        }

        /// <summary>
        /// вывод всех записей из базы данных в порядке добавления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataBaseButton_Click(object sender, EventArgs e)
        {
            uiManager.SetSortComboBoxText("В порядке добавления");
            dataManager.GetUnsortedData();
        }

        /// <summary>
        /// вывод всех записей из базы данных с указанной сортировкой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortComboBox_RadioButton_Changed(object sender, EventArgs e)
        {
            dataManager.GetSortedData();
        }
    }
}
