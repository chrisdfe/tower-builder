using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TowerBuilder.Stores.Time
{
    public partial class TimeStore
    {
        public static class Selectors
        {
            public static float GetCurrentTickInterval(TimeState timeState)
            {
                TimeSpeed currentSpeed = timeState.speed;
                float interval = TimeStore.Constants.TICK_INTERVAL * TimeStore.Constants.TIME_SPEED_TICK_INTERVALS[currentSpeed];
                return interval;
            }

        }
    }
}