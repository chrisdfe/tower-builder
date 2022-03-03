using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public partial class RoomCells
    {
        public List<CellCoordinates> cells { get; private set; }

        public RoomCells(List<CellCoordinates> roomCells)
        {
            cells = roomCells;
        }

        public static RoomCells CreateRectangularRoom(int xWidth, int floors)
        {
            List<CellCoordinates> roomCells = new List<CellCoordinates>();

            for (int x = 0; x < xWidth; x++)
            {
                for (int floor = 0; floor < floors; floor++)
                {
                    roomCells.Add(new CellCoordinates()
                    {
                        x = x,
                        // z = z,
                        floor = 0
                    });
                }
            }

            return new RoomCells(roomCells);
        }

        // public static RoomCells createRectangularRoom(int xWidth, int zWidth, int floors) { }

        public static RoomCells PositionAtCoordinates(RoomCells roomCells, CellCoordinates targetCellCoordinates)
        {
            Debug.Log(roomCells);
            Debug.Log(targetCellCoordinates);
            List<CellCoordinates> newCells = new List<CellCoordinates>();

            foreach (CellCoordinates coordinates in roomCells.cells)
            {
                newCells.Add(new CellCoordinates()
                {
                    x = coordinates.x + targetCellCoordinates.x,
                    // z = coordinates.z + targetCellCoordinates.z,
                    floor = coordinates.floor + targetCellCoordinates.floor
                });
            }

            // TODO - does this mutate roomCells outside of the scope of this method?
            roomCells.cells = newCells;
            return roomCells;
        }
    }
}


