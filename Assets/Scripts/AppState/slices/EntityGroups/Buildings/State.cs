using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using TowerBuilder.DataTypes.EntityGroups.Buildings;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
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
            appState.EntityGroups.Rooms.onItemsRemoved += OnRoomsRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.EntityGroups.Rooms.onItemsBuilt -= OnRoomsBuilt;
            appState.EntityGroups.Rooms.onItemsRemoved -= OnRoomsRemoved;
        }

        /*
            Event Handlers
        */
        void OnRoomsBuilt(ListWrapper<EntityGroup> rooms)
        {
            foreach (EntityGroup room in rooms.items)
            {
                EntityGroup parent = FindEntityGroupParent(room);

                if (parent == null)
                {
                    CreateNewBuildingForOrAddToExistingBuilding(room);
                }
            }
        }

        void OnRoomsRemoved(ListWrapper<EntityGroup> deletedRooms)
        {
            ListWrapper<EntityGroup> buildingsToDelete = new ListWrapper<EntityGroup>();

            foreach (EntityGroup room in deletedRooms.items)
            {
                EntityGroup roomParentBuilding = appState.EntityGroups.FindEntityGroupParent(room);

                if (roomParentBuilding != null)
                {
                    roomParentBuilding.childEntityGroups.Remove(room);

                    ListWrapper<EntityGroup> buildingRooms = roomParentBuilding.childEntityGroups;

                    if (buildingRooms.Count == 0 && !buildingsToDelete.Contains(roomParentBuilding))
                    {
                        buildingsToDelete.Add(roomParentBuilding);
                    }
                }
            }

            Remove(buildingsToDelete);
        }

        /*
            Internals
        */
        void CreateNewBuildingForOrAddToExistingBuilding(EntityGroup room)
        {
            ListWrapper<EntityGroup> neighborRooms = FindNeighborRooms(room);

            // TODO - something has gone wrong if these rooms don't have a parent building
            EntityGroup entityGroupWithParent = neighborRooms.Find(neighborRoom => appState.EntityGroups.FindEntityGroupParent(neighborRoom) != null);

            if (entityGroupWithParent != null)
            {
                EntityGroup parentBuilding = appState.EntityGroups.FindEntityGroupParent(entityGroupWithParent);

                AddChild(parentBuilding, room);
            }
            else
            {
                // TODO - validate here that player hasn't reached their building limit
                // Room is the first in the building
                Building newBuilding = new Building(new BuildingDefinition());

                Add(newBuilding);
                AddChild(newBuilding, room);
            }
        }

        ListWrapper<EntityGroup> FindNeighborRooms(EntityGroup room)
        {
            CellCoordinatesList perimeterCellCoordinatesList =
                appState.EntityGroups.GetAbsoluteCellCoordinatesList(room).GetPerimeterCellCoordinates();

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