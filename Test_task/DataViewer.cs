using System;
using System.Collections.Generic;
using System.Data;

namespace Test_task
{
    /// <summary>
    /// класс Dataviewer - отвечает за вывод информации в таблицу
    /// </summary>
    class DataViewer
    {
        /// <summary>
        /// форма, на которой находится таблица для вывода записей
        /// </summary>
        private Form1 form;

        /// <summary>
        /// конструктор класса DataViewer
        /// </summary>
        /// <param name="form">форма, на которой находится таблица для вывода записей</param>
        public DataViewer(Form1 form)
        {
            this.form = form;
        }

        /// <summary>
        /// изменение ширины столбца
        /// </summary>
        /// <param name="columnID">номер столбца, ширину которого требуется изменить 
        /// (начинается с 0)</param>
        /// <param name="width">ширина столбца, которую требуется установить</param>
        public void SetColumnWidth(int columnID, int width)
        {
            var column = form.dataGridView.Columns[columnID];
            column.Width = width;
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
            form.dataGridView.Columns.Clear();
        }

        /// <summary>
        /// удаление столбцов и лишних строк
        /// </summary>
        public void ClearAllCells()
        {
            form.dataGridView.DataSource = null;
            form.dataGridView.RowCount = 1;
            form.dataGridView.Columns.Clear();
        }

        /// <summary>
        /// добавление столбцов в DataGridView
        /// </summary>
        public void CreateColumns()
        {
            form.dataGridView.Columns.Add("name", "Название страны");
            form.dataGridView.Columns.Add("alpha3Code", "Код");
            form.dataGridView.Columns.Add("capital", "Столица");
            form.dataGridView.Columns.Add("area", "Площадь");
            form.dataGridView.Columns.Add("population", "Население");
            form.dataGridView.Columns.Add("region", "Регион");
            SetColumnsWidth();
        }

        /// <summary>
        /// вывод информации о стране в datagridview
        /// </summary>
        /// <param name="countries">информация о стране</param>
        public void Show(List<Country> countries)
        {
            // если в datagridview есть столбцы - удаление столбцов и лишних строк
            if (form.dataGridView.Columns.Count != 0)
            {
                ClearAllCells();
            }
            CreateColumns();
            // номер текущей строки таблицы
            int rowID;
            // заполнение ячеек таблицы полученными данными
            foreach (Country country in countries)
            {
                try
                {
                    // добавление новой строки в таблице и получение ее номера
                    rowID = form.dataGridView.Rows.Add();
                    form.dataGridView.Rows[rowID].Cells["name"].Value = country.name;
                    form.dataGridView.Rows[rowID].Cells["alpha3Code"].Value = country.alpha3Code;
                    form.dataGridView.Rows[rowID].Cells["capital"].Value = country.capital;
                    // там, где значение площади страны должно быть null, выводим null
                    form.dataGridView.Rows[rowID].Cells["area"].Value = (country.area >= 0) ? country.area : null;
                    form.dataGridView.Rows[rowID].Cells["population"].Value = country.population;
                    form.dataGridView.Rows[rowID].Cells["region"].Value = country.region;
                }
                catch (Exception e)
                {
                    Dialog.Message("Возникла проблема при добавлении строки\n" + e.Message);
                }
            }
        }

        /// <summary>
        /// вывод информации о всех странах в базе данных
        /// </summary>
        /// <param name="dataTable">данные из БД</param>
        public void ShowTable(DataTable dataTable)
        {
            try
            {
                form.dataGridView.DataSource = dataTable;
                SetColumnsWidth();
            }
            catch (Exception e)
            {
                Dialog.Message("Возникла проблема при получении данных\n" + e.Message);
            }
        }
    }
}
