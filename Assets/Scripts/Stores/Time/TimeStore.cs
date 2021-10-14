using System.Collections;
using System.Collections.Generic;

using TowerBuilder;
using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Time
{
    public partial class TimeStore : StoreBase<TimeState>
    {
        public TimeStore()
        {
            state.isActive = false;
            state.tick = 0;
            state.time = new Time()
            {
                minute = 0,
                hour = 0,
                day = 0,
                week = 0,
                season = 0,
                year = 0,
            };
            state.speed = TimeSpeed.Normal;
        }
    }
}
