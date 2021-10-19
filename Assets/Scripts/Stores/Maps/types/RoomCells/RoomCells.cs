using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    public partial class RoomCells
    {
        public List<CellCoordinates> cells { get; private set; }

        public RoomCells(List<CellCoordinates> roomCells)
        {
            cells = roomCells;
        }

        public static RoomCells CreateRectangularRoom(int xWidth, int zWidth)
        {
            List<CellCoordinates> roomCells = new List<CellCoordinates>();

            for (int x = 0; x < xWidth; x++)
            {
                for (int z = 0; z < zWidth; z++)
                {
                    roomCells.Add(new CellCoordinates()
                    {
                        x = x,
                        z = z,
                        floor = 0
                    });
                }
            }

            return new RoomCells(roomCells);
        }

        // public static RoomCells createRectangularRoom(int xWidth, int zWidth, int floors) { }

        public static RoomCells PositionAtCoordinates(RoomCells roomCells, CellCoordinates targetCellCoordinates)
        {
            List<CellCoordinates> newCells = new List<CellCoordinates>();

            foreach (CellCoordinates coordinates in roomCells.cells)
            {
                newCells.Add(new CellCoordinates()
                {
                    x = coordinates.x + targetCellCoordinates.x,
                    z = coordinates.z + targetCellCoordinates.z,
                    floor = coordinates.floor + targetCellCoordinates.floor
                });
            }

            // TODO - does this mutate roomCells outside of the scope of this method?
            roomCells.cells = newCells;
            return roomCells;
        }

        public static RoomCells Rotate(RoomCells roomCells, MapRoomRotation rotation)
        {
            List<CellCoordinates> newCells = new List<CellCoordinates>();
            // CellCoordinates2D rotationValues = MapStore.Constants.MAP_ROOM_ROTATION_VALUES[rotation];

            foreach (CellCoordinates coordinates in roomCells.cells)
            {
                CellCoordinates rotatedCellCoordinates;
                switch (rotation)
                {
                    case MapRoomRotation.Right:
                        // Rooms are laid out this way by defualt
                        rotatedCellCoordinates = coordinates;
                        break;
                    case MapRoomRotation.Down:
                        rotatedCellCoordinates = new CellCoordinates()
                        {
                            x = coordinates.z * -1,
                            z = coordinates.x * -1,
                            floor = coordinates.floor
                        };
                        break;
                    case MapRoomRotation.Left:
                        rotatedCellCoordinates = new CellCoordinates()
                        {
                            x = coordinates.x * -1,
                            z = coordinates.z * -1,
                            floor = coordinates.floor
                        };
                        break;
                    case MapRoomRotation.Up:
                        rotatedCellCoordinates = new CellCoordinates()
                        {
                            x = coordinates.z,
                            z = coordinates.x,
                            floor = coordinates.floor
                        };
                        break;
                    default:
                        rotatedCellCoordinates = new CellCoordinates()
                        {
                            x = coordinates.z,
                            z = coordinates.x,
                            floor = coordinates.floor
                        };
                        break;
                }
                newCells.Add(rotatedCellCoordinates);
            }

            // TODO - does this mutate roomCells outside of the scope of this method?
            roomCells.cells = newCells;
            return roomCells;
        }
    }
}


