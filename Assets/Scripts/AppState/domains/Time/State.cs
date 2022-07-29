using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.State.Time
{
    public class State
    {
        public struct Input
        {
            public bool? isActive;
            public int? tick;
            public TimeValue? time;
            public TimeSpeed? speed;
        }

        public ResourceStructField<bool> isActive { get; private set; } = new ResourceStructField<bool>(false);
        public ResourceStructField<int> tick { get; private set; } = new ResourceStructField<int>(0);
        public ResourceStructField<TimeValue> time { get; private set; } = new ResourceStructField<TimeValue>(TimeValue.zero);
        public ResourceField<TimeSpeed> speed { get; private set; } = new ResourceField<TimeSpeed>();

        public delegate void TimeUpdatedEvent(TimeValue newTime);
        public TimeUpdatedEvent onTimeOfDayChanged;
        public TimeUpdatedEvent onTick;

        public State() : this(new Input()) { }

        public State(Input input)
        {
            isActive.value = input.isActive ?? false;
            tick.value = input.tick ?? 0;
            time.value = input.time ?? TimeValue.zero;
            speed.value = input.speed ?? TimeSpeed.Normal;
        }

        public void UpdateTime(TimeValue newTime)
        {
            TimeValue previousTime = time.value.Clone();

            time.value = newTime;

            if (previousTime.GetCurrentTimeOfDay() != time.value.GetCurrentTimeOfDay())
            {
                if (onTimeOfDayChanged != null)
                {
                    onTimeOfDayChanged(time.value);
                }
            }
        }

        public void AddTime(TimeValue.Input timeInput)
        {
            TimeValue newTime = TimeValue.Add(time.value, timeInput);
            UpdateTime(newTime);
        }

        public void SubtractTime(TimeValue.Input timeInput)
        {
            TimeValue newTime = TimeValue.Subtract(time.value, timeInput);
            UpdateTime(newTime);
        }

        public void Tick()
        {
            AddTime(new TimeValue.Input()
            {
                minute = Constants.MINUTES_ELAPSED_PER_TICK
            });

            tick.value += 1;

            if (onTick != null)
            {
                onTick(time.value);
            }
        }

        public void UpdateSpeed(TimeSpeed newTimeSpeed)
        {
            speed.value = newTimeSpeed;
        }

        public float GetCurrentTickInterval()
        {
            return Constants.TICK_INTERVAL * Constants.TIME_SPEED_TICK_INTERVALS[speed.value];
        }
    }
}