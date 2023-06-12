using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups.Buildings;
using UnityEngine;

namespace TowerBuilder.ApplicationState.EntityGroups.Buildings
{
    public class State : EntityGroupStateSlice
    {
        public class Input
        {
            public ListWrapper<Building> buildingList;
        }

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