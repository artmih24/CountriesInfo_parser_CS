using System.Windows.Forms;

namespace Test_task
{
    /// <summary>
    /// класс UIManager - отвечает за изменения свойств 
    /// прочих элементов графического интерфейса 
    /// </summary>
    class UIManager
    {
        /// <summary>
        /// форма, на которой находится таблица для вывода записей
        /// </summary>
        private Form1 form;

        /// <summary>
        /// конструктор класса UIManager
        /// </summary>
        /// <param name="form">форма, на которой находится таблица для вывода записей</param>
        public UIManager(Form1 form)
        {
            this.form = form;
        }

        /// <summary>
        /// очистка строки поиска страны
        /// </summary>
        public void ClearCountryTextBox()
        {
            form.countryTextBox.Text = "";
        }

        /// <summary>
        /// очистка текста в ComboBox для сортировки
        /// </summary>
        public void ClearSortComboBox()
        {
            form.sortComboBox.Text = "";
        }

        /// <summary>
        /// установка текста в ComboBox для сортировки
        /// </summary>
        /// <param name="text">текст сообщения, выводимый в MessageBox</param>
        public void SetSortComboBoxText(string text)
        {
            form.sortComboBox.Text = text;
        }

        /// <summary>
        /// установка RadioButton в состояние Checked
        /// </summary>
        /// <param name="radioButton">RadioButton, который требуется 
        /// возвести в Checked</param>
        public void CheckRadioButton(RadioButton radioButton)
        {
            radioButton.Checked = true;
        }
    }
}
