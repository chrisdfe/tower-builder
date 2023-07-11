using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
using UnityEngine;

namespace TowerBuilder.ApplicationState.EntityGroups
{
    public class State : StateSlice
    {
        public class Input
        {
            public Rooms.State.Input Rooms = new Rooms.State.Input();
            public Vehicles.State.Input Vehicles = new Vehicles.State.Input();
            public Buildings.State.Input Buildings = new Buildings.State.Input();
            public Misc.State.Input Misc = new Misc.State.Input();

            public Input()
            {
                Rooms = new Rooms.State.Input();
                Vehicles = new Vehicles.State.Input();
                Buildings = new Buildings.State.Input();
                Misc = new Misc.State.Input();
            }
        }

        /*
            Events
        */
        public ListEvent<EntityGroup> onEntityGroupsAdded { get; set; }
        public ListEvent<EntityGroup> onEntityGroupsRemoved { get; set; }
        public ListEvent<EntityGroup> onEntityGroupsBuilt { get; set; }

        public ItemEvent<EntityGroup> onPositionUpdated { get; set; }

        /*
            State
        */
        public Rooms.State Rooms { get; }
        public Vehicles.State Vehicles { get; }
        public Buildings.State Buildings { get; }
        public Misc.State Misc { get; }

        List<EntityGroupStateSlice> allSlices;

        public State(AppState appState, Input input) : base(appState)
        {
            Rooms = new Rooms.State(appState, input.Rooms);
            Vehicles = new Vehicles.State(appState, input.Vehicles);
            Buildings = new Buildings.State(appState, input.Buildings);
            Misc = new Misc.State(appState, input.Misc);

            allSlices = new List<EntityGroupStateSlice>() {
                Rooms,
                Vehicles,
                Buildings,
                Misc
            };
        }

        /*
            Lifecycle
        */
        public override void Setup()
        {
            base.Setup();

            allSlices.ForEach(slice =>
            {
                slice.Setup();
                AddListeners(slice);
            });

            void AddListeners(EntityGroupStateSlice stateSlice)
            {
                stateSlice.onItemsAdded += OnEntityGroupsAdded;
                stateSlice.onItemsRemoved += OnEntityGroupsRemoved;
                stateSlice.onItemsBuilt += OnEntityGroupsBuilt;

                stateSlice.onPositionUpdated += OnEntityGroupPositionUpdated;
            }
        }

        public override void Teardown()
        {
            base.Teardown();

            allSlices.ForEach(slice =>
            {
                slice.Teardown();
                RemoveListeners(slice);
            });

            void RemoveListeners(EntityGroupStateSlice stateSlice)
            {
                stateSlice.onItemsAdded -= OnEntityGroupsAdded;
                stateSlice.onItemsRemoved -= OnEntityGroupsRemoved;
                stateSlice.onItemsBuilt -= OnEntityGroupsBuilt;

                stateSlice.onPositionUpdated -= OnEntityGroupPositionUpdated;
            }
        }

        /*
            Mutations
        */
        public void Add(EntityGroup entityGroup)
        {
            GetStateSlice(entityGroup)?.Add(entityGroup);
        }

        public void Add(ListWrapper<EntityGroup> entityGroups)
        {
            // TODO - use grouped entitygroups to reduce the number of mutations
            foreach (EntityGroup entityGroup in entityGroups.items)
            {
                Add(entityGroup);
            }
        }

        // Right now it is important that entityGroups get added BEFORE entites
        // to allow entities to calculate their absoluteOffsetPosition
        public void AddWithChildren(EntityGroup entityGroup)
        {
            Add(entityGroup);
            Add(entityGroup.GetDescendantEntityGroups());
            appState.Entities.Add(entityGroup.GetDescendantEntities());
        }

        public void Build(EntityGroup entityGroup)
        {
            GetStateSlice(entityGroup)?.Build(entityGroup);
        }

