using System;

using UnityEngine;

namespace TowerBuilder.Stores.Time
{
    public partial class TimeStore
    {
        public static class Mutations
        {
            public static void UpdateTime(Time newTime)
            {
                TimeStore timeStore = Registry.storeRegistry.timeStore;
                TimeState previousState = timeStore.state;

                Registry.storeRegistry.timeStore.state.time = newTime;

                TimeState state = timeStore.state;

                if (TimeStore.Events.onTimeStateUpdated != null)
                {
                    TimeStore.StateEventPayload payload = new TimeStore.StateEventPayload()
                    {
                        state = state,
                        previousState = previousState,
                    };

                    TimeStore.Events.onTimeStateUpdated(payload);
                }
            }

            public static void AddTime(TimeInput timeInput)
            {
                TimeStore timeStore = Registry.storeRegistry.timeStore;
                TimeState state = timeStore.state;

                Time newTime = TimeStore.Helpers.AddTime(state.time, timeInput);
                UpdateTime(newTime);
            }

            public static void SubtractTime(TimeInput timeInput)
            {
                TimeStore timeStore = Registry.storeRegistry.timeStore;
                TimeState state = timeStore.state;
                Time newTime = TimeStore.Helpers.SubtractTime(state.time, timeInput);
                UpdateTime(newTime);
            }


            public static void Tick()
            {
                TimeStore timeStore = Registry.storeRegistry.timeStore;
                TimeState timeState = timeStore.state;
                Time time = timeState.time;
                AddTime(new TimeInput()
                {
                    minute = TimeStore.Constants.MINUTES_ELAPSED_PER_TICK
                });
            }

            public static void UpdateSpeed(TimeSpeed newTimeSpeed)
            {
                TimeStore timeStore = Registry.storeRegistry.timeStore;
                TimeState previousState = timeStore.state;

                Registry.storeRegistry.timeStore.state.speed = newTimeSpeed;

                TimeState state = timeStore.state;

                if (TimeStore.Events.onTimeStateUpdated != null)
                {
                    TimeStore.StateEventPayload payload = new TimeStore.StateEventPayload()
                    {
                        state = state,
                        previousState = previousState,
                    };

                    TimeStore.Events.onTimeStateUpdated(payload);
                }
            }
        }
    }
}