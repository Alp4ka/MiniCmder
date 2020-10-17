namespace MiniCmder
{
    /// <summary>
    /// Класс для упрощения работы с конкатенацией.
    /// </summary>
    public class Concatenator
    {
        private string data;
        public Concatenator()
        {
            SetNull();
        }
        /// <summary>
        /// Обнуляет this.data.
        /// </summary>
        /// <returns> this.data = "". </returns>
        public string SetNull()
        {
            data = "";
            return data;
        }
        /// <summary>
        /// Осуществляет конкатенацию строк.
        /// </summary>
        /// <param name="addition"></param>
        /// <returns></returns>
        public string Concat(string addition)
        {
            data += addition;
            return data;
        }
        /// <summary>
        /// Возвращает текущее значение Concatenator = this.data.
        /// </summary>
        /// <returns> this.data. </returns>
        public override string ToString()
        {
            return data;
        }
    }
}
