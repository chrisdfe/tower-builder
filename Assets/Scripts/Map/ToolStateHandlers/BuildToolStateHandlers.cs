using System;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.UI
{
    public class BuildToolStateHandlers : ToolStateHandlersBase
    {
        public BuildToolStateHandlers(MapManager parentMapManager) : base(parentMapManager) { }

        public override void Update()
        {
            if (Input.GetKeyDown("."))
            {
                // TODO - make this a mutation
                MapRoomRotation rotation = Registry.Stores.MapUI.currentBlueprintRotation;
                MapRoomRotation nextRotation = RoomRotationHelpers.GetRightMapRoomRotation(rotation);
                Registry.Stores.MapUI.SetCurrentBlueprintRotation(nextRotation);
            }

            if (Input.GetKeyDown(","))
            {
                // TODO - make this a mutation
                MapRoomRotation rotation = Registry.Stores.MapUI.currentBlueprintRotation;
                MapRoomRotation nextRotation = RoomRotationHelpers.GetLeftMapRoomRotation(rotation);
                Registry.Stores.MapUI.SetCurrentBlueprintRotation(nextRotation);
            }
        }

        public override void OnTransitionTo(ToolState previousToolState)
        {
            mapManager.mapCursor.Enable();
        }

        public override void OnTransitionFrom(ToolState nextToolState)
        {
            mapManager.mapCursor.Disable();
        }

        public override void OnMouseUp()
        {
            // TODO - move this to CreateRoomAtCurrentSelectedTile
            CellCoordinates mapRoomCoordinates = Registry.Stores.MapUI.currentSelectedTile;
            RoomKey mapRoomKey = Registry.Stores.MapUI.selectedRoomKey;
            MapRoomRotation currentRotation = Registry.Stores.MapUI.currentBlueprintRotation;

            // TODO - this should already exist at this point

            RoomBlueprint blueprint = new RoomBlueprint()
            {
                roomKey = mapRoomKey,
                coordinates = mapRoomCoordinates,
                rotation = currentRotation
            };

            MapRoom newRoom = new MapRoom(blueprint);
            Registry.Stores.Map.AddRoom(newRoom);
        }
    }
}