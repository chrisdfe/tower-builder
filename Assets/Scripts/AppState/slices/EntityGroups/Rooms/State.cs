using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Foundations;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.EntityGroups;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.EntityGroups.Rooms
{
    [Serializable]
    public class State : EntityGroupStateSlice
    {
        public struct Input
        {
            public List<Room> roomList;
        }

        public State(AppState appState, Input input) : base(appState)
        {
        }

        public override void Setup()
        {
            base.Setup();

            appState.Entities.onItemsBuilt += OnEntitiesBuilt;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.onItemsBuilt -= OnEntitiesBuilt;
        }

        public ListWrapper<TransportationItem> FindTransportationItemsEnterableFromRoom(Room room)
        {
            var result = new ListWrapper<TransportationItem>();
            CellCoordinatesList roomCellCoordinates = appState.EntityGroups.GetAbsoluteCellCoordinatesList(room);

            foreach (Entity entity in room.childEntities.items)
            {
                if (entity.GetType() == typeof(TransportationItem) && (entity as TransportationItem).entranceCellCoordinatesList.OverlapsWith(roomCellCoordinates))
                {
                    result.Add(entity as TransportationItem);
                }
            }

            return null;
        }

        /*
            Internals
        */
        void OnEntitiesBuilt(ListWrapper<Entity> entities)
        {
            foreach (Entity entity in entities.items)
            {
                if (appState.EntityGroups.FindEntityParent(entity) == null)
                {
                    CreateNewOrAddToExistingRoom(entity);
                }
            }
        }

        void CreateNewOrAddToExistingRoom(Entity entity)
        {
            if (entity.GetType() == typeof(Foundation))
            {
                // Foundations always create a new room (for now)
                AddRoom();
            }
            else
            {
                CellCoordinatesList absoluteCellCoordinatesList = appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity);
                EntityGroup existingRoom = FindEntityGroupWithCellsOverlapping(absoluteCellCoordinatesList);

                if (existingRoom != null)
                {
                    Debug.Log($"adding {entity} to {existingRoom}");
                    AddChild(existingRoom, entity);
                }
                else
                {
                    AddRoom();
                }
            }

            void AddRoom()
            {
                Room room = new Room(new RoomDefinition());
                room.relativeOffsetCoordinates = entity.relativeCellCoordinatesList.bottomLeftCoordinates;
                Add(room);
                AddChild(room, entity);
                Build(room);
            }
        }
    }
}
