using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.State.Time
{
    public class State : StateSlice
    {
        public struct Input
        {
            public bool? isActive;
            public int? tick;
            public TimeValue? time;
            public TimeSpeed? speed;
        }

        public bool isActive { get; private set; } = false;
        public int tick { get; private set; } = 0;
        public TimeValue time { get; private set; } = TimeValue.zero;
        public TimeSpeed speed { get; private set; } = TimeSpeed.Normal;

        public delegate void TimeUpdatedEvent(TimeValue newTime);
        public TimeUpdatedEvent onTimeUpdated;
        public TimeUpdatedEvent onTimeOfDayUpdated;
        public TimeUpdatedEvent onTick;

        public delegate void TimeSpeedUpdatedEvent(TimeSpeed newTimeSpeed);
        public TimeSpeedUpdatedEvent onTimeSpeedUpdated;

        public State(AppState appState, Input input) : base(appState)
        {
            isActive = input.isActive ?? false;
            tick = input.tick ?? 0;
            time = input.time ?? TimeValue.zero;
            speed = input.speed ?? TimeSpeed.Normal;
        }

        public void UpdateTime(TimeValue newTime)
        {
            TimeValue previousTime = time;

            time = newTime;

            if (previousTime.GetCurrentTimeOfDay() != time.GetCurrentTimeOfDay())
            {
                if (onTimeOfDayUpdated != null)
                {
                    onTimeOfDayUpdated(time);
                }
            }

            if (onTimeUpdated != null)
            {
                onTimeUpdated(time);
            }
        }

        public void AddTime(TimeValue.Input timeInput)
        {
            TimeValue newTime = TimeValue.Add(time, timeInput);
            UpdateTime(newTime);
        }

        public void SubtractTime(TimeValue.Input timeInput)
        {
            TimeValue newTime = TimeValue.Subtract(time, timeInput);
            UpdateTime(newTime);
        }

        public void Tick()
        {
            AddTime(new TimeValue.Input()
            {
                minute = Constants.MINUTES_ELAPSED_PER_TICK
            });

            tick += 1;

            if (onTick != null)
            {
                onTick(time);
            }
        }

        public void UpdateSpeed(TimeSpeed newTimeSpeed)
        {
            speed = newTimeSpeed;
        }

        public float GetCurrentTickInterval()
        {
            return Constants.TICK_INTERVAL * Constants.TIME_SPEED_TICK_INTERVALS[speed];
        }
    }
}