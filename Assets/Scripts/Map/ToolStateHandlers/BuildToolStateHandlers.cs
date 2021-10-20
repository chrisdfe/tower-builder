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
        GameObject placeholderTileCube;

        public BuildToolStateHandlers(MapManager parentMapManager) : base(parentMapManager)
        {
            placeholderTileCube = Resources.Load<GameObject>("Prefabs/Map/PlaceholderTileCube");
        }

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

        // void CreatePlaceholderTile(CellCoordinates cellCoordinates)
        // {
        //     GameObject placeholder = UnityEngine.Object.Instantiate<GameObject>(placeholderTileCube);
        //     int currentFocusFloor = Registry.storeRegistry.mapUIStore.state.currentFocusFloor;
        //     float TILE_SIZE = Stores.Map.MapStore.Constants.TILE_SIZE;

        //     placeholder.transform.position = new Vector3(
        //         mapManager.mapCursor.transform.localPosition.x,
        //         (currentFocusFloor * TILE_SIZE) + (TILE_SIZE / 2),
        //         mapManager.mapCursor.transform.localPosition.z
        //     );
        //     placeholder.transform.SetParent(mapManager.buildingWrapper);

        //     PlaceholderTileCube placeholderCube = placeholder.GetComponent<PlaceholderTileCube>();
        // }

        //     void CreatePlaceholderTileOnCurrentFloor()
        //     {
        //         Transform cursor = mapManager.mapCursor.transform;
        //         int tileX = MapCellHelpers.RoundToNearestTile(cursor.position.x);
        //         int tileZ = MapCellHelpers.RoundToNearestTile(cursor.position.z);
        //         int currentFocusFloor = Registry.storeRegistry.mapUIStore.state.currentFocusFloor;

        //         CreatePlaceholderTile(new CellCoordinates()
        //         {
        //             x = tileX,
        //             z = tileZ,
        //             floor = currentFocusFloor
        //         });
        //     }
        // }
    }
}