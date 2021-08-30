using System.Windows.Forms;

namespace Test_task
{
    /// <summary>
    /// класс Dialog - отвечает за вывод диалоговых окон
    /// </summary>
    class Dialog
    {
        /// <summary>
        /// вывод простого MessageBox
        /// </summary>
        /// <param name="message">
        /// текст, который будет отображен в MessageBox</param>
        public static void Message(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// вывод MessageBox с кнопками Да/Нет 
        /// с целью предложения сохранить полученные данные в БД
        /// </summary>
        /// <returns>DialogResult - какая кнопка была нажата (Да/Нет)</returns>
        public static DialogResult AskSaveOrNot()
        {
            return MessageBox.Show("Сохранить данные в базу данных?", "Сохранение", MessageBoxButtons.YesNo);
        }
    }
}
