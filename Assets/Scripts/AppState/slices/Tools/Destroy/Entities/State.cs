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
            if (appState.UI.entitiesInSelection.Count > 0)
            {
                List<Entity> sortedEntities = appState.UI.entitiesInSelection.OrderBy(entity => entity.zIndex).ToList();

                // TODO here "single" or "multiple" mode? for now only delete the last entity in the list/closest to the camera
                Entity entityToDelete = sortedEntities.ToList().Last();

                return new ListWrapper<Entity>(entityToDelete);
            }

            return new ListWrapper<Entity>();
        }
    }
}