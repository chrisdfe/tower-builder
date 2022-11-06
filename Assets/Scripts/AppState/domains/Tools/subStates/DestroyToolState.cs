using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.State.Tools
{
    public class DestroyToolState : ToolStateBase
    {
        public struct Input { }

        public class Events
        {
            public delegate void DestroyEvent();
            public DestroyEvent onDestroyStart;
            public DestroyEvent onDestroyEnd;
        }

        public Events events { get; private set; }

        // TODO - List<CellCoordinates> is all a room block is, but maybe it should be its own RoomBlock class
        public Room roomToDeleteBlocksFrom { get; private set; }
        public RoomBlocks blocksToDelete { get; private set; }

        public DestroyToolState(Tools.State state, Input input) : base(state)
        {
            events = new Events();
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Teardown()
        {
            base.Teardown();
        }

        protected override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            if (Registry.appState.UI.selectionIsActive)
            {
                CalculateDeleteCells();
            }
        }

        protected override void OnSelectionStart(SelectionBox selectionBox)
        {
            StartDestroy();
        }

        protected override void OnSelectionEnd(SelectionBox selectionBox)
        {
            EndDestroy();
        }

        void StartDestroy()
        {
            if (events.onDestroyStart != null)
            {
                events.onDestroyStart();
            }
        }

        void EndDestroy()
        {
            // Restrict destroy to whichever room destroy started on
            if (roomToDeleteBlocksFrom != null && blocksToDelete.Count > 0)
            {
                Registry.appState.Rooms.DestroyRoomBlocks(roomToDeleteBlocksFrom, blocksToDelete);
            }

            roomToDeleteBlocksFrom = null;
            blocksToDelete = new RoomBlocks();

            if (events.onDestroyEnd != null)
            {
                events.onDestroyEnd();
            }
        }

        void CalculateDeleteCells()
        {
            SelectionBox selectionBox = Registry.appState.UI.selectionBox;

            blocksToDelete = new RoomBlocks();

            CellCoordinates endCell = selectionBox.end;
            roomToDeleteBlocksFrom = Registry.appState.Rooms.queries.FindRoomAtCell(endCell);

            if (roomToDeleteBlocksFrom != null)
            {
                // Just for testing
                // blocksToDelete = roomToDeleteBlocksFrom.blocks;
                blocksToDelete.Set(roomToDeleteBlocksFrom.FindBlockByCellCoordinates(selectionBox.end));
                Debug.Log("blocksToDelete.Count");
                Debug.Log(blocksToDelete.Count);
            }

            /* // Determine which blocks in that room to destroy:
            if (startCellRoom.resizability.Matches(RoomResizability.Horizontal))
            {
                // TODO - account for double tall rooms
                cellsToDelete = selectionBox.GetCells().FindAll(cellCoordinates => cellCoordinates.floor == startCellRoom.bottomLeftCoordinates.floor).ToList();
            }
            else if (startCellRoom.resizability.Matches(RoomResizability.Vertical))
            {
                cellsToDelete = selectionBox.GetCells().FindAll(cellCoordinates => cellCoordinates.floor == startCellRoom.bottomLeftCoordinates.x).ToList();
            }
            else if (startCellRoom.resizability.Matches(RoomResizability.Flexible))
            {
                // Find overlap between cells
            }
            else
            {
                // Just delete whole room
            } */
        }

        // public void DestroyCurrentSelectedRoomBlock()
        // {
        //     if (Registry.appState.UI.currentSelectedRoom != null)
        //     {
        //         Registry.appState.Rooms.DestroyRoomBlock(Registry.appState.UI.currentSelectedRoom, Registry.appState.UI.currentSelectedRoomBlock);
        //     }
        // }
    }
}