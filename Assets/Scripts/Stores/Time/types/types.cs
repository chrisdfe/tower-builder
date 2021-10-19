using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Time
{
    public struct TimeInput
    {
        public int? minute;
        public int? hour;
        public int? day;
        public int? week;
        public int? season;
        public int? year;
    }

    public enum TimeSpeed
    {
        Pause = 0,
        Normal = 1,
        Fast = 2,
        Fastest = 3
    }

    public enum DayPeriod
    {
        Dawn,
        Morning,
        Afternoon,
        Evening,
        Dusk,
        Night
    }

    public enum DayType
    {
        Weekday,
        Weekend,
    }

    public enum SeasonType
    {
        Spring,
        Summer,
        Autumn,
        Winter,
    }

    public struct TimeState
    {
        public bool isActive;
        public int tick;
        public TimeValue currentTime;
        public TimeSpeed speed;
    }
}