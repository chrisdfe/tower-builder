using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.State;
using TowerBuilder.State.Tools;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldRoom : MonoBehaviour
    {
        public Room room { get; private set; }

        public List<GameWorldRoomCell> gameWorldRoomCells = new List<GameWorldRoomCell>();
        public List<GameWorldRoomEntrance> gameWorldRoomEntrances = new List<GameWorldRoomEntrance>();

        public void SetRoom(Room room)
        {
            this.room = room;
            gameObject.name = $"Room {room.id}";
        }

        public void Initialize()
        {
            CreateRoomCells();
            CreateRoomEntrances();
        }

        public void Teardown()
        {
            DestroyRoomCells();
            DestroyRoomEntrances();
        }

        public void Reset()
        {
            ResetRoomCells();
            ResetRoomEntrances();
        }

        // When this has been converted from a blueprint room to a actual room
        public void OnBuild()
        {
            ResetRoomCells();
            ResetRoomEntrances();
        }

        void Awake()
        {
            transform.localPosition = Vector3.zero;
        }

        void OnDestroy()
        {
            DestroyRoomCells();
            DestroyRoomEntrances();
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
            foreach (RoomCell roomCell in room.cells.cells)
            {
                GameWorldRoomCell gameWorldRoomCell = GameWorldRoomCell.Create(transform);

                gameWorldRoomCell.roomCell = roomCell;
                gameWorldRoomCell.gameWorldRoom = this;
                gameWorldRoomCell.baseColor = room.color;

                gameWorldRoomCell.Initialize();
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
                gameWorldRoomCell.UpdateRoomCellMeshSegments();
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
            RoomCells currentSelectedRoomBlock = Registry.appState.UI.currentSelectedRoomBlock;

            ToolState toolState = Registry.appState.Tools.toolState;

            bool roomContainsCurrentSelectedRoomBlock = room.blocks.ContainsBlock(currentSelectedRoomBlock);
            bool hasUpdated = false;

            switch (toolState)
            {
                case (ToolState.Build):
                    if (room.isInBlueprintMode)
                    {
                        if (room.validator.isValid)
                        {
                            gameWorldRoomCell.SetValidBlueprintColor();
                        }
                        else
                        {
                            gameWorldRoomCell.SetInvalidBlueprintColor();
                        }

                        hasUpdated = true;
                    }
                    break;
                case (ToolState.Destroy):
                    if (currentSelectedRoomBlock != null && currentSelectedRoomBlock.Contains(gameWorldRoomCell.roomCell))
                    {
                        gameWorldRoomCell.SetDestroyHoverColor();
                        hasUpdated = true;
                    }
                    break;
                default:
                    if (room.blocks.ContainsBlock(currentSelectedRoomBlock))
                    {
                        gameWorldRoomCell.SetHoverColor();
                        hasUpdated = true;
                    }
                    break;
            }

            if (!hasUpdated)
            {
                gameWorldRoomCell.SetBaseColor();
            }
        }

        /* 
            Room Entrances
         */
        void CreateRoomEntrances()
        {
            foreach (RoomEntrance roomEntrance in room.entrances)
            {
                GameWorldRoomEntrance gameWorldRoomEntrance = GameWorldRoomEntrance.Create(transform);
                gameWorldRoomEntrance.roomEntrance = roomEntrance;
                gameWorldRoomEntrance.Initialize();
                SetRoomEntranceColor(gameWorldRoomEntrance);
                gameWorldRoomEntrances.Add(gameWorldRoomEntrance);
            }
        }

        void DestroyRoomEntrances()
        {
            foreach (GameWorldRoomEntrance gameWorldRoomEntrance in gameWorldRoomEntrances)
            {
                GameObject.Destroy(gameWorldRoomEntrance.gameObject);
            }

            gameWorldRoomEntrances = new List<GameWorldRoomEntrance>();
        }

        void ResetRoomEntrances()
        {
            DestroyRoomEntrances();
            CreateRoomEntrances();
        }

        void SetRoomEntranceColor(GameWorldRoomEntrance gameWorldRoomEntrance)
        {
            RoomConnection roomEntranceConnection =
                               Registry.appState.Rooms.roomConnections.FindConnectionForRoomEntrance(gameWorldRoomEntrance.roomEntrance);

            if (roomEntranceConnection != null)
            {
                gameWorldRoomEntrance.SetConnectedColor();
            }
            else
            {
                gameWorldRoomEntrance.ResetColor();
            }
        }

        /* 
            Static API
         */
        public static GameWorldRoom Create(Transform parent)
        {
            GameObject roomPrefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/Room");
            GameObject roomGameObject = Instantiate<GameObject>(roomPrefab);

            roomGameObject.transform.parent = parent;

            GameWorldRoom gameWorldRoom = roomGameObject.GetComponent<GameWorldRoom>();
            return gameWorldRoom;
        }
    }
}