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
        public RoomEntrance roomEntrance;
        public GameWorldRoomCell parentGameWorldRoomCell;

        public void Initialize()
        {
            SetPosition();
        }

        void SetPosition()
        {
            float TILE_SIZE = TowerBuilder.Stores.Map.Rooms.Constants.TILE_SIZE;

            if (roomEntrance.position == RoomEntrancePosition.Left)
            {
                // RoomEntrancePosition.Right
                transform.localPosition = new Vector3(
                    // tODO - use parentGameWorldRoomCell instead of transform.parent.transform
                    -(TILE_SIZE / 2) + (transform.parent.transform.localScale.x / 2),
                    -(TILE_SIZE / 2) + (transform.parent.transform.localScale.y / 2),
                    0
                );
            }
            else
            {
                // RoomEntrancePosition.Right
                transform.localPosition = new Vector3(
                    (TILE_SIZE / 2) - (transform.parent.transform.localScale.x / 2),
                    -(TILE_SIZE / 2) + (transform.parent.transform.localScale.y / 2),
                    0
                );
            }
        }
    }
}