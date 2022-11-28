using TowerBuilder.DataTypes.Journeys;

namespace TowerBuilder.ApplicationState.Journeys
{
    public class State : StateSlice
    {
        public class Input { }

        public class Events { }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }
        }

        public Journey currentJourney;

        public Events events { get; private set; }
        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            events = new Events();
            queries = new Queries(this);
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}