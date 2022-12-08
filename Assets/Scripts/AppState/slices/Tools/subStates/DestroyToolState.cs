using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Entities.Rooms;
using TowerBuilder.ApplicationState.UI;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public class DestroyToolState : ToolStateBase
    {
        public struct Input { }

        public class Events
        {
            public delegate void DestroyEvent();
            public DestroyEvent onDestroyStart;
            public DestroyEvent onDestroyEnd;
            public DestroyEvent onDestroySelectionUpdated;
        }

        public Events events { get; private set; }

        // TODO - List<CellCoordinates> is all a room block is, but maybe it should be its own RoomBlock class
        public RoomList roomsToDeleteBlocksFrom { get; private set; } = new RoomList();
        public CellCoordinatesBlockList blocksToDelete { get; private set; } = new CellCoordinatesBlockList();

        bool destroyIsActive = false;

        public CellCoordinatesList cellsToDelete
        {
            get
            {
                List<CellCoordinates> list = new List<CellCoordinates>();

                foreach (CellCoordinatesBlock roomBlock in blocksToDelete.items)
                {
                    list = list.Concat(roomBlock.items).ToList();
                }

                return new CellCoordinatesList(list);
            }
        }

        public DestroyToolState(AppState appState, Tools.State state, Input input) : base(appState, state)
        {
            events = new Events();
        }

        public override void Setup()
        {
            base.Setup();
            roomsToDeleteBlocksFrom = new RoomList();
            blocksToDelete = new CellCoordinatesBlockList();
        }

        public override void Teardown()
        {
            base.Teardown();
            roomsToDeleteBlocksFrom = new RoomList();
            blocksToDelete = new CellCoordinatesBlockList();
        }

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            CalculateDeleteCells();

            if (events.onDestroySelectionUpdated != null)
            {
                events.onDestroySelectionUpdated();
            }
        }

        public override void OnSelectionStart(SelectionBox selectionBox)
        {
            StartDestroy();
        }

        public override void OnSelectionEnd(SelectionBox selectionBox)
        {
            EndDestroy();
        }

        void StartDestroy()
        {
            destroyIsActive = true;
            CalculateDeleteCells();

            if (events.onDestroyStart != null)
            {
                events.onDestroyStart();
            }
        }

        void EndDestroy()
        {
            // This happens when the mouse click up happens outside the screen or over a UI element
            if (!destroyIsActive) return;

            destroyIsActive = false;
            // Restrict destroy to whichever room destroy started on
            if (roomsToDeleteBlocksFrom.Count > 0 && blocksToDelete.Count > 0)
            {
                foreach (Room roomToDelete in roomsToDeleteBlocksFrom.items)
                {
                    CellCoordinatesBlockList roomBlocksToDelete =
                        new CellCoordinatesBlockList(
                            blocksToDelete.items.FindAll(roomBlock => roomToDelete.blocksList.Contains(roomBlock))
                        );
                    Registry.appState.Entities.Rooms.DestroyRoomBlocks(roomToDelete, roomBlocksToDelete);
                }
            }

            roomsToDeleteBlocksFrom = new RoomList();
            blocksToDelete = new CellCoordinatesBlockList();

            if (events.onDestroyEnd != null)
            {
                events.onDestroyEnd();
            }
        }

        void CalculateDeleteCells()
        {
            SelectionBox selectionBox = Registry.appState.UI.selectionBox;

            roomsToDeleteBlocksFrom = new RoomList();
            blocksToDelete = new CellCoordinatesBlockList();

            foreach (CellCoordinates cellCoordinates in selectionBox.cellCoordinatesList.items)
            {
                var (roomToDelete, roomBlockToDelete) = Registry.appState.Entities.Rooms.queries.FindRoomBlockAtCell(cellCoordinates);

                if (roomToDelete != null && roomBlockToDelete != null)
                {
                    roomsToDeleteBlocksFrom.Add(roomToDelete);
                    blocksToDelete.Add(roomBlockToDelete);
                }
            }
        }
    }
}