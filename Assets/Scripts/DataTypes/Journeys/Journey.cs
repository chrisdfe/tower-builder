namespace TowerBuilder.DataTypes.Journeys
{
    public class Journey
    {
        public int id { get; private set; }

        public string title { get; private set; } = "None";
        public string key { get; private set; } = "None";
        public string category { get; private set; } = "None";

        public float totalDistance = 1000;
        public float currentProgress = 0f;
    }
}