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

    public struct RoomFurnitureCoordinates
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
        // The plane this piece of furniture is 'attached' to
        RoomFurniturePositionPlane plane = RoomFurniturePositionPlane.Floor;

        // The direction it is facing (this could probably be gleaned by the plane?)
        RoomFurnitureDirection direction = RoomFurnitureDirection.Forward;

        // the room 'sub cells' that this furniture takes up
        List<RoomFurnitureCoordinates> cells = new List<RoomFurnitureCoordinates>();
    }

    // As the furniture piece is defined in the RoomTemplate
    public class RoomFurnitureDefinition
    {
        RoomFurniturePosition position;
        CellCoordinates coordinates;
    }
}