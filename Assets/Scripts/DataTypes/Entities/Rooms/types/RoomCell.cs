using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms
{
    public class RoomCell
    {
        public int id = UIDGenerator.Generate("roomCell");

        public CellCoordinates coordinates = CellCoordinates.zero;

        [JsonIgnore]
        public List<RoomCellOrientation> orientation = new List<RoomCellOrientation>();

        public RoomCell(Room room, int x, int floor)
        {
            this.coordinates = new CellCoordinates(x, floor);
        }

        public RoomCell(CellCoordinates cellCoordinates)
        {
            this.coordinates = cellCoordinates.Clone();
        }
    }
}


