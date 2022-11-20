using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;

using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Entrances;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldRoomEntrance : MonoBehaviour
    {
        static Color INSPECTED_COLOR = Color.white;
        static Color CONNECTED_COLOR = Color.green;

        public RoomEntrance roomEntrance;
        // public GameWorldRoomCell parentGameWorldRoomCell;

        Transform cube;
        Material cubeMaterial;
        Color baseColor;

        public void Setup()
        {
            gameObject.name = roomEntrance.ToString();
            SetPosition();
        }

        public void Teardown() { }

        public void SetInspectedColor()
        {
            cubeMaterial.color = INSPECTED_COLOR;
        }

        public void SetConnectedColor()
        {
            cubeMaterial.color = CONNECTED_COLOR;
        }

        public void ResetColor()
        {
            cubeMaterial.color = baseColor;
        }

        void Awake()
        {
            cube = transform.Find("RoomEntranceCube");
            cubeMaterial = cube.GetComponent<Renderer>().material;
            baseColor = cubeMaterial.color;
        }

        void SetPosition()
        {
            float TILE_SIZE = DataTypes.Rooms.Constants.TILE_SIZE;

            transform.localPosition = GameWorldMapCellHelpers.CellCoordinatesToPosition(roomEntrance.cellCoordinates);

            if (roomEntrance.position == RoomEntrancePosition.Left)
            {
                // RoomEntrancePosition.Left
                transform.localPosition += new Vector3(
                    -(TILE_SIZE / 2) + (cube.transform.lossyScale.y / 2),
                    0,
                    0
                );
            }
            else
            {
                // RoomEntrancePosition.Right
                transform.localPosition += new Vector3(
                    (TILE_SIZE / 2) - (cube.transform.lossyScale.y / 2),
                    0,
                    0
                );
            }
        }

        public static GameWorldRoomEntrance Create(Transform parent)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Map/Rooms/RoomEntrance");
            GameObject roomEntranceGameObject = Instantiate<GameObject>(prefab);

            roomEntranceGameObject.transform.parent = parent;

            GameWorldRoomEntrance gameWorldRoomEntrance = roomEntranceGameObject.GetComponent<GameWorldRoomEntrance>();
            return gameWorldRoomEntrance;
        }
    }
}