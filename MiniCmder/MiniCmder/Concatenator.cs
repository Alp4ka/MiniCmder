namespace MiniCmder
{
    public class Concatenator
    {
        private string data;
        public Concatenator()
        {
            SetNull();
        }
        public string SetNull()
        {
            data = "";
            return data;
        }
        public string Concat(string addition)
        {
            data += addition;
            return data;
        }
        public virtual string ToString()
        {
            return data;
        }
    }
}
