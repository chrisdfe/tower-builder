using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Time
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

        public class Events
        {
            public delegate void TimeUpdatedEvent(TimeValue newTime);
            public TimeUpdatedEvent onTimeUpdated;
            public TimeUpdatedEvent onTimeOfDayUpdated;
            public TimeUpdatedEvent onTick;

            public delegate void TimeSpeedUpdatedEvent(TimeSpeed newTimeSpeed);
            public TimeSpeedUpdatedEvent onTimeSpeedUpdated;
        }

        public bool isActive { get; private set; } = false;
        public int tick { get; private set; } = 0;
        public TimeValue time { get; private set; } = TimeValue.zero;
        public TimeSpeed speed { get; private set; } = TimeSpeed.Normal;

        public Events events { get; private set; }
        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            isActive = input.isActive ?? false;
            tick = input.tick ?? 0;
            time = input.time ?? TimeValue.zero;
            speed = input.speed ?? TimeSpeed.Normal;

            events = new Events();
            queries = new Queries(this);
        }

        public void UpdateTime(TimeValue newTime)
        {
            TimeValue previousTime = time;

            time = newTime;

            if (previousTime.GetCurrentTimeOfDay() != time.GetCurrentTimeOfDay())
            {
                if (events.onTimeOfDayUpdated != null)
                {
                    events.onTimeOfDayUpdated(time);
                }
            }

            if (events.onTimeUpdated != null)
            {
                events.onTimeUpdated(time);
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

            if (events.onTick != null)
            {
                events.onTick(time);
            }
        }

        public void UpdateSpeed(TimeSpeed newTimeSpeed)
        {
            speed = newTimeSpeed;
        }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }

            public float currentTickInterval
            {
                get
                {
                    return Constants.TIME_SPEED_TICK_INTERVALS[state.speed];
                }
            }

            // "Relative" refers to a relative to the start of the current day
            public TimeValue currentRelativeTimeValue
            {
                get
                {
                    return new TimeValue(new TimeValue.Input()
                    {
                        hour = state.time.hour,
                        minute = state.time.minute
                    });
                }
            }

            public int currentTickRelativeTimeValueAsMinutes
            {
                get
                {
                    return currentRelativeTimeValue.AsMinutes();
                }
            }

            public TimeValue nextTickTimeValue
            {
                get
                {
                    return TimeValue.Add(
                        state.time,
                        new TimeValue.Input()
                        {
                            minute = Constants.MINUTES_ELAPSED_PER_TICK
                        }
                    );
                }
            }

            public TimeValue nextTickRelativeTimeValue
            {
                get
                {
                    return TimeValue.Add(
                        currentRelativeTimeValue,
                        new TimeValue.Input()
                        {
                            minute = Constants.MINUTES_ELAPSED_PER_TICK
                        }
                    );
                }
            }

            public int nextTickRelativeTimeValueAsMinutes
            {
                get
                {
                    return nextTickRelativeTimeValue.AsMinutes();
                }
            }
        }
    }
}