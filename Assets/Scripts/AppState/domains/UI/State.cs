using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Vehicles;
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

            public delegate void SelectedRoomBlockEvent(RoomCells roomBlock);
            public SelectedRoomBlockEvent onCurrentSelectedRoomBlockUpdated;

            public delegate void SelectionBoxEvent(SelectionBox selectionBox);
            public SelectionBoxEvent onSelectionBoxUpdated;
            public SelectionBoxEvent onSelectionStart;
            public SelectionBoxEvent onSelectionEnd;

            public delegate void SelectedCellEntityListEvent(EntityList entityList);
            public SelectedCellEntityListEvent onCurrentSelectedEntityListUpdated;

            public delegate void ActionEvent();
            public ActionEvent onSecondaryActionPerformed;
        }

        public class Queries { }

        public CellCoordinates currentSelectedCell { get; private set; } = null;
        public Room currentSelectedRoom { get; private set; } = null;
        public RoomCells currentSelectedRoomBlock { get; private set; } = null;
        public Vehicle currentSelectedVehicle { get; private set; } = null;

        public SelectionBox selectionBox { get; private set; }
        public bool selectionIsActive { get; private set; } = false;

        public EntityList currentSelectedCellEntityList { get; private set; } = new EntityList();

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

            currentSelectedRoom = Registry.appState.Rooms.queries.FindRoomAtCell(currentSelectedCell);

            currentSelectedRoomBlock = null;
            if (currentSelectedRoom != null)
            {
                currentSelectedRoomBlock = currentSelectedRoom.FindBlockByCellCoordinates(currentSelectedCell);

                currentSelectedVehicle = Registry.appState.Vehicles.queries.FindVehicleByRoom(currentSelectedRoom);
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

        void PerformSecondaryAction()
        {
            Debug.Log("secondary action");

            if (events.onSecondaryActionPerformed != null)
            {
                events.onSecondaryActionPerformed();
            }
        }

        void SetEntityList()
        {
            EntityList entityList = new EntityList();

            if (currentSelectedCell != null)
            {
                if (currentSelectedRoom != null)
                {
                    RoomEntity roomEntity = new RoomEntity(currentSelectedRoom);
                    entityList.Add(roomEntity);
                }

                // if (currentSelectedRoomBlock != null)
                // {
                //     RoomBlockEntity roomBlockEntity = new RoomBlockEntity(currentSelectedRoomBlock);
                //     entityList.Add(roomBlockEntity);
                // }

                Furniture furnitureAtCell = appState.Furnitures.queries.FindFurnitureAtCell(currentSelectedCell);

                if (furnitureAtCell != null)
                {
                    FurnitureEntity furnitureEntity = new FurnitureEntity(furnitureAtCell);
                    entityList.Add(furnitureEntity);
                }

                Resident residentAtCell = appState.Residents.queries.FindResidentAtCell(currentSelectedCell);

                if (residentAtCell != null)
                {
                    ResidentEntity residentEntity = new ResidentEntity(residentAtCell);
                    entityList.Add(residentEntity);
                }
            }

            currentSelectedCellEntityList = entityList;

            if (events.onCurrentSelectedEntityListUpdated != null)
            {
                events.onCurrentSelectedEntityListUpdated(currentSelectedCellEntityList);
            }
        }
    }
}

