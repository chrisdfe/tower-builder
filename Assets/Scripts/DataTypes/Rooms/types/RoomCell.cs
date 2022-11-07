using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Newtonsoft.Json;
using TowerBuilder.DataTypes.Rooms.Entrances;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    public class RoomCell
    {
        public int id = UIDGenerator.Generate("roomCell");

        public CellCoordinates coordinates = CellCoordinates.zero;

        [JsonIgnore]
        public List<RoomCellOrientation> orientation = new List<RoomCellOrientation>();

        public RoomCell(int x, int floor)
        {
            this.coordinates = new CellCoordinates(x, floor);
        }

        public RoomCell(CellCoordinates cellCoordinates)
        {
            this.coordinates = cellCoordinates.Clone();
        }
    }
}


