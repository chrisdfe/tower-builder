using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public partial class MapStore
    {
        public static class Helpers
        {
            public static Room FindRoomAtCell(CellCoordinates targetCellCoordinates, List<Room> rooms)
            {
                foreach (Room room in rooms)
                {
                    foreach (CellCoordinates cellCoordinates in room.roomCells)
                    {
                        if (cellCoordinates.Matches(targetCellCoordinates))
                        {
                            return room;
                        }
                    }
                }

                return null;
            }
        }
    }
}