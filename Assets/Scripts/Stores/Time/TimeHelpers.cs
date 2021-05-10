using System;

namespace TowerBuilder.Stores.Time
{
    public partial class TimeStore
    {
        public static class Helpers
        {

            public static Time GetZeroTime()
            {
                return new Time()
                {

                    minute = 0,
                    hour = 0,
                    day = 0,
                    week = 0,
                    season = 0,
                    year = 0,
                };
            }

            public static Time GetFullTimeFromTimeInput(TimeInput timeInput)
            {
                Time time = GetZeroTime();

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

            public static int TimeToMinutes(Time time)
            {
                int minutes = time.minute;

                int hourMinutes = time.hour * TimeStore.Constants.MINUTES_PER_HOUR;
                int dayMinutes = time.day * TimeStore.Constants.MINUTES_PER_DAY;
                int weekMinutes = time.week * TimeStore.Constants.MINUTES_PER_WEEK;
                int seasonMinutes = time.season * TimeStore.Constants.MINUTES_PER_SEASON;
                int yearMinutes = time.year * TimeStore.Constants.MINUTES_PER_YEAR;

                return (
                    minutes +
                    hourMinutes +
                    dayMinutes +
                    weekMinutes +
                    seasonMinutes +
                    yearMinutes
                );
            }

            public static Time MinutesToTime(int minutes)
            {
                int leftover = minutes;
                int year = leftover / TimeStore.Constants.MINUTES_PER_YEAR;
                leftover = leftover % TimeStore.Constants.MINUTES_PER_YEAR;

                int season = leftover / TimeStore.Constants.MINUTES_PER_SEASON;
                leftover = leftover % TimeStore.Constants.MINUTES_PER_SEASON;

                int week = leftover / TimeStore.Constants.MINUTES_PER_WEEK;
                leftover = leftover % TimeStore.Constants.MINUTES_PER_WEEK;

                int day = leftover / TimeStore.Constants.MINUTES_PER_DAY;
                leftover = leftover % TimeStore.Constants.MINUTES_PER_DAY;

                int hour = leftover / TimeStore.Constants.MINUTES_PER_HOUR;
                leftover = leftover % TimeStore.Constants.MINUTES_PER_HOUR;

                int minute = leftover;

                return new Time()
                {
                    minute = minute,
                    hour = hour,
                    day = day,
                    week = week,
                    season = season,
                    year = year,
                };
            }

            public static Time AddTime(Time time, TimeInput timeInput)
            {
                int timeAsMinutes = TimeToMinutes(time);
                int timeInputAsMinutes = TimeToMinutes(GetFullTimeFromTimeInput(timeInput));

                int newMinutes = timeAsMinutes + timeInputAsMinutes;
                Time newTime = MinutesToTime(newMinutes);
                return newTime;
            }

            // TODO - make sure time doesn't go below 0
            public static Time SubtractTime(Time time, TimeInput timeInput)
            {
                int timeAsMinutes = TimeToMinutes(time);
                int timeInputAsMinutes = TimeToMinutes(GetFullTimeFromTimeInput(timeInput));

                int newMinutes = timeAsMinutes - timeInputAsMinutes;
                Time newTime = MinutesToTime(newMinutes);
                return newTime;
            }

            public static DayPeriod getDayPeriod(Time time)
            {
                int hour = time.hour;

                if (hour < 6)
                {
                    return DayPeriod.Night;
                }

                if (hour < 7)
                {
                    return DayPeriod.Dawn;
                }

                if (hour < 12)
                {
                    return DayPeriod.Morning;
                }

                if (hour < 17)
                {
                    return DayPeriod.Afternoon;
                }

                if (hour < 20)
                {
                    return DayPeriod.Evening;
                }

                if (hour < 21)
                {
                    return DayPeriod.Dusk;
                }

                return DayPeriod.Night;
            }
        }
    }
}