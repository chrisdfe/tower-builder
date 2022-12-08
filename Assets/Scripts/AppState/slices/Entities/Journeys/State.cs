using TowerBuilder.DataTypes.Journeys;

namespace TowerBuilder.ApplicationState.Entities.Journeys
{
    using JourneyListStateSlice = ListStateSlice<JourneyList, Journey, State.Events>;

    public class State : JourneyListStateSlice
    {
        public class Input { }

        public new class Events : JourneyListStateSlice.Events { }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }
        }

        public Journey currentJourney;

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(this);
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}