        public void Build(ListWrapper<EntityGroup> entityGroups)
        {
            // TODO - use groupedEntityGroups
            foreach (EntityGroup entityGroup in entityGroups.items)
            {
                Build(entityGroup);
            }
        }

        public void BuildWithChildren(EntityGroup entityGroup)
        {
            Build(entityGroup);
            Build(entityGroup.GetDescendantEntityGroups());
            appState.Entities.Build(entityGroup.GetDescendantEntities());
        }

        public void Remove(EntityGroup entityGroup)
        {
            GetStateSlice(entityGroup)?.Remove(entityGroup);
        }

        // TODO - use grouped entityGroups
        public void Remove(ListWrapper<EntityGroup> entityGroups)
        {
            foreach (EntityGroup entityGroup in entityGroups.items)
            {
                Remove(entityGroup);
            }
        }

        public void RemoveWithChildren(EntityGroup entityGroup)
        {
            appState.Entities.Remove(entityGroup.GetDescendantEntities());
            Remove(entityGroup.GetDescendantEntityGroups());
            Remove(entityGroup);
        }

        public void SetBlueprintMode(EntityGroup entityGroup, bool isInBlueprintMode)
        {
            SetBlueprintMode(entityGroup.GetDescendantEntityGroups(), isInBlueprintMode);
            entityGroup.SetBlueprintMode(isInBlueprintMode);
        }

        public void SetBlueprintMode(ListWrapper<EntityGroup> entityGroups, bool isInBlueprintMode)
        {
            foreach (EntityGroup entityGroup in entityGroups.items)
            {
                SetBlueprintMode(entityGroup, isInBlueprintMode);
            }
        }

        public void SetBlueprintModeWithChildren(EntityGroup entityGroup, bool isInBlueprintMode)
        {
            SetBlueprintMode(entityGroup, isInBlueprintMode);
            appState.Entities.SetBlueprintMode(entityGroup.GetDescendantEntities(), isInBlueprintMode);
            appState.EntityGroups.SetBlueprintMode(entityGroup.GetDescendantEntityGroups(), isInBlueprintMode);
        }

        public void UpdateOffsetCoordinates(EntityGroup entityGroup, CellCoordinates newOffsetCoordinates)
        {
            GetStateSlice(entityGroup)?.UpdateOffsetCoordinates(entityGroup, newOffsetCoordinates);
        }

        /*
            Queries
        */
        public EntityGroupStateSlice GetStateSlice(EntityGroup entityGroup) =>
            entityGroup switch
            {
                DataTypes.EntityGroups.Rooms.Room => Rooms,
                DataTypes.EntityGroups.Vehicles.Vehicle => Vehicles,
                DataTypes.EntityGroups.Buildings.Building => Buildings,
                _ => Misc
            };

        public ListWrapper<EntityGroup> GetAllEntityGroups() =>
            new ListWrapper<EntityGroup>(
                allSlices.Aggregate(new HashSet<EntityGroup>(), (acc, slice) =>
                {
                    foreach (EntityGroup entityGroup in slice.list.items)
                    {
                        acc.Add(entityGroup);
                    }

                    return acc;
                }).ToList()
            );

        public EntityGroup FindEntityGroupParent(EntityGroup entityGroup)
        {
            foreach (EntityGroupStateSlice slice in allSlices)
            {
                EntityGroup parent = slice.list.items.Find(parentCandidate => parentCandidate.childEntityGroups.items.Contains(entityGroup));

                if (parent != null)
                {
                    return parent;
                }
            }

            return null;
        }

        public EntityGroup FindEntityParent(Entity entity)
        {
            foreach (EntityGroupStateSlice slice in allSlices)
            {
                EntityGroup parent = slice.list.items.Find(parentCandidate => parentCandidate.childEntities.items.Contains(entity));

                if (parent != null)
                {
                    return parent;
                }
            }

            return null;
        }

