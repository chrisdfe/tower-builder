using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.Stores.Rooms.Furniture
{
    public enum RoomFurniturePositionPlane
    {
        Floor,
        BackWall,
        LeftWall,
        RightWall,
        Ceiling,
    }

    public enum RoomFurnitureDirection
    {
        Right,
        // Towards camera
        Forward,
        Left,
        Up,
        Down
    }

    public class RoomFurnitureCoordinates
    {
        int x;
        int y;
        int z;

        public RoomFurnitureCoordinates(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class RoomFurniturePosition
    {
        // 5 x 5 x 5 for now
        // TODO - put this somewhere else that makes more sense when I figure out where that is
        static RoomFurnitureCoordinates ROOM_CELL_SUBDIVISIONS = new RoomFurnitureCoordinates(5, 5, 5);

        // The plane this piece of furniture is 'attached' to
        RoomFurniturePositionPlane plane = RoomFurniturePositionPlane.Floor;

        // The direction it is facing (this could probably be gleaned by the plane?)
        RoomFurnitureDirection direction = RoomFurnitureDirection.Forward;

        // the room 'sub cells' that this furniture takes up
        List<RoomFurnitureCoordinates> cells = new List<RoomFurnitureCoordinates>();
    }
}