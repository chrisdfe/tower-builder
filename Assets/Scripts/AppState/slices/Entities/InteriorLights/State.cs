using System;
using TowerBuilder.DataTypes.Entities.InteriorLights;

namespace TowerBuilder.ApplicationState.Entities.InteriorLights
{
    [Serializable]
    public class State : EntityStateSlice
    {
        public class Input { }

        public State(AppState appState, Input input) : base(appState)
        {
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Teardown()
        {
            base.Teardown();
        }
    }
}
