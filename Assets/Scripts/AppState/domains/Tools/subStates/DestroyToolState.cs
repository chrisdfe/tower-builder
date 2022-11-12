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
            public DestroyEvent onDestroySelectionUpdated;
        }

        public Events events { get; private set; }

        // TODO - List<CellCoordinates> is all a room block is, but maybe it should be its own RoomBlock class
        public RoomList roomsToDeleteBlocksFrom { get; private set; } = new RoomList();
        public RoomBlocks blocksToDelete { get; private set; } = new RoomBlocks();

        bool destroyIsActive = false;

        public CellCoordinatesList cellsToDelete
        {
            get
            {
                List<CellCoordinates> list = new List<CellCoordinates>();

                foreach (RoomCells roomBlock in blocksToDelete.blocks)
                {
                    list = list.Concat(roomBlock.coordinatesList.items).ToList();
                }

                return new CellCoordinatesList(list);
            }
        }

        public DestroyToolState(Tools.State state, Input input) : base(state)
        {
            events = new Events();
        }

        public override void Setup()
        {
            base.Setup();
            roomsToDeleteBlocksFrom = new RoomList();
            blocksToDelete = new RoomBlocks();
        }

        public override void Teardown()
        {
            base.Teardown();
            roomsToDeleteBlocksFrom = new RoomList();
            blocksToDelete = new RoomBlocks();
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
                foreach (Room roomToDelete in roomsToDeleteBlocksFrom.rooms)
                {
                    RoomBlocks roomBlocksToDelete = new RoomBlocks(blocksToDelete.blocks.FindAll(roomBlock => roomToDelete.blocks.ContainsBlock(roomBlock)));
                    Registry.appState.Rooms.DestroyRoomBlocks(roomToDelete, roomBlocksToDelete);
                }
            }

            roomsToDeleteBlocksFrom = new RoomList();
            blocksToDelete = new RoomBlocks();

            if (events.onDestroyEnd != null)
            {
                events.onDestroyEnd();
            }
        }

        void CalculateDeleteCells()
        {
            SelectionBox selectionBox = Registry.appState.UI.selectionBox;

            roomsToDeleteBlocksFrom = new RoomList();
            blocksToDelete = new RoomBlocks();

            foreach (CellCoordinates cellCoordinates in selectionBox.cellCoordinatesList.items)
            {
                var (roomToDelete, roomBlockToDelete) = Registry.appState.Rooms.queries.FindRoomBlockAtCell(cellCoordinates);

                if (roomToDelete != null && roomBlockToDelete != null)
                {
                    roomsToDeleteBlocksFrom.Add(roomToDelete);
                    blocksToDelete.Add(roomBlockToDelete);
                }
            }
        }
    }
}