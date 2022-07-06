using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furniture
{
    public abstract class FurnitureBuilderBase
    {
        public abstract List<FurnitureBase> BuildFurnitureForRoom(Room room);
        public abstract List<FurnitureBase> BuildFurnitureForRoomBlock(Room room, List<RoomCells> roomBlock);
    }
}