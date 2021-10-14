using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Time
{
    public partial class TimeStore
    {
        public static class Constants
        {
            public const int MINUTES_ELAPSED_PER_TICK = 15;

            // 1 second
            public const int TICK_INTERVAL = 1;

            // tick intervals in seconds
            public static Dictionary<TimeSpeed, float> TIME_SPEED_TICK_INTERVALS = new Dictionary<TimeSpeed, float>()
            {
                [TimeSpeed.Normal] = 1.0f,
                [TimeSpeed.Fast] = 0.7f,
                [TimeSpeed.Fastest] = 0.1f,
            };

            public const int MINUTES_PER_HOUR = 60;

            public const int HOURS_PER_DAY = 24;

            public const int MINUTES_PER_DAY = HOURS_PER_DAY * MINUTES_PER_HOUR;

            public static DayType[] WEEKDAY_SEQUENCE = new DayType[]
            {
                DayType.Weekday,
                DayType.Weekday,
                DayType.Weekday,
                DayType.Weekend,
                DayType.Weekend,
            };

            public static int DAYS_PER_WEEK = WEEKDAY_SEQUENCE.Length;

            public static Dictionary<DayType, String> WEEKDAY_LABEL_MAP = new Dictionary<DayType, String>
            {
                [DayType.Weekday] = "Weekday",
                [DayType.Weekend] = "Weekend",
            };

            public static int MINUTES_PER_WEEK = DAYS_PER_WEEK * MINUTES_PER_DAY;

            public const int WEEKS_PER_SEASON = 4;

            public static SeasonType[] SEASON_SEQUENCE = new SeasonType[]
            {
                SeasonType.Spring,
                SeasonType.Summer,
                SeasonType.Autumn,
                SeasonType.Winter,
            };

            public static Dictionary<SeasonType, String> SEASON_LABEL_MAP = new Dictionary<SeasonType, String>()
            {
                [SeasonType.Spring] = "Spring",
                [SeasonType.Summer] = "Summer",
                [SeasonType.Autumn] = "Autumn",
                [SeasonType.Winter] = "Winter",
            };

            public static int MINUTES_PER_SEASON = WEEKS_PER_SEASON * MINUTES_PER_WEEK;

            public static int SEASONS_PER_YEAR = SEASON_SEQUENCE.Length;

            public static int MINUTES_PER_YEAR = SEASONS_PER_YEAR * MINUTES_PER_SEASON;
        }
    }
}