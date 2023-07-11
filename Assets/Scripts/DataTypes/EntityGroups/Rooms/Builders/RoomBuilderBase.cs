using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class RoomBuilderBase : EntityGroupBuilderBase
    {
        protected Room currentRoom;

        public RoomBuilderBase(EntityGroupDefinition definition) : base(definition) { }

        public override EntityGroup Build(AppState appState)
        {
            currentRoom = new Room();

            BuildFoundation(appState);
            BuildFurniture(appState);
            BuildWindows(appState);
            BuildTransportationItems(appState);

            Debug.Log("current room children:");
            Debug.Log($"{currentRoom.childEntities.Count} entities");
            Debug.Log($"{currentRoom.childEntityGroups.Count} entityGroups");
            return currentRoom;
        }

        protected virtual void BuildFoundation(AppState appState) { }
        protected virtual void BuildFurniture(AppState appState) { }
        protected virtual void BuildWindows(AppState appState) { }
        protected virtual void BuildTransportationItems(AppState appState) { }
    }
}


