using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools.Destroy.Rooms
{
    public class State : DestroyModeStateBase
    {
        public struct Input
        {
        }

        public State(AppState appState, Input input) : base(appState) { }

        public override ListWrapper<Entity> CalculateEntitiesToDelete()
        {
            if (appState.UI.roomsInSelection.Count == 0)
            {
                return new ListWrapper<Entity>();
            }

            List<Entity> entitiesToDelete =
                appState.UI.roomsInSelection.Aggregate(new HashSet<Entity>(), (acc, entityGroup) =>
                {
                    foreach (Entity entity in entityGroup.GetDescendantEntities().items)
                    {
                        acc.Add(entity);
                    }

                    return acc;
                }).ToList();

            return new ListWrapper<Entity>(entitiesToDelete);
        }
    }
}