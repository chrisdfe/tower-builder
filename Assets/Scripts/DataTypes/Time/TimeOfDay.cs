
namespace TowerBuilder.DataTypes.Time
{
    public class TimeOfDay
    {
        public enum Key
        {
            MorningNight,
            Dawn,
            Morning,
            Afternoon,
            Evening,
            Dusk,
            DuskNight
        }

        public Key key;
        public int startsOnHour;
        public string name;
    }
}