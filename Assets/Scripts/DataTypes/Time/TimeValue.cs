using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Time
{
    public class TimeValue
    {
        // 0-59
        public int minute = 0;
        // 0-23
        public int hour = 0;
        // day of week
        public int day = 0;
        // week of season
        public int week = 0;
        // season of year
        public int season = 0;
        // 1 +
        public int year = 0;

        public static TimeValue zero
        {
            get
            {
                return new TimeValue()
                {

                    minute = 0,
                    hour = 0,
                    day = 0,
                    week = 0,
                    season = 0,
                    year = 0,
                };
            }
        }

        public static TimeValue FromTimeInput(TimeInput timeInput)
        {
            TimeValue time = TimeValue.zero;

            if (timeInput.minute != null)
            {
                time.minute = (int)timeInput.minute;
            }

            if (timeInput.hour != null)
            {
                time.hour = (int)timeInput.hour;
            }

            if (timeInput.day != null)
            {
                time.day = (int)timeInput.day;
            }

            if (timeInput.week != null)
            {
                time.week = (int)timeInput.week;
            }

            if (timeInput.season != null)
            {
                time.season = (int)timeInput.season;
            }

            if (timeInput.year != null)
            {
                time.year = (int)timeInput.year;
            }

            return time;
        }

        public TimeValue Clone()
        {
            return new TimeValue()
            {
                minute = minute,
                hour = hour,
                day = day,
                week = week,
                season = season,
                year = year
            };
        }
    }
}