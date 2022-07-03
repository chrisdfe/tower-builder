using TowerBuilder.DataTypes.Time;

namespace TowerBuilder.State.Time
{
    public class State
    {
        public struct Input
        {
            public bool? isActive;
            public int? tick;
            public TimeValue time;
            public TimeSpeed? speed;
        }

        public bool isActive { get; private set; } = false;
        public int tick { get; private set; } = 0;
        public TimeValue time { get; private set; }
        public TimeSpeed speed { get; private set; }

        public delegate void TimeUpdatedEvent(TimeValue newTime);
        public TimeUpdatedEvent onTimeUpdated;
        public TimeUpdatedEvent onTick;

        public delegate void SpeedUpdatedEvent(TimeSpeed newTimeSpeed);
        public SpeedUpdatedEvent onTimeSpeedUpdated;

        public State() : this(new Input()) { }

        public State(Input input)
        {
            isActive = input.isActive ?? false;
            tick = input.tick ?? 0;
            time = input.time ?? TimeValue.zero;
            speed = input.speed ?? TimeSpeed.Normal;
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