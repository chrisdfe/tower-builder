using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TowerBuilder.Stores.Time
{
    public static class Selectors
    {
        public static float GetCurrentTickInterval(State timeState)
        {
            TimeSpeed currentSpeed = timeState.speed;
            float interval = Constants.TICK_INTERVAL * Constants.TIME_SPEED_TICK_INTERVALS[currentSpeed];
            return interval;
        }
    }
}
