using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures
{
    public abstract class FurnitureBuilderBase
    {
        public abstract List<Furniture> BuildFurnitureForRoom(Room room);
        public abstract List<Furniture> BuildFurnitureForRoomBlock(Room room, List<RoomCells> roomBlock);
    }
}