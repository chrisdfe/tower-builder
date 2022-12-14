using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Rooms
{
    public class GameWorldRoomList : MonoBehaviour
    {
        [HideInInspector]
        public List<GameWorldRoom> gameWorldRoomList = new List<GameWorldRoom>();

        void Awake()
        {
            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            Registry.appState.Entities.Rooms.events.onItemsAdded += OnRoomsAdded;
            Registry.appState.Entities.Rooms.events.onItemsBuilt += OnRoomsBuilt;
            Registry.appState.Entities.Rooms.events.onItemsRemoved += OnRoomsRemoved;
            Registry.appState.Entities.Rooms.events.onRoomBlocksUpdated += OnRoomBlocksUpdated;

            Registry.appState.UI.events.onCurrentSelectedRoomUpdated += OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.events.onCurrentSelectedRoomBlockUpdated += OnCurrentSelectedRoomBlockUpdated;

            Registry.appState.Tools.destroyToolState.events.onDestroySelectionUpdated += OnDestroySelectionUpdated;
            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated += OnCurrentSelectedEntityUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Entities.Rooms.events.onItemsAdded -= OnRoomsAdded;
            Registry.appState.Entities.Rooms.events.onItemsBuilt -= OnRoomsBuilt;
            Registry.appState.Entities.Rooms.events.onItemsRemoved -= OnRoomsRemoved;
            Registry.appState.Entities.Rooms.events.onRoomBlocksUpdated -= OnRoomBlocksUpdated;

            Registry.appState.UI.events.onCurrentSelectedRoomUpdated -= OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.events.onCurrentSelectedRoomBlockUpdated -= OnCurrentSelectedRoomBlockUpdated;

            Registry.appState.Tools.destroyToolState.events.onDestroySelectionUpdated -= OnDestroySelectionUpdated;
            Registry.appState.Tools.inspectToolState.events.onCurrentSelectedEntityUpdated -= OnCurrentSelectedEntityUpdated;
        }

        /*
         * Rooms - Internals
         */
        GameWorldRoom FindGameWorldRoomByRoom(Room room) =>
            gameWorldRoomList.Find(gwr => gwr.room == room);

        void CreateRoom(Room room)
        {
            GameWorldRoom gameWorldRoom = GameWorldRoom.Create(transform);
            gameWorldRoom.SetRoom(room);
            gameWorldRoom.Setup();
            gameWorldRoomList.Add(gameWorldRoom);
        }

        void RemoveRoom(Room room)
        {
            GameWorldRoom gameWorldRoom = gameWorldRoomList.Find(otherRoom => otherRoom.room == room);
            gameWorldRoomList.Remove(gameWorldRoom);
            Destroy(gameWorldRoom.gameObject);
        }

        void UpdateRoomColors()
        {
            foreach (GameWorldRoom gameWorldRoom in gameWorldRoomList)
            {
                UpdateRoomColor(gameWorldRoom);
            }
        }

        void UpdateRoomColor(GameWorldRoom gameWorldRoom)
        {
            Entity inspectedEntity = Registry.appState.Tools.inspectToolState.inspectedEntity;
            Room room = gameWorldRoom.room;
            ToolState toolState = Registry.appState.Tools.toolState;

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
                gameWorldRoom.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Default);
            }

            void SetBuildStateColor()
            {
                if (room.isInBlueprintMode)
                {
                    if (room.validator.isValid)
                    {
                        gameWorldRoom.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.ValidBlueprint);
                    }
                    else
                    {
                        gameWorldRoom.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.InvalidBlueprint);
                    }

                    hasUpdated = true;
                }
            }

            void SetDestroyStateColor()
            {
                // not supported yet
            }

            void SetInspectStateColor()
            {
                if ((inspectedEntity is Room) && ((Room)inspectedEntity) == room)
                {
                    gameWorldRoom.GetComponent<EntityMeshWrapper>().SetColor(EntityMeshWrapper.ColorKey.Inspected);
                    hasUpdated = true;
                }
            }
        }

        /* 
         * Event Handlers
         */
        void OnRoomsAdded(RoomList roomList)
        {
            roomList.ForEach(room =>
            {
                CreateRoom(room);
            });
        }

        void OnRoomsBuilt(RoomList roomList)
        {
            roomList.ForEach(room =>
            {
                GameWorldRoom gameWorldRoom = FindGameWorldRoomByRoom(room);
                if (gameWorldRoom == null) return;
                gameWorldRoom.OnBuild();
            });
        }

        void OnRoomsRemoved(RoomList roomList)
        {
            roomList.ForEach(room =>
            {
                RemoveRoom(room);
            });
        }

        void OnRoomBlocksUpdated(Room room, CellCoordinatesBlockList blocks)
        {
            GameWorldRoom gameWorldRoom = FindGameWorldRoomByRoom(room);

            if (gameWorldRoom != null)
            {
                gameWorldRoom.Reset();
            }
        }

        void OnCurrentSelectedRoomUpdated(Room selectedRoom)
        {
            UpdateRoomColors();
        }

        void OnCurrentSelectedRoomBlockUpdated(CellCoordinatesBlock roomBlock) { }

        void OnInspectRoomUpdated(Room currentInspectedRoom) { }

        void OnDestroySelectionUpdated()
        {
            UpdateRoomColors();
        }

        void OnCurrentSelectedEntityUpdated(Entity entity)
        {
            UpdateRoomColors();
        }
    }
}