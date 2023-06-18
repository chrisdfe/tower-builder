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

            appState.EntityGroups.Rooms.onItemsBuilt += OnRoomsBuilt;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.EntityGroups.Rooms.onItemsBuilt -= OnRoomsBuilt;
        }


        void OnRoomsBuilt(ListWrapper<EntityGroup> rooms)
        {
            foreach (EntityGroup room in rooms.items)
            {
                if (room.parent == null)
                {
                    CreateNewOrAddToExistingBuilding(room);
                }
            }
        }

        void CreateNewOrAddToExistingBuilding(EntityGroup room)
        {

            ListWrapper<EntityGroup> neighborRooms = FindNeighborRooms(room);

            // TODO - something has gone wrong if these rooms don't have a parent building
            EntityGroup entityGroupWithParent = neighborRooms.Find(neighborRoom => neighborRoom.parent != null);

            if (entityGroupWithParent != null)
            {
                AddToEntityGroup(entityGroupWithParent.parent, room);
            }
            else
            {
                // TODO - validate here that player hasn't reached their building limit
                // Room is the first in the building
                Building newBuilding = new Building(new BuildingDefinition());
                Add(newBuilding);
                AddToEntityGroup(newBuilding, room);
            }
        }

        ListWrapper<EntityGroup> FindNeighborRooms(EntityGroup room)
        {

            CellCoordinatesList perimeterCellCoordinatesList =
                room.absoluteCellCoordinatesList.GetPerimeterCellCoordinates();

            ListWrapper<EntityGroup> result = new ListWrapper<EntityGroup>();

            foreach (CellCoordinates cellCoordinates in perimeterCellCoordinatesList.items)
            {
                EntityGroup neighborRoom = appState.EntityGroups.Rooms.FindEntityGroupWithCellsOverlapping(perimeterCellCoordinatesList);

                if (neighborRoom != null)
                {
                    result.Add(neighborRoom);
                }
            }

            return result;
        }
    }
}