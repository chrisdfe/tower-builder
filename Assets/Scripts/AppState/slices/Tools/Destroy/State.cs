using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools.Destroy
{
    public class State : ToolStateBase
    {
        public struct Input { }

        public enum DestroyMode
        {
            EntityBlocks,
            Entities,
            EntityGroups
        }

        public delegate void DestroyEvent();
        public DestroyEvent onDestroyStart;
        public DestroyEvent onDestroyEnd;
        public DestroyEvent onDestroySelectionUpdated;

        // public ListWrapper<Room> roomsToDeleteBlocksFrom { get; private set; } = new ListWrapper<Room>();
        // public CellCoordinatesBlockList blocksToDelete { get; private set; } = new CellCoordinatesBlockList();
        // public CellCoordinatesList cellCoordinatesToDestroy { get; private set; } = new CellCoordinatesList();

        public ListWrapper<Entity> entitiesToDelete = new ListWrapper<Entity>();

        bool destroyIsActive = false;

        // public CellCoordinatesList cellsToDelete
        // {
        //     get
        //     {
        //         List<CellCoordinates> list = new List<CellCoordinates>();

        //         foreach (CellCoordinatesBlock roomBlock in blocksToDelete.items)
        //         {
        //             list = list.Concat(roomBlock.items).ToList();
        //         }

        //         return new CellCoordinatesList(list);
        //     }
        // }

        public State(AppState appState, Tools.State state, Input input) : base(appState, state)
        {
        }

        public override void Setup()
        {
            base.Setup();

            // roomsToDeleteBlocksFrom = new ListWrapper<Room>();
            // blocksToDelete = new CellCoordinatesBlockList();
            // cellCoordinatesToDestroyList = new CellCoordinatesList();
        }

        public override void Teardown()
        {
            base.Teardown();

            // roomsToDeleteBlocksFrom = new ListWrapper<Room>();
            // blocksToDelete = new CellCoordinatesBlockList();
            // cellCoordinatesToDestroyList = new CellCoordinatesList();
        }

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            CalculateDeletion();

            onDestroySelectionUpdated?.Invoke();
        }

        public override void OnSelectionStart(SelectionBox selectionBox)
        {
            StartDestroy();
        }

        public override void OnSelectionEnd(SelectionBox selectionBox)
        {
            EndDestroy();
        }

        public override void OnSecondaryActionEnd()
        {
            base.OnSecondaryActionEnd();
            toolsState.SetToolState(ApplicationState.Tools.State.Key.Inspect);
        }


        /*
            Event Handlers
        */
        void StartDestroy()
        {
            destroyIsActive = true;
            CalculateDeletion();

            if (onDestroyStart != null)
            {
                onDestroyStart();
            }
        }

        void EndDestroy()
        {
            // This happens when the mouse click up happens outside the screen or over a UI element
            if (!destroyIsActive) return;

            foreach (var entity in entitiesToDelete.items)
            {
                // TODO - just pass entitiesToDelete in here instead
                appState.Entities.Remove(entity);
            }

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

            // roomsToDeleteBlocksFrom = new ListWrapper<Room>();
            // blocksToDelete = new CellCoordinatesBlockList();

            onDestroyEnd?.Invoke();
        }

        /*
            Internals
        */
        void CalculateDeletion()
        {
            SelectionBox selectionBox = Registry.appState.UI.selectionBox;

            HashSet<Entity> entities = new HashSet<Entity>();

            // roomsToDeleteBlocksFrom = new ListWrapper<Room>();
            // blocksToDelete = new CellCoordinatesBlockList();
            // cellCoordinatesToDestroyList = new CellCoordinatesList();
            // This will eventually be split up by "DestroyMode" 
            // Lets only deal with Entities for now
            foreach (CellCoordinates cellCoordinates in selectionBox.cellCoordinatesList.items)
            {
                ListWrapper<Entity> entitiesAtCell = Registry.appState.Entities.FindEntitiesAtCell(cellCoordinates);

                foreach (Entity entity in entitiesAtCell.items)
                {
                    entities.Add(entity);
                }
            }


            if (entities.Count > 0)
            {
                // TODO - sort entities here by z index

                Entity firstEntity = entities.ToList()[0];

                // TODO here "single" or "multiple" mode? for now only delete the first entity in the list
                entitiesToDelete = new ListWrapper<Entity>(firstEntity);
            }
            else
            {
                entitiesToDelete = new ListWrapper<Entity>();
            }
        }
    }
}