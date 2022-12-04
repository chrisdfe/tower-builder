using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Rooms
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

            public (Room, RoomCells) FindRoomBlockAtCell(CellCoordinates cellCoordinates)
            {
                Room room = FindRoomAtCell(cellCoordinates);

                if (room != null)
                {
                    RoomCells roomBlock = room.FindBlockByCellCoordinates(cellCoordinates);

                    if (roomBlock != null)
                    {
                        return (room, roomBlock);
                    }
                }

                return (null, null);
            }

            public List<Room> FindPerimeterRooms(Room room)
            {
                List<CellCoordinates> perimeterRoomCellCoordinates = room.blocks.cells.coordinatesList.GetPerimeterCellCoordinates();
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

            public List<Room> FindRoomsToCombineWith(Room room)
            {
                List<Room> result = new List<Room>();

                switch (room.resizability)
                {
                    case RoomResizability.Flexible:
                        SearchHorizontallyForRooms();
                        SearchVerticallyForRooms();
                        break;
                    case RoomResizability.Horizontal:
                        SearchHorizontallyForRooms();
                        break;
                    case RoomResizability.Vertical:
                        SearchVerticallyForRooms();
                        break;
                    case RoomResizability.Inflexible:
                        break;
                }

                return result;

                void SearchHorizontallyForRooms()
                {
                    //  Check on either side
                    foreach (int floor in room.blocks.cells.coordinatesList.floorValues)
                    {
                        Room leftRoom = FindRoomAtCell(new CellCoordinates(
                            room.blocks.cells.coordinatesList.lowestX - 1,
                            floor
                        ));

                        Room rightRoom = FindRoomAtCell(new CellCoordinates(
                            room.blocks.cells.coordinatesList.highestX + 1,
                            floor
                        ));

                        foreach (Room otherRoom in new Room[] { leftRoom, rightRoom })
                        {
                            if (
                                otherRoom != null &&
                                otherRoom.key == room.key &&
                                !result.Contains(otherRoom)
                            )
                            {
                                result.Add(otherRoom);
                            }
                        }
                    }
                }

                void SearchVerticallyForRooms()
                {
                    //  Check on floors above and below
                    foreach (int x in room.blocks.cells.coordinatesList.xValues)
                    {
                        Room aboveRoom = FindRoomAtCell(new CellCoordinates(
                            x,
                            room.blocks.cells.coordinatesList.highestFloor + 1
                        ));

                        Room belowRoom = FindRoomAtCell(new CellCoordinates(
                            x,
                            room.blocks.cells.coordinatesList.lowestFloor - 1
                        ));

                        foreach (Room otherRoom in new Room[] { aboveRoom, belowRoom })
                        {
                            if (
                                otherRoom != null &&
                                otherRoom.key == room.key &&
                                !result.Contains(otherRoom)
                            )
                            {
                                result.Add(otherRoom);
                            }
                        }
                    }
                }
            }
        }
    }

}
