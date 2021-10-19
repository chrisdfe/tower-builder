namespace TowerBuilder.Stores.Time
{
    public class State
    {
        public bool isActive { get; private set; }
        public int tick { get; private set; }
        public TimeValue time { get; private set; }
        public TimeSpeed speed { get; private set; }

        public delegate void TimeUpdatedEvent(TimeValue newTime);
        public TimeUpdatedEvent onTimeUpdated;
        public TimeUpdatedEvent onTick;

        public delegate void SpeedUpdatedEvent(TimeSpeed newTimeSpeed);
        public SpeedUpdatedEvent onTimeSpeedUpdated;

        public State()
        {
            isActive = false;
            tick = 0;
            time = new TimeValue()
            {
                minute = 0,
                hour = 0,
                day = 0,
                week = 0,
                season = 0,
                year = 0,
            };
            speed = TimeSpeed.Normal;
        }

        public void UpdateTime(TimeValue newTime)
        {
            time = newTime;

            if (onTimeUpdated != null)
            {
                onTimeUpdated(newTime);
            }
        }

        public void AddTime(TimeInput timeInput)
        {
            TimeValue newTime = Helpers.AddTime(time, timeInput);
            UpdateTime(newTime);
        }

        public void SubtractTime(TimeInput timeInput)
        {
            TimeValue newTime = Helpers.SubtractTime(time, timeInput);
            UpdateTime(newTime);
        }


        public void Tick()
        {
            AddTime(new TimeInput()
            {
                minute = Constants.MINUTES_ELAPSED_PER_TICK
            });

            if (onTick != null)
            {
                onTick(time);
            }
        }

        public void UpdateSpeed(TimeSpeed newTimeSpeed)
        {
            speed = newTimeSpeed;

            if (onTimeSpeedUpdated != null)
            {
                onTimeSpeedUpdated(speed);
            }
        }
    }
}