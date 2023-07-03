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
            HashSet<Entity> allEntitiesBeneathCursor = new HashSet<Entity>();

            foreach (CellCoordinates cellCoordinates in selectionBox.cellCoordinatesList.items)
            {
                ListWrapper<Entity> entitiesAtCell = Registry.appState.Entities.FindEntitiesAtCell(cellCoordinates);

                foreach (Entity entity in entitiesAtCell.items)
                {
                    allEntitiesBeneathCursor.Add(entity);
                }
            }

            if (allEntitiesBeneathCursor.Count > 0)
            {
                // TODO - sort entities here by z index
                Entity firstEntity = allEntitiesBeneathCursor.ToList()[0];

                // TODO here "single" or "multiple" mode? for now only delete the first entity in the list
                result = new ListWrapper<Entity>(firstEntity);
            }
            else
            {
                result = new ListWrapper<Entity>();
            }

            return result;
        }
    }
}