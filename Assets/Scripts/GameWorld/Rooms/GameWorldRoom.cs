using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldRoom : MonoBehaviour
    {
        public Room room { get; private set; }

        public List<GameWorldRoomCell> gameWorldRoomCells = new List<GameWorldRoomCell>();

        /*
            Lifecycle Methods
        */
        void Awake()
        {
            transform.localPosition = Vector3.zero;
        }

        void OnDestroy()
        {
            DestroyRoomCells();
        }

        public void Setup()
        {
            CreateRoomCells();
        }

        public void Teardown()
        {
            DestroyRoomCells();
        }

        public void Reset()
        {
            ResetRoomCells();
        }

        // When this has been converted from a blueprint room to a actual room
        public void OnBuild()
        {
            ResetRoomCells();
        }

        /* 
            Public Interface
        */
        public void SetRoom(Room room)
        {
            this.room = room;
            gameObject.name = $"Room {room.id}";
        }

        /* 
            Room Cells
         */
        public void SetRoomCellColors()
        {
            foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoomCells)
            {
                SetRoomCellColor(gameWorldRoomCell);
            }
        }

        void CreateRoomCells()
        {
            foreach (CellCoordinates cellCoordinates in room.cellCoordinatesList.items)
            {
                GameWorldRoomCell gameWorldRoomCell = GameWorldRoomCell.Create(transform);

                gameWorldRoomCell.room = room;
                gameWorldRoomCell.cellCoordinates = cellCoordinates;
                gameWorldRoomCell.cellCoordinatesList = room.cellCoordinatesList;
                gameWorldRoomCell.cellPosition = room.cellPositionMap[cellCoordinates];
                gameWorldRoomCell.gameWorldRoom = this;

                gameWorldRoomCell.Setup();
                SetRoomCellColor(gameWorldRoomCell);
                gameWorldRoomCells.Add(gameWorldRoomCell);
            }
        }

        void DestroyRoomCells()
        {
            foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoomCells)
            {
                Destroy(gameWorldRoomCell.gameObject);
            }

            gameWorldRoomCells = new List<GameWorldRoomCell>();
        }

        void UpdateRoomCells()
        {
            foreach (GameWorldRoomCell gameWorldRoomCell in gameWorldRoomCells)
            {
                gameWorldRoomCell.UpdateMesh();
            }
        }

        void ResetRoomCells()
        {
            DestroyRoomCells();
            CreateRoomCells();
        }

        void SetRoomCellColor(GameWorldRoomCell gameWorldRoomCell)
        {
            CellCoordinates currentSelectedCell = Registry.appState.UI.currentSelectedCell;
            Room currentSelectedRoom = Registry.appState.UI.currentSelectedRoom;
            CellCoordinatesBlock currentSelectedRoomBlock = Registry.appState.UI.currentSelectedRoomBlock;

            ToolState toolState = Registry.appState.Tools.toolState;

            bool roomContainsCurrentSelectedRoomBlock = room.blocksList.Contains(currentSelectedRoomBlock);
            bool hasUpdated = false;

            switch (toolState)
            {
                case (ToolState.Build):
                    SetBuildStateColor();
                    break;
                case (ToolState.Destroy):
                    SetDestroyStateColor();
                    break;
                default:
                    SetInspectStateColor();
                    break;
            }

            if (!hasUpdated)
            {
                gameWorldRoomCell.SetColor(GameWorldRoomCell.ColorKey.Base);
            }

            void SetBuildStateColor()
            {
                if (room.isInBlueprintMode)
                {
                    if (room.validator.isValid)
                    {
                        gameWorldRoomCell.SetColor(GameWorldRoomCell.ColorKey.ValidBlueprint);
                    }
                    else
                    {
                        gameWorldRoomCell.SetColor(GameWorldRoomCell.ColorKey.InvalidBlueprint);
                    }

                    hasUpdated = true;
                }
            }

            void SetDestroyStateColor()
            {
                CellCoordinatesList cellsToDestroy = Registry.appState.Tools.destroyToolState.cellsToDelete;
                if (cellsToDestroy.items.Contains(gameWorldRoomCell.cellCoordinates))
                {
                    gameWorldRoomCell.SetColor(GameWorldRoomCell.ColorKey.Destroy);
                    hasUpdated = true;
                }
            }

            void SetInspectStateColor()
            {
                EntityList inspectedEntityList = Registry.appState.Tools.inspectToolState.inspectedEntityList;
                Entity inspectedEntity = Registry.appState.Tools.inspectToolState.inspectedEntity;

                if (inspectedEntity != null)
                {
                    if ((inspectedEntity is Room) && ((Room)inspectedEntity) == room)
                    {
                        gameWorldRoomCell.SetColor(GameWorldRoomCell.ColorKey.Inspected);
                        hasUpdated = true;
                    }
                }
            }
        }

        /* 
            Static API
         */
        public static GameWorldRoom Create(Transform parent)
        {
            GameWorldRoomsManager roomsManager = GameWorldRoomsManager.Find();
            GameObject prefab = roomsManager.prefabAssets.FindByKey(GameWorldRoomsManager.AssetKey.Room);
            GameObject roomGameObject = Instantiate<GameObject>(prefab);

            roomGameObject.transform.parent = parent;

            GameWorldRoom gameWorldRoom = roomGameObject.GetComponent<GameWorldRoom>();
            return gameWorldRoom;
        }
    }
}