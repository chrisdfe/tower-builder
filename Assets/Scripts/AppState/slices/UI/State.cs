using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using UnityEngine;

namespace TowerBuilder.ApplicationState.UI
{
    public class State : StateSlice
    {
        public class Input
        {
            public CellCoordinates currentSelectedCell;
        }

        /*
            Events
        */
        public delegate void CellCoordinatesEvent(CellCoordinates currentSelectedCell);
        public CellCoordinatesEvent onCurrentSelectedCellUpdated;

        public delegate void SelectedEntityBlockEvent(CellCoordinatesBlockList selectedBlockList);
        public SelectedEntityBlockEvent onSelectedEntityBlocksUpdated;

        public delegate void SelectedCellEntityListEvent(ListWrapper<Entity> entityList);
        public SelectedCellEntityListEvent onCurrentSelectedEntityListUpdated;

        public delegate void SelectionBoxEvent(SelectionBox selectionBox);
        public SelectionBoxEvent onSelectionBoxUpdated;
        public SelectionBoxEvent onSelectionBoxReset;
        public SelectionBoxEvent onSelectionStart;
        public SelectionBoxEvent onSelectionEnd;

        public delegate void ActionEvent();
        public ActionEvent onPrimaryActionStart;
        public ActionEvent onPrimaryActionEnd;
        public ActionEvent onSecondaryActionStart;
        public ActionEvent onSecondaryActionEnd;

        /*
            State
        */
        public CellCoordinates currentSelectedCell { get; private set; } = null;
        public CellCoordinatesBlockList currentSelectedBlockList { get; private set; } = null;
        public Entity currentSelectedEntity { get; private set; } = null;
        public EntityGroup currentSelectedEntityGroup { get; private set; } = null;

        public SelectionBox selectionBox { get; private set; }
        public bool selectionIsActive { get; private set; } = false;

        public ListWrapper<Entity> currentSelectedCellEntityList { get; private set; } = new ListWrapper<Entity>();

        bool altActionIsActive = false;

        public State(AppState appState, Input input) : base(appState)
        {
            currentSelectedCell = input.currentSelectedCell ?? CellCoordinates.zero;

            selectionBox = new SelectionBox(currentSelectedCell);
        }

        /*
            Public Interface
        */
        public void LeftClickStart()
        {
            SelectStart();
        }

        public void LeftClickEnd()
        {
            SelectEnd();
        }

        public void RightClickStart()
        {
            SetEntityList();
            onSecondaryActionStart?.Invoke();
        }

        public void RightClickEnd()
        {
            onSecondaryActionEnd?.Invoke();
        }

        public void SetCurrentSelectedCell(CellCoordinates currentSelectedCell)
        {
            this.currentSelectedCell = currentSelectedCell;
            SetEntityList();

            if (selectionIsActive)
            {
                selectionBox.SetEnd(currentSelectedCell);
            }
            else
            {
                selectionBox.SetStartAndEnd(currentSelectedCell);
            }


            onCurrentSelectedCellUpdated?.Invoke(currentSelectedCell);
            onSelectionBoxUpdated?.Invoke(selectionBox);
        }

        public void SelectStart()
        {
            // SetEntityList();

            selectionIsActive = true;
            selectionBox.SetStartAndEnd(currentSelectedCell);

            onSelectionStart?.Invoke(selectionBox);
        }

        public void SelectEnd()
        {
            selectionIsActive = false;
            selectionBox.SetEnd(currentSelectedCell);

            onSelectionEnd?.Invoke(selectionBox);

            ResetSelectionBox();
        }

        /*
            Internals
        */
        void ResetSelectionBox()
        {
            selectionBox = new SelectionBox(currentSelectedCell);

            onSelectionBoxReset?.Invoke(selectionBox);
        }

        void SetEntityList()
        {
            if (currentSelectedCell != null)
            {
                currentSelectedCellEntityList = appState.Entities.FindEntitiesAtCell(currentSelectedCell);

                currentSelectedBlockList = new CellCoordinatesBlockList(
                    currentSelectedCellEntityList.items
                        .Select(entity => entity.FindBlockByCellCoordinates(currentSelectedCell))
                        .ToList()
                        .FindAll(block => block != null)
                );

                onCurrentSelectedEntityListUpdated?.Invoke(currentSelectedCellEntityList);
                onSelectedEntityBlocksUpdated?.Invoke(currentSelectedBlockList);
            }
        }
    }
}

