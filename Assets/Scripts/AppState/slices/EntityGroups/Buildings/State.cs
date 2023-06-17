using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups;
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

            appState.EntityGroups.Rooms.onItemsAdded += OnRoomsAdded;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.EntityGroups.Rooms.onItemsAdded -= OnRoomsAdded;
        }

        void OnRoomsAdded(ListWrapper<EntityGroup> rooms)
        {
            Debug.Log("a room has been addded and now I will check if it should be added to a building");
        }
    }
}