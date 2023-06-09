using System;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.EntityGroups
{
    public class EntityGroupStateSlice : StateSlice
    {
        /*
            Events
        */
        public ListEvent<EntityGroup> onEntityGroupsAdded { get; set; }
        public ListEvent<EntityGroup> onEntityGroupsRemoved { get; set; }
        public ListEvent<EntityGroup> onEntityGroupsBuilt { get; set; }

        public ListEvent<EntityGroup> onListUpdated { get; set; }

        public ListWrapper<EntityGroup> list { get; }
        public ListWrapper<EntityGroup> entityGroupList => list.ConvertAll<EntityGroup>();

        public EntityGroupStateSlice(AppState appState) : base(appState)
        {
            list = new ListWrapper<EntityGroup>();
        }

        public void Add(ListWrapper<EntityGroup> newItemsList)
        {
            list.Add(newItemsList);

            onEntityGroupsAdded?.Invoke(newItemsList);
            onListUpdated?.Invoke(list);
        }

        public void Add(EntityGroup entityGroup)
        {
            ListWrapper<EntityGroup> newItemsList = new ListWrapper<EntityGroup>();
            newItemsList.Add(entityGroup);
            Add(newItemsList);
        }

        public void Remove(ListWrapper<EntityGroup> removedItemsList)
        {
            removedItemsList.items.ForEach((entityGroup) =>
            {
                // OnPreDestroy(entityGroup);

                // TODO - validation
                // TODO - add money back into wallet

                // entityGroup.OnDestroy();
            });

            list.Remove(removedItemsList);

            onEntityGroupsRemoved?.Invoke(removedItemsList);
            onListUpdated?.Invoke(list);
        }

        public void Remove(EntityGroup entityGroup)
        {
            ListWrapper<EntityGroup> removedItemsList = new ListWrapper<EntityGroup>(entityGroup);
            Remove(removedItemsList);
        }

        public void Build(EntityGroup entityGroup)
        {
            // if (!entity.validator.isValid)
            // {
            //     // TODO - these should be unique messages - right now they are not
            //     appState.Notifications.Add(entity.validator.errors);
            //     return;
            // }

            // 
            // appState.Wallet.SubtractBalance(entity.price);

            // OnPreBuild(entityGroup);
            // entity.OnBuild();

            ListWrapper<EntityGroup> builtItemsList = new ListWrapper<EntityGroup>();
            builtItemsList.Add(entityGroup);

            onEntityGroupsBuilt?.Invoke(builtItemsList);
        }

        // public EntityGroup FindEntityGroupAtCell(CellCoordinates cellCoordinates) =>
        //     state.list.items
        //         .Find(entity => entity.cellCoordinatesList.Contains(cellCoordinates));


        // public Entity FindEntityAtCell(CellCoordinates cellCoordinates) =>
        //     FindEntityGroupAtCell(cellCoordinates) as Entity;

        // public ListWrapper<EntityGroup> FindEntityGroupsAtCell(CellCoordinates cellCoordinates) =>
        //     new ListWrapper<EntityGroup>(
        //         state.list.items
        //             .FindAll(entity => entity.cellCoordinatesList.Contains(cellCoordinates))
        //     );

        // public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
        //     FindEntityGroupsAtCell(cellCoordinates).ConvertAll<Entity>();

        // public ListWrapper<Entity> FindEntityByType(Type type) =>
        //     new ListWrapper<Entity>(
        //         state.entityList.items.FindAll(entity => entity.GetType() == type)
        //     );
    }
}