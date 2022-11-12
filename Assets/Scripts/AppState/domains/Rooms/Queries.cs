using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Buildings;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Validators;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.State.Rooms
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

            public Building FindBuildingByRoom(Room room)
            {
                foreach (Building building in Registry.appState.Buildings.buildingList)
                {
                    if (room.building == building)
                    {
                        return building;
                    }
                }

                return null;
            }

            public RoomList FindRoomsInBuilding(Building building)
            {
                RoomList buildingRooms = new RoomList();

                foreach (Room room in state.roomList.rooms)
                {
                    if (room.building == building)
                    {
                        buildingRooms.Add(room);
                    }
                }

                return buildingRooms;
            }

            public Room FindRoomAtCell(CellCoordinates cellCoordinates)
            {
                return state.roomList.FindRoomAtCell(cellCoordinates);
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
                    foreach (int floor in room.blocks.cells.coordinatesList.GetFloorValues())
                    {
                        Room leftRoom = FindRoomAtCell(new CellCoordinates(
                            room.blocks.cells.coordinatesList.GetLowestX() - 1,
                            floor
                        ));

                        Room rightRoom = FindRoomAtCell(new CellCoordinates(
                            room.blocks.cells.coordinatesList.GetHighestX() + 1,
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
                    foreach (int x in room.blocks.cells.coordinatesList.GetXValues())
                    {
                        Room aboveRoom = FindRoomAtCell(new CellCoordinates(
                            x,
                            room.blocks.cells.coordinatesList.GetHighestFloor() + 1
                        ));

                        Room belowRoom = FindRoomAtCell(new CellCoordinates(
                            x,
                            room.blocks.cells.coordinatesList.GetLowestFloor() - 1
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
