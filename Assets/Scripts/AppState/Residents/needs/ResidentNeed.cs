namespace TowerBuilder.State.Residents
{
    public class ResidentNeed
    {
        public string name { get; private set; }

        public int currentValue { get; private set; } = 100;
        int MIN = 0;
        int MAX = 100;

        public ResidentNeed(string name)
        {
            this.name = name;
        }
    }
}