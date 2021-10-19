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
                MapRoomRotation rotation = Registry.storeRegistry.mapUIStore.state.currentBlueprintRotation;
                MapRoomRotation nextRotation = MapRoomRotationHelpers.GetRightMapRoomRotation(rotation);
                MapUIStore.Mutations.SetCurrentBlueprintRotation(nextRotation);
            }

            if (Input.GetKeyDown(","))
            {
                MapRoomRotation rotation = Registry.storeRegistry.mapUIStore.state.currentBlueprintRotation;
                MapRoomRotation nextRotation = MapRoomRotationHelpers.GetLeftMapRoomRotation(rotation);
                MapUIStore.Mutations.SetCurrentBlueprintRotation(nextRotation);
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
            CellCoordinates mapRoomCoordinates = Registry.storeRegistry.mapUIStore.state.currentSelectedTile;
            RoomKey mapRoomKey = Registry.storeRegistry.mapUIStore.state.selectedRoomKey;
            MapRoomRotation currentRotation = Registry.storeRegistry.mapUIStore.state.currentBlueprintRotation;

            // TODO - this should already exist at this point

            MapRoomBlueprint blueprint = new MapRoomBlueprint()
            {
                roomKey = mapRoomKey,
                coordinates = mapRoomCoordinates,
                rotation = currentRotation
            };

            MapRoom newRoom = new MapRoom(blueprint);
            MapStore.Mutations.AddRoom(newRoom);
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

        void CreateRoom()
        {

        }
    }
}