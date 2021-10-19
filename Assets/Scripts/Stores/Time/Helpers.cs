using System;

namespace TowerBuilder.Stores.Time
{
    public static class Helpers
    {

        public static TimeValue GetFullTimeFromTimeInput(TimeInput timeInput)
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

        public static int TimeToMinutes(TimeValue time)
        {
            int minutes = time.minute;

            int hourMinutes = time.hour * Constants.MINUTES_PER_HOUR;
            int dayMinutes = time.day * Constants.MINUTES_PER_DAY;
            int weekMinutes = time.week * Constants.MINUTES_PER_WEEK;
            int seasonMinutes = time.season * Constants.MINUTES_PER_SEASON;
            int yearMinutes = time.year * Constants.MINUTES_PER_YEAR;

            return (
                minutes +
                hourMinutes +
                dayMinutes +
                weekMinutes +
                seasonMinutes +
                yearMinutes
            );
        }

        public static TimeValue MinutesToTime(int minutes)
        {
            int leftover = minutes;
            int year = leftover / Constants.MINUTES_PER_YEAR;
            leftover = leftover % Constants.MINUTES_PER_YEAR;

            int season = leftover / Constants.MINUTES_PER_SEASON;
            leftover = leftover % Constants.MINUTES_PER_SEASON;

            int week = leftover / Constants.MINUTES_PER_WEEK;
            leftover = leftover % Constants.MINUTES_PER_WEEK;

            int day = leftover / Constants.MINUTES_PER_DAY;
            leftover = leftover % Constants.MINUTES_PER_DAY;

            int hour = leftover / Constants.MINUTES_PER_HOUR;
            leftover = leftover % Constants.MINUTES_PER_HOUR;

            int minute = leftover;

            return new TimeValue()
            {
                minute = minute,
                hour = hour,
                day = day,
                week = week,
                season = season,
                year = year,
            };
        }

        public static TimeValue AddTime(TimeValue time, TimeInput timeInput)
        {
            int timeAsMinutes = TimeToMinutes(time);
            int timeInputAsMinutes = TimeToMinutes(GetFullTimeFromTimeInput(timeInput));

            int newMinutes = timeAsMinutes + timeInputAsMinutes;
            TimeValue newTime = MinutesToTime(newMinutes);
            return newTime;
        }

        // TODO - make sure time doesn't go below 0
        public static TimeValue SubtractTime(TimeValue time, TimeInput timeInput)
        {
            int timeAsMinutes = TimeToMinutes(time);
            int timeInputAsMinutes = TimeToMinutes(GetFullTimeFromTimeInput(timeInput));

            int newMinutes = timeAsMinutes - timeInputAsMinutes;
            TimeValue newTime = MinutesToTime(newMinutes);
            return newTime;
        }

        public static DayPeriod getDayPeriod(TimeValue time)
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
