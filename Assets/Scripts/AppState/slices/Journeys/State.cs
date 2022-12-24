using TowerBuilder.DataTypes.Journeys;

namespace TowerBuilder.ApplicationState.Journeys
{
    public class State : StateSlice
    {
        public class Input { }

        public class Events
        {
            public delegate void JourneyEvent(Journey journey);
            public JourneyEvent onJourneyProgressUpdated;
        }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }
        }

        // TODO - don't initialize this here like this
        public Journey currentJourney = new Journey();

        public Events events { get; } = new Events();
        public Queries queries { get; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(this);
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public void UpdateJourneyProgress(float amount)
        {
            currentJourney.currentProgress += amount;
            events.onJourneyProgressUpdated?.Invoke(currentJourney);
        }
    }
}