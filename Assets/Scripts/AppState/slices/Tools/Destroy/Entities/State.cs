using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.ApplicationState.Tools.Destroy.Entities
{
    public class State : DestroyModeStateBase
    {
        public struct Input
        {
        }

        public State(AppState appState, Input input) : base(appState) { }

        public override ListWrapper<Entity> CalculateEntitiesToDelete()
        {
            ListWrapper<Entity> result = new ListWrapper<Entity>();
            SelectionBox selectionBox = Registry.appState.UI.selectionBox;
            HashSet<Entity> allEntitiesInSelectionBox = new HashSet<Entity>();

            foreach (CellCoordinates cellCoordinates in selectionBox.cellCoordinatesList.items)
            {
                ListWrapper<Entity> entitiesAtCell = Registry.appState.Entities.FindEntitiesAtCell(cellCoordinates);

                foreach (Entity entity in entitiesAtCell.items)
                {
                    allEntitiesInSelectionBox.Add(entity);
                }
            }

            if (allEntitiesInSelectionBox.Count > 0)
            {
                List<Entity> sortedEntities = allEntitiesInSelectionBox.OrderBy(entity => entity.zIndex).ToList();

                // TODO here "single" or "multiple" mode? for now only delete the first entity in the list
                // TODO - [0] or last item?
                Entity entityToDelete = allEntitiesInSelectionBox.ToList().Last();

                result = new ListWrapper<Entity>(entityToDelete);
            }
            else
            {
                result = new ListWrapper<Entity>();
            }

            return result;
        }
    }
}