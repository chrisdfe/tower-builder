using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using UnityEngine;

namespace TowerBuilder.State.UI
{
    public class State
    {
        public struct Input
        {
            public CellCoordinates currentSelectedCell;
        }

        public CellCoordinates currentSelectedCell { get; private set; } = null;

        public delegate void CellCoordinatesEvent(CellCoordinates currentSelectedCell);
        public CellCoordinatesEvent onCurrentSelectedCellUpdated;

        public Room currentSelectedRoom { get; private set; } = null;
        public delegate void selectedRoomEvent(Room room);
        public selectedRoomEvent onCurrentSelectedRoomUpdated;

        public RoomCells currentSelectedRoomBlock { get; private set; } = null;
        public delegate void SelectedRoomBlockEvent(RoomCells roomBlock);
        public SelectedRoomBlockEvent onCurrentSelectedRoomBlockUpdated;

        // public SelectableEntityStack selectableEntityStack { get; private set; } = new SelectableEntityStack();

        public State(Input input)
        {
            currentSelectedCell = input.currentSelectedCell ?? CellCoordinates.zero;
        }

        public State() : this(new Input()) { }

        public void SetCurrentSelectedCell(CellCoordinates currentSelectedCell)
        {
            this.currentSelectedCell = currentSelectedCell;

            currentSelectedRoom = Registry.appState.Rooms.queries.FindRoomAtCell(currentSelectedCell);

            currentSelectedRoomBlock = null;
            if (currentSelectedRoom != null)
            {
                currentSelectedRoomBlock = currentSelectedRoom.FindBlockByCellCoordinates(currentSelectedCell);

            }

            // ToolStateBase currentToolState = GetCurrentActiveToolSubState();
            // currentToolState.OnCurrentSelectedCellUpdated(currentSelectedCell);
            // currentToolState.OnCurrentSelectedRoomUpdated(currentSelectedRoom);
            // currentToolState.OnCurrentSelectedRoomBlockUpdated(currentSelectedRoomBlock);

            if (onCurrentSelectedCellUpdated != null)
            {
                onCurrentSelectedCellUpdated(currentSelectedCell);
            }

            if (onCurrentSelectedRoomUpdated != null)
            {
                onCurrentSelectedRoomUpdated(currentSelectedRoom);
            }

            if (onCurrentSelectedRoomBlockUpdated != null)
            {
                onCurrentSelectedRoomBlockUpdated(currentSelectedRoomBlock);
            }
        }
    }
}

