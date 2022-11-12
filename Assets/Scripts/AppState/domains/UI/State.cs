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
    public class State : StateSlice
    {
        public class Input
        {
            public CellCoordinates currentSelectedCell;
        }

        public class Events
        {
            public delegate void CellCoordinatesEvent(CellCoordinates currentSelectedCell);
            public CellCoordinatesEvent onCurrentSelectedCellUpdated;

            public delegate void selectedRoomEvent(Room room);
            public selectedRoomEvent onCurrentSelectedRoomUpdated;

            public delegate void SelectedRoomBlockEvent(RoomCells roomBlock);
            public SelectedRoomBlockEvent onCurrentSelectedRoomBlockUpdated;

            public delegate void SelectionBoxEvent(SelectionBox selectionBox);
            public SelectionBoxEvent onSelectionBoxUpdated;
            public SelectionBoxEvent onSelectionStart;
            public SelectionBoxEvent onSelectionEnd;
        }

        public CellCoordinates currentSelectedCell { get; private set; } = null;
        public Room currentSelectedRoom { get; private set; } = null;
        public RoomCells currentSelectedRoomBlock { get; private set; } = null;

        public SelectionBox selectionBox { get; private set; }
        public bool selectionIsActive { get; private set; } = false;
        // public SelectableEntityStack selectableEntityStack { get; private set; } = new SelectableEntityStack();

        public Events events;

        public State(AppState appState, Input input) : base(appState)
        {
            currentSelectedCell = input.currentSelectedCell ?? CellCoordinates.zero;

            selectionBox = new SelectionBox(currentSelectedCell);

            events = new Events();
        }

        public void SetCurrentSelectedCell(CellCoordinates currentSelectedCell)
        {
            this.currentSelectedCell = currentSelectedCell;

            currentSelectedRoom = Registry.appState.Rooms.queries.FindRoomAtCell(currentSelectedCell);

            currentSelectedRoomBlock = null;
            if (currentSelectedRoom != null)
            {
                currentSelectedRoomBlock = currentSelectedRoom.FindBlockByCellCoordinates(currentSelectedCell);
            }

            if (selectionIsActive)
            {
                selectionBox.SetEnd(currentSelectedCell);
            }
            else
            {
                selectionBox.SetStartAndEnd(currentSelectedCell);
            }

            if (events.onCurrentSelectedCellUpdated != null)
            {
                events.onCurrentSelectedCellUpdated(currentSelectedCell);
            }

            if (events.onCurrentSelectedRoomUpdated != null)
            {
                events.onCurrentSelectedRoomUpdated(currentSelectedRoom);
            }

            if (events.onCurrentSelectedRoomBlockUpdated != null)
            {
                events.onCurrentSelectedRoomBlockUpdated(currentSelectedRoomBlock);
            }

            if (events.onSelectionBoxUpdated != null)
            {
                events.onSelectionBoxUpdated(selectionBox);
            }
        }

        public void SelectStart()
        {
            selectionIsActive = true;
            selectionBox.SetStartAndEnd(currentSelectedCell);

            if (events.onSelectionStart != null)
            {
                events.onSelectionStart(selectionBox);
            }
        }

        public void SelectEnd()
        {
            selectionIsActive = false;
            selectionBox.SetEnd(currentSelectedCell);

            if (events.onSelectionEnd != null)
            {
                events.onSelectionEnd(selectionBox);
            }

            ResetSelectionBox();
        }

        void ResetSelectionBox()
        {
            selectionBox = new SelectionBox(currentSelectedCell);

            if (events.onSelectionBoxUpdated != null)
            {
                events.onSelectionBoxUpdated(selectionBox);
            }
        }
    }
}

