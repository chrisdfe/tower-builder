using System;
using TowerBuilder.DataTypes.Entities.InteriorWalls;

namespace TowerBuilder.ApplicationState.Entities.InteriorWalls
{
    [Serializable]
    public class State : EntityStateSlice<InteriorWall, State.Events>
    {
        public class Input { }

        public new class Events : EntityStateSlice<InteriorWall, State.Events>.Events { }

        public new Queries queries { get; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
        }

        public override void Setup() { }

        public override void Teardown() { }

        public new class Queries : EntityStateSlice<InteriorWall, State.Events>.Queries
        {
            public Queries(AppState appState, State state) : base(appState, state) { }
        }
    }
}
