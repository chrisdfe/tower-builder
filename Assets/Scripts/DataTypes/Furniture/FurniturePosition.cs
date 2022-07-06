using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes.Furniture
{
    public enum FurniturePositionPlane
    {
        Floor,
        BackWall,
        LeftWall,
        RightWall,
        Ceiling,
    }

    public enum FurnitureDirection
    {
        Right,
        // Towards camera
        Forward,
        Left,
        Up,
        Down
    }

    public struct FurnitureCoordinates
    {
        int x;
        int y;
        int z;

        public FurnitureCoordinates(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class FurniturePosition
    {
        // The plane this piece of furniture is 'attached' to
        FurniturePositionPlane plane = FurniturePositionPlane.Floor;

        // The direction it is facing (this could probably be gleaned by the plane?)
        FurnitureDirection direction = FurnitureDirection.Forward;

        // the room 'sub cells' that this furniture takes up
        List<FurnitureCoordinates> cells = new List<FurnitureCoordinates>();
    }

    // As the furniture piece is defined in the RoomTemplate
    public class FurnitureDefinition
    {
        FurniturePosition position;
        CellCoordinates coordinates;
    }
}