        public CellCoordinatesBlockList GetAbsoluteBlocksList(Entity entity) =>
            new CellCoordinatesBlockList(
                entity.relativeBlocksList.items
                    .Select(relativeBlock =>
                        new CellCoordinatesBlock(
                            relativeBlock.items
                                .Select((relativeCellCoordinates) =>
                                {
                                    CellCoordinates result = relativeCellCoordinates.Add(entity.relativeOffsetCoordinates);

                                    EntityGroup parent = FindEntityParent(entity);

                                    while (parent != null)
                                    {
                                        result = CellCoordinates.Add(result, parent.relativeOffsetCoordinates);
                                        parent = FindEntityGroupParent(parent);
                                    }

                                    return result;
                                })
                                .ToList()
                        )
                    ).ToList()
            );

        public CellCoordinates GetAbsoluteOffsetCellCoordinates(EntityGroup entityGroup)
        {
            CellCoordinates result = entityGroup.relativeOffsetCoordinates;
            EntityGroup currentParent = FindEntityGroupParent(entityGroup);

            while (currentParent != null)
            {
                result = CellCoordinates.Add(result, currentParent.relativeOffsetCoordinates);
                currentParent = FindEntityGroupParent(currentParent);
            }

            return result;
        }

        public CellCoordinatesList GetAbsoluteCellCoordinatesList(Entity entity) =>
            CellCoordinatesList.FromBlocksList(GetAbsoluteBlocksList(entity));

        // TODO - this should be based off of a GetRelativeCellCoordinates for EntityGroup
        public CellCoordinatesList GetAbsoluteCellCoordinatesList(EntityGroup entityGroup)
        {
            CellCoordinatesList result = new CellCoordinatesList();

            foreach (Entity entity in entityGroup.GetDescendantEntities().items)
            {
                result.Add(appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).items);
            }

            return result;
        }

        public CellCoordinatesBlock FindEntityBlockByCellCoordinates(Entity entity, CellCoordinates cellCoordinates) =>
            GetAbsoluteBlocksList(entity)
                .items.Find(cellCoordinatesBlock => (
                    cellCoordinatesBlock.Contains(cellCoordinates)
                ));

        public ListWrapper<Entity> FindChildEntitiesAtCell(EntityGroup entityGroup, CellCoordinates cellCoordinates) =>
            entityGroup.childEntities.FindAll(entity => GetAbsoluteCellCoordinatesList(entity).Contains(cellCoordinates));

        public EntityGroup FindRoomAtCell(CellCoordinates cellCoordinates) => FindEntityGroupAtCell(cellCoordinates, Rooms);
        public EntityGroup FindBuildingAtCell(CellCoordinates cellCoordinates) => FindEntityGroupAtCell(cellCoordinates, Buildings);
        public EntityGroup FindVehiclesAtCell(CellCoordinates cellCoordinates) => FindEntityGroupAtCell(cellCoordinates, Vehicles);


        /*
            Event Handlers
        */
        void OnEntityGroupsAdded(ListWrapper<EntityGroup> list)
        {
            onEntityGroupsAdded?.Invoke(list);
        }

        void OnEntityGroupsRemoved(ListWrapper<EntityGroup> list)
        {
            onEntityGroupsRemoved?.Invoke(list);
        }

        void OnEntityGroupsBuilt(ListWrapper<EntityGroup> list)
        {
            onEntityGroupsBuilt?.Invoke(list);
        }

        void OnEntityGroupPositionUpdated(EntityGroup entityGroup)
        {
            onPositionUpdated?.Invoke(entityGroup);
        }

        /*
            Internals
        */
        EntityGroup FindEntityGroupAtCell(CellCoordinates cellCoordinates, EntityGroupStateSlice slice)
        {
            ListWrapper<EntityGroup> entityGroupList = new ListWrapper<EntityGroup>();

            foreach (EntityGroup entityGroup in slice.list.items)
            {
                ListWrapper<Entity> entitiesAtCell = FindChildEntitiesAtCell(entityGroup, cellCoordinates);

                if (entitiesAtCell.Count > 0)
                {
                    return entityGroup;
                }
            }

            return null;
        }
    }
}
