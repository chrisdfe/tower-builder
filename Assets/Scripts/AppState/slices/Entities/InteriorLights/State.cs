using System;
using TowerBuilder.DataTypes.Entities.InteriorLights;

namespace TowerBuilder.ApplicationState.Entities.InteriorLights
{
    [Serializable]
    public class State : EntityStateSlice<InteriorLight, State.Events>
    {
        public class Input { }

        public new class Events : EntityStateSlice<InteriorLight, State.Events>.Events { }

        public new Queries queries { get; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
        }

        public override void Setup() { }

        public override void Teardown() { }

        public new class Queries : EntityStateSlice<InteriorLight, State.Events>.Queries
        {
            public Queries(AppState appState, State state) : base(appState, state) { }
        }
    }
}
