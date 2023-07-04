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
        public delegate void CellCoordinatesEvent(CellCoordinates selectedCell);
        public CellCoordinatesEvent onSelectedCellUpdated;

        public delegate void EntityBlocksInSelectionEvent(List<CellCoordinatesBlock> blocksList);
        public EntityBlocksInSelectionEvent onEntityBlocksInSelectionUpdated;

        public delegate void EntitiesInSelectionEvent(List<Entity> entityList);
        public EntitiesInSelectionEvent onEntitiesInSelectionUpdated;

        public delegate void EntityGroupsInSelectionEvent(List<EntityGroup> entityGroupList);
        public EntityGroupsInSelectionEvent onEntityGroupsInSelectionUpdated;

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
        public CellCoordinates selectedCell { get; private set; } = null;
        public EntityGroup selectedEntityGroup { get; private set; } = null;

        public SelectionBox selectionBox { get; private set; } = new SelectionBox();
        public List<Entity> entitiesInSelection { get; private set; } = new List<Entity>();
        public List<EntityGroup> entityGroupsInSelection { get; private set; } = new List<EntityGroup>();
        public List<CellCoordinatesBlock> entityBlocksInSelection { get; private set; } = new List<CellCoordinatesBlock>();

        public bool selectionIsActive { get; private set; } = false;

        bool altActionIsActive = false;

        public State(AppState appState, Input input) : base(appState)
        {
            selectedCell = input.currentSelectedCell ?? CellCoordinates.zero;
            selectionBox = new SelectionBox(selectedCell);
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
            onSecondaryActionStart?.Invoke();
        }

        public void RightClickEnd()
        {
            onSecondaryActionEnd?.Invoke();
        }

        public void SetCurrentSelectedCell(CellCoordinates newSelectedCell)
        {
            this.selectedCell = newSelectedCell;

            if (selectionIsActive)
            {
                selectionBox.SetEnd(selectedCell);
            }
            else
            {
                selectionBox.SetStartAndEnd(selectedCell);
            }

            SetEntityList();

            onSelectedCellUpdated?.Invoke(selectedCell);
            onSelectionBoxUpdated?.Invoke(selectionBox);
        }

        public void SelectStart()
        {
            selectionIsActive = true;

            selectionBox.SetStartAndEnd(selectedCell);
            SetEntityList();

            onSelectionStart?.Invoke(selectionBox);
        }

        public void SelectEnd()
        {
            selectionIsActive = false;
            selectionBox.SetEnd(selectedCell);
            SetEntityList();

            onSelectionEnd?.Invoke(selectionBox);

            ResetSelectionBox();
        }

        /*
            Internals
        */
        void ResetSelectionBox()
        {
            selectionBox = new SelectionBox(selectedCell);

            onSelectionBoxReset?.Invoke(selectionBox);
        }

        void SetEntityList()
        {
            // TODO - when is this ever null?
            if (selectedCell == null) return;

            entitiesInSelection =
                selectionBox.cellCoordinatesList.items
                    .Aggregate(new HashSet<Entity>(), (acc, cellCoordinates) =>
                    {
                        ListWrapper<Entity> entitiesAtCell = Registry.appState.Entities.FindEntitiesAtCell(cellCoordinates);

                        foreach (Entity entity in entitiesAtCell.items)
                        {
                            acc.Add(entity);
                        }

                        return acc;
                    })
                    .ToList();

            entityBlocksInSelection =
                entitiesInSelection
                    .Aggregate(new HashSet<CellCoordinatesBlock>(), (acc, entity) =>
                    {
                        CellCoordinatesBlock block = appState.EntityGroups.FindEntityBlockByCellCoordinates(entity, selectedCell);

                        if (block != null)
                        {
                            acc.Add(block);
                        }

                        return acc;
                    })
                    .ToList();

            entityGroupsInSelection =
                entitiesInSelection
                    .Aggregate(new HashSet<EntityGroup>(), (acc, entity) =>
                    {
                        EntityGroup entityGroup = appState.EntityGroups.FindEntityGroupAtCell(selectedCell);

                        if (entityGroup != null)
                        {
                            acc.Add(entityGroup);
                        }

                        return acc;
                    })
                    .ToList();

            onEntitiesInSelectionUpdated?.Invoke(entitiesInSelection);
            onEntityBlocksInSelectionUpdated?.Invoke(entityBlocksInSelection);
            onEntityGroupsInSelectionUpdated?.Invoke(entityGroupsInSelection);
        }
    }
}

