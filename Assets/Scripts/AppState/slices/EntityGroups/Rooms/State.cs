using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Foundations;
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

        /*
            Internals
        */
        void OnEntitiesBuilt(ListWrapper<Entity> entities)
        {
            foreach (Entity entity in entities.items)
            {
                if (entity.parent == null)
                {
                    CreateNewOrAddToExistingRoom(entity);
                }
            }
        }

        void CreateNewOrAddToExistingRoom(Entity entity)
        {
            if (entity.GetType() == typeof(Foundation))
            {
                // Foundations always create a new room
                AddRoom();
            }
            else
            {
                EntityGroup existingRoom = FindEntityGroupWithCellsOverlapping(entity.absoluteCellCoordinatesList);

                if (existingRoom != null)
                {
                    AddToEntityGroup(existingRoom, entity);
                }
                else
                {
                    AddRoom();
                }
            }

            void AddRoom()
            {
                Room room = new Room(new RoomDefinition());
                Add(room);
                AddToEntityGroup(room, entity);
                Build(room);
            }
        }
    }
}
