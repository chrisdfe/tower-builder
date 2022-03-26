using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.Rooms
{
    public class GameWorldRoomEntrance : MonoBehaviour
    {
        static Color CONNECTED_COLOR = Color.green;

        public RoomEntrance roomEntrance;
        // public GameWorldRoomCell parentGameWorldRoomCell;

        Transform cube;
        Material cubeMaterial;
        Color baseColor;

        public void Initialize()
        {
            SetPosition();
        }

        public void SetConnectedColor()
        {
            cubeMaterial.color = CONNECTED_COLOR;
        }

        public void ResetColor()
        {
            // TODO - if room is connected to another entrance GREEN
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
            float TILE_SIZE = TowerBuilder.Stores.Map.Rooms.Constants.TILE_SIZE;

            transform.localPosition = GameWorldMapCellHelpers.CellCoordinatesToPosition(roomEntrance.cellCoordinates);

            // if (roomEntrance.position == RoomEntrancePosition.Left)
            // {
            //     // RoomEntrancePosition.Right
            //     transform.localPosition += new Vector3(
            //         -(TILE_SIZE / 2) + (cube.transform.localScale.x / 2),
            //         -(TILE_SIZE / 2) + (cube.transform.localScale.y / 2),
            //         0
            //     );
            // }
            // else
            // {
            //     // RoomEntrancePosition.Right
            //     transform.localPosition += new Vector3(
            //         (TILE_SIZE / 2) - (cube.transform.localScale.x / 2),
            //         -(TILE_SIZE / 2) + (cube.transform.localScale.y / 2),
            //         0
            //     );
            // }
        }
    }
}