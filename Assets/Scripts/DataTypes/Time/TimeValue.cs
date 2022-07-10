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

        public TimeValue() { }

        public TimeValue(TimeValue timeValue)
        {
            minute = (int)timeValue.minute;
            hour = (int)timeValue.hour;
            day = (int)timeValue.day;
            week = (int)timeValue.week;
            season = (int)timeValue.season;
            year = (int)timeValue.year;
        }

        public TimeValue(TimeInput timeInput)
        {
            if (timeInput.minute != null)
            {
                minute = (int)timeInput.minute;
            }

            if (timeInput.hour != null)
            {
                hour = (int)timeInput.hour;
            }

            if (timeInput.day != null)
            {
                day = (int)timeInput.day;
            }

            if (timeInput.week != null)
            {
                week = (int)timeInput.week;
            }

            if (timeInput.season != null)
            {
                season = (int)timeInput.season;
            }

            if (timeInput.year != null)
            {
                year = (int)timeInput.year;
            }
        }

        public TimeValue(int minutes)
        {
            SetFromMinutes(minutes);
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

        public int AsMinutes()
        {
            int minutes = minute;
            int hourMinutes = hour * Constants.MINUTES_PER_HOUR;
            int dayMinutes = day * Constants.MINUTES_PER_DAY;
            int weekMinutes = week * Constants.MINUTES_PER_WEEK;
            int seasonMinutes = season * Constants.MINUTES_PER_SEASON;
            int yearMinutes = year * Constants.MINUTES_PER_YEAR;

            return (
                minutes +
                hourMinutes +
                dayMinutes +
                weekMinutes +
                seasonMinutes +
                yearMinutes
            );
        }

        public void SetFromMinutes(int minutes)
        {
            int leftover = minutes;
            year = leftover / Constants.MINUTES_PER_YEAR;
            leftover = leftover % Constants.MINUTES_PER_YEAR;

            season = leftover / Constants.MINUTES_PER_SEASON;
            leftover = leftover % Constants.MINUTES_PER_SEASON;

            week = leftover / Constants.MINUTES_PER_WEEK;
            leftover = leftover % Constants.MINUTES_PER_WEEK;

            day = leftover / Constants.MINUTES_PER_DAY;
            leftover = leftover % Constants.MINUTES_PER_DAY;

            hour = leftover / Constants.MINUTES_PER_HOUR;
            leftover = leftover % Constants.MINUTES_PER_HOUR;

            minute = leftover;
        }

        public void Add(TimeInput timeInput)
        {
            int timeAsMinutes = AsMinutes();
            int timeInputAsMinutes = new TimeValue(timeInput).AsMinutes();

            int newMinutes = timeAsMinutes + timeInputAsMinutes;

            SetFromMinutes(newMinutes);
        }

        // TODO - make sure time doesn't go below 0
        public void Subtract(TimeInput timeInput)
        {
            int timeAsMinutes = AsMinutes();
            int timeInputAsMinutes = new TimeValue(timeInput).AsMinutes();

            int newMinutes = timeAsMinutes - timeInputAsMinutes;

            SetFromMinutes(newMinutes);
        }

        public int GetCurrentTimeOfDayIndex()
        {
            for (int i = Constants.TIMES_OF_DAY.Length - 1; i >= 0; i--)
            {
                TimeOfDay timeOfDay = Constants.TIMES_OF_DAY[i];

                if (hour >= timeOfDay.startsOnHour)
                {
                    return i;
                }
            }

            return 0;
        }

        public TimeOfDay GetCurrentTimeOfDay()
        {
            return Constants.TIMES_OF_DAY[GetCurrentTimeOfDayIndex()];
        }

        public int GetNextTimeOfDayIndex()
        {
            int index = GetCurrentTimeOfDayIndex();
            index++;
            if (index >= Constants.TIMES_OF_DAY.Length - 1)
            {
                index = 0;
            }
            return index;
        }

        public TimeOfDay GetNextTimeOfDay()
        {
            return Constants.TIMES_OF_DAY[GetNextTimeOfDayIndex()];
        }
    }
}