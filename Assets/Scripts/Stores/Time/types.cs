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

    public struct Time
    {
        // 0-59
        public int minute;
        // 0-23
        public int hour;
        // day of week
        public int day;
        // week of season
        public int week;
        // season of year
        public int season;
        // 1 +
        public int year;
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
        public Time time;
        public TimeSpeed speed;
    }
}