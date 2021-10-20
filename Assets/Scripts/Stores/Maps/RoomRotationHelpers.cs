using System;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Map
{
    public static class RoomRotationHelpers
    {
        public static MapRoomRotation GetRightMapRoomRotation(MapRoomRotation rotation)
        {
            if (rotation == MapRoomRotation.Right)
            {
                return MapRoomRotation.Down;
            }

            if (rotation == MapRoomRotation.Down)
            {
                return MapRoomRotation.Left;
            }

            if (rotation == MapRoomRotation.Left)
            {
                return MapRoomRotation.Up;
            }

            // Up
            return MapRoomRotation.Right;
        }

        public static MapRoomRotation GetLeftMapRoomRotation(MapRoomRotation rotation)
        {
            if (rotation == MapRoomRotation.Right)
            {
                return MapRoomRotation.Up;
            }

            if (rotation == MapRoomRotation.Up)
            {
                return MapRoomRotation.Left;
            }

            if (rotation == MapRoomRotation.Left)
            {
                return MapRoomRotation.Down;
            }

            // Down
            return MapRoomRotation.Right;
        }
    }
}