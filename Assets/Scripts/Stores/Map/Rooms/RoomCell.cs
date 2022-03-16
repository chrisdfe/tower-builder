using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomCell
    {
        public RoomCells roomCells { get; private set; }
        public CellCoordinates coordinates = CellCoordinates.zero;
        public List<RoomEntrance> entrances = new List<RoomEntrance>();
        public List<RoomCellPosition> position = new List<RoomCellPosition>();

        public CellCoordinates relativeCellCoordinates
        {
            get
            {
                return coordinates.Subtract(roomCells.GetTopLeftCoordinates());
            }
        }

        public RoomCell(RoomCells roomCells)
        {
            this.roomCells = roomCells;
        }


        public RoomCell(RoomCells roomCells, int x, int floor) : this(roomCells)
        {
            this.coordinates = new CellCoordinates(x, floor);
        }

        public RoomCell(RoomCells roomCells, CellCoordinates cellCoordinates) : this(roomCells)
        {
            this.coordinates = cellCoordinates.Clone();
        }

        public void Reset()
        {
            SetEntrances();
            SetPosition();
        }

        void SetEntrances()
        {
            List<RoomEntrance> result = new List<RoomEntrance>();

            foreach (RoomEntrance roomEntrance in roomCells.room.entrances)
            {
                // RoomEntrances need a Room instance attached
                roomEntrance.room = roomCells.room;

                if (relativeCellCoordinates.Matches(roomEntrance.cellCoordinates))
                {
                    result.Add(roomEntrance);
                    roomEntrance.roomCell = this;
                }
            }

            entrances = result;
        }

        void SetPosition()
        {
            List<RoomCellPosition> result = new List<RoomCellPosition>();

            if (!roomCells.HasCellAtCoordinates(new CellCoordinates(coordinates.x, coordinates.floor + 1)))
            {
                result.Add(RoomCellPosition.Top);
            }

            if (!roomCells.HasCellAtCoordinates(new CellCoordinates(coordinates.x + 1, coordinates.floor)))
            {
                result.Add(RoomCellPosition.Right);
            }

            if (!roomCells.HasCellAtCoordinates(new CellCoordinates(coordinates.x, coordinates.floor - 1)))
            {
                result.Add(RoomCellPosition.Bottom);
            }

            if (!roomCells.HasCellAtCoordinates(new CellCoordinates(coordinates.x - 1, coordinates.floor)))
            {
                result.Add(RoomCellPosition.Left);
            }

            position = result;
        }
    }
}


