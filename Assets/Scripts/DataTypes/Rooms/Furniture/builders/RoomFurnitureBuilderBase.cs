using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Furniture
{
    public abstract class RoomFurnitureBuilderBase
    {
        public abstract List<RoomFurnitureBase> BuildFurnitureForRoom(Room room);
        public abstract List<RoomFurnitureBase> BuildFurnitureForRoomBlock(Room room, List<RoomCells> roomBlock);
    }
}