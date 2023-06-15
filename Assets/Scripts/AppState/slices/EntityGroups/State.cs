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

            public Input()
            {
                Rooms = new Rooms.State.Input();
                Vehicles = new Vehicles.State.Input();
                Buildings = new Buildings.State.Input();
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

        List<EntityGroupStateSlice> sliceList;

        public State(AppState appState, Input input) : base(appState)
        {
            Rooms = new Rooms.State(appState, input.Rooms);
            Vehicles = new Vehicles.State(appState, input.Vehicles);
            Buildings = new Buildings.State(appState, input.Buildings);

            sliceList = new List<EntityGroupStateSlice>() {
                Rooms,
                Vehicles,
                Buildings
            };
        }

        /*
            Lifecycle
        */
        public override void Setup()
        {
            base.Setup();

            sliceList.ForEach(slice =>
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

            sliceList.ForEach(slice =>
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
            Public Interface
        */
        public void Add(EntityGroup entityGroup)
        {
            GetStateSlice(entityGroup)?.Add(entityGroup);
        }

        public void Build(EntityGroup entityGroup)
        {
            GetStateSlice(entityGroup)?.Build(entityGroup);
        }

        public void Remove(EntityGroup entityGroup)
        {
            GetStateSlice(entityGroup)?.Remove(entityGroup);
        }

        public void UpdateOffsetCoordinates(EntityGroup entityGroup, CellCoordinates newOffsetCoordinates)
        {
            GetStateSlice(entityGroup)?.UpdateOffsetCoordinates(entityGroup, newOffsetCoordinates);
            Debug.Log("entitygroup offset coordinates updated: ");
            Debug.Log(entityGroup.offsetCoordinates);
        }

        public EntityGroupStateSlice GetStateSlice(EntityGroup entityGroup) =>
            entityGroup switch
            {
                DataTypes.EntityGroups.Rooms.Room => Rooms,
                DataTypes.EntityGroups.Vehicles.Vehicle => Vehicles,
                DataTypes.EntityGroups.Buildings.Building => Buildings,
                _ => throw new NotSupportedException($"EntityGroup type not handled: {entityGroup.GetType()}")
            };

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
    }
}
