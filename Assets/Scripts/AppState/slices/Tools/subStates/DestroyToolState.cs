using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups.Rooms;

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

        public ListWrapper<Room> roomsToDeleteBlocksFrom { get; private set; } = new ListWrapper<Room>();
        public CellCoordinatesBlockList blocksToDelete { get; private set; } = new CellCoordinatesBlockList();
        public CellCoordinatesList cellCoordinatesToDelete { get; private set; } = new CellCoordinatesList();

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

            roomsToDeleteBlocksFrom = new ListWrapper<Room>();
            blocksToDelete = new CellCoordinatesBlockList();
            cellCoordinatesToDelete = new CellCoordinatesList();
        }

        public override void Teardown()
        {
            base.Teardown();

            roomsToDeleteBlocksFrom = new ListWrapper<Room>();
            blocksToDelete = new CellCoordinatesBlockList();
            cellCoordinatesToDelete = new CellCoordinatesList();
        }

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            CalculateDeleteCells();

            events.onDestroySelectionUpdated?.Invoke();
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
            // if (roomsToDeleteBlocksFrom.Count > 0 && blocksToDelete.Count > 0)
            // {
            //     foreach (Room roomToDelete in roomsToDeleteBlocksFrom.items)
            //     {
            //         CellCoordinatesBlockList roomBlocksToDelete =
            //             new CellCoordinatesBlockList(
            //                 blocksToDelete.items.FindAll(roomBlock => roomToDelete.blocksList.Contains(roomBlock))
            //             );
            //         Registry.appState.Entities.Rooms.DestroyRoomBlocks(roomToDelete, roomBlocksToDelete);
            //     }
            // }

            roomsToDeleteBlocksFrom = new ListWrapper<Room>();
            blocksToDelete = new CellCoordinatesBlockList();

            events.onDestroyEnd?.Invoke();
        }

        void CalculateDeleteCells()
        {
            SelectionBox selectionBox = Registry.appState.UI.selectionBox;

            roomsToDeleteBlocksFrom = new ListWrapper<Room>();
            blocksToDelete = new CellCoordinatesBlockList();
            cellCoordinatesToDelete = new CellCoordinatesList();

            // foreach (CellCoordinates cellCoordinates in selectionBox.cellCoordinatesList.items)
            // {
            //     var (roomToDelete, roomBlockToDelete) = Registry.appState.Entities.Rooms.queries.FindRoomBlockAtCell(cellCoordinates);

            //     if (roomToDelete != null && roomBlockToDelete != null)
            //     {
            //         roomsToDeleteBlocksFrom.Add(roomToDelete);
            //         blocksToDelete.Add(roomBlockToDelete);
            //         cellCoordinatesToDelete.Add(roomBlockToDelete.items);
            //     }
            // }
        }
    }
}