using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Rooms
{
    public partial class State
    {
        public class Queries
        {
            State state;
            public Queries(State state)
            {
                this.state = state;
            }

            public Room FindRoomAtCell(CellCoordinates cellCoordinates)
            {
                return state.list.FindRoomAtCell(cellCoordinates);
            }

            public (Room, CellCoordinatesBlock) FindRoomBlockAtCell(CellCoordinates cellCoordinates)
            {
                Room room = FindRoomAtCell(cellCoordinates);

                if (room != null)
                {
                    CellCoordinatesBlock roomBlock = room.FindBlockByCellCoordinates(cellCoordinates);

                    if (roomBlock != null)
                    {
                        return (room, roomBlock);
                    }
                }

                return (null, null);
            }

            public List<Room> FindPerimeterRooms(Room room)
            {
                List<CellCoordinates> perimeterRoomCellCoordinates = room.cellCoordinatesList.GetPerimeterCellCoordinates();
                List<Room> result = new List<Room>();

                foreach (CellCoordinates coordinates in perimeterRoomCellCoordinates)
                {
                    Room perimeterRoom = FindRoomAtCell(coordinates);
                    if (perimeterRoom != null && perimeterRoom != room)
                    {
                        result.Add(perimeterRoom);
                    }
                }

                return result;
            }
        }
    }

}
