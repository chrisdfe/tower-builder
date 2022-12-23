using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.Entities.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.UI
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

            public delegate void SelectedRoomBlockEvent(CellCoordinatesBlock cellCoordinatesBlock);
            public SelectedRoomBlockEvent onCurrentSelectedRoomBlockUpdated;

            public delegate void SelectionBoxEvent(SelectionBox selectionBox);
            public SelectionBoxEvent onSelectionBoxUpdated;
            public SelectionBoxEvent onSelectionBoxReset;
            public SelectionBoxEvent onSelectionStart;
            public SelectionBoxEvent onSelectionEnd;

            public delegate void SelectedCellEntityListEvent(ListWrapper<Entity> entityList);
            public SelectedCellEntityListEvent onCurrentSelectedEntityListUpdated;

            public delegate void ActionEvent();
            public ActionEvent onSecondaryActionPerformed;
        }

        public class Queries { }

        public CellCoordinates currentSelectedCell { get; private set; } = null;
        public Room currentSelectedRoom { get; private set; } = null;
        public CellCoordinatesBlock currentSelectedRoomBlock { get; private set; } = null;
        public Vehicle currentSelectedVehicle { get; private set; } = null;

        public SelectionBox selectionBox { get; private set; }
        public bool selectionIsActive { get; private set; } = false;

        public ListWrapper<Entity> currentSelectedCellEntityList { get; private set; } = new ListWrapper<Entity>();

        public Events events;
        public Queries queries;

        bool altActionIsActive = false;

        public State(AppState appState, Input input) : base(appState)
        {
            currentSelectedCell = input.currentSelectedCell ?? CellCoordinates.zero;

            selectionBox = new SelectionBox(currentSelectedCell);

            events = new Events();
            queries = new Queries();
        }

        public void LeftClickStart()
        {
            if (altActionIsActive)
            {
                PerformSecondaryAction();
            }
            else
            {
                SelectStart();
            }
        }

        public void LeftClickEnd()
        {
            SelectEnd();
        }

        public void AltActionStart()
        {
            altActionIsActive = true;
        }

        public void AltActionEnd()
        {
            altActionIsActive = false;
        }

        public void SetCurrentSelectedCell(CellCoordinates currentSelectedCell)
        {
            this.currentSelectedCell = currentSelectedCell;

            currentSelectedRoom = Registry.appState.Entities.Rooms.queries.FindRoomAtCell(currentSelectedCell);

            currentSelectedRoomBlock = null;
            if (currentSelectedRoom != null)
            {
                currentSelectedRoomBlock = currentSelectedRoom.FindBlockByCellCoordinates(currentSelectedCell);

                currentSelectedVehicle = Registry.appState.Entities.Vehicles.queries.FindVehicleByRoom(currentSelectedRoom);
            }

            if (selectionIsActive)
            {
                selectionBox.SetEnd(currentSelectedCell);
            }
            else
            {
                selectionBox.SetStartAndEnd(currentSelectedCell);
            }

            SetEntityList();

            events.onCurrentSelectedCellUpdated?.Invoke(currentSelectedCell);

            events.onSelectionBoxUpdated?.Invoke(selectionBox);

            events.onCurrentSelectedRoomUpdated?.Invoke(currentSelectedRoom);

            events.onCurrentSelectedRoomBlockUpdated?.Invoke(currentSelectedRoomBlock);
        }

        public void SelectStart()
        {
            selectionIsActive = true;
            selectionBox.SetStartAndEnd(currentSelectedCell);

            events.onSelectionStart?.Invoke(selectionBox);
        }

        public void SelectEnd()
        {
            selectionIsActive = false;
            selectionBox.SetEnd(currentSelectedCell);

            events.onSelectionEnd?.Invoke(selectionBox);

            ResetSelectionBox();
        }

        void ResetSelectionBox()
        {
            selectionBox = new SelectionBox(currentSelectedCell);

            // events.onSelectionBoxUpdated?.Invoke(selectionBox);
            events.onSelectionBoxReset?.Invoke(selectionBox);
        }

        void PerformSecondaryAction()
        {
            events.onSecondaryActionPerformed?.Invoke();
        }

        void SetEntityList()
        {
            if (currentSelectedCell != null)
            {
                currentSelectedCellEntityList = appState.Entities.Queries.FindEntitiesAtCell(currentSelectedCell);
                events.onCurrentSelectedEntityListUpdated?.Invoke(currentSelectedCellEntityList);
            }
        }
    }
}

