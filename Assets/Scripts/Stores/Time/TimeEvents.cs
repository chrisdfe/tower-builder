using System;

namespace TowerBuilder.Stores.Time
{
    public partial class TimeStore
    {
        public struct StateEventPayload
        {
            public TimeState state;
            public TimeState previousState;
        }

        public static class Events
        {
            public delegate void OnTimeStateUpdated(StateEventPayload payload);
            public static OnTimeStateUpdated onTimeStateUpdated;
        }
    }
}
