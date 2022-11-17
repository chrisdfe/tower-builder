using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    public class RoomFurnitureBuilder
    {
        Room room;
        public RoomFurnitureBuilder(Room room)
        {
            this.room = room;
        }

        public FurnitureList BuildFurniture()
        {
            List<Furniture> items = new List<Furniture>();

            /*
            // For now just put one furniture in each cell
            foreach (CellCoordinates cellCoordinates in room.blocks.cells.coordinatesList.items)
            {
                Furniture furniture = new Furniture();
                furniture.room = room;
                furniture.cellCoordinates = cellCoordinates;
                items.Add(furniture);
            }
            */

            return new FurnitureList(items);
        }
    }
}


