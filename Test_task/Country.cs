/// <summary>
/// класс Country (страна) - для десериализации данных, полученных в формате JSON
/// </summary>
namespace Test_task
{
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
}
