using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities
{
    public class State : StateSlice
    {
        public class Input
        {
            public Foundations.State.Input Foundations = new Foundations.State.Input();
            public Furnitures.State.Input Furnitures = new Furnitures.State.Input();
            public Residents.State.Input Residents = new Residents.State.Input();
            public Floors.State.Input Floors;
            public Windows.State.Input Windows;
            public InteriorLights.State.Input InteriorLights;
            public TransportationItems.State.Input TransportationItems;
            public Freight.State.Input Freight;
            public Wheels.State.Input Wheels;
            public Misc.State.Input Misc;

            public Input()
            {
                Foundations = new Foundations.State.Input();
                Floors = new Floors.State.Input();
                Windows = new Windows.State.Input();
                InteriorLights = new InteriorLights.State.Input();
                Furnitures = new Furnitures.State.Input();
                Residents = new Residents.State.Input();
                TransportationItems = new TransportationItems.State.Input();
                Freight = new Freight.State.Input();
                Wheels = new Wheels.State.Input();
                Misc = new Misc.State.Input();
            }
        }

        /*
            Events
        */
        public ListEvent<Entity> onItemsAdded { get; set; }
        public ListEvent<Entity> onItemsRemoved { get; set; }
        public ListEvent<Entity> onItemsBuilt { get; set; }

        public ItemEvent<Entity> onEntityPositionUpdated { get; set; }

        /*
            State
        */
        public Foundations.State Foundations { get; }
        public Floors.State Floors { get; }
        public Windows.State Windows { get; }
        public InteriorLights.State InteriorLights { get; }

        public Furnitures.State Furnitures { get; }
        public Residents.State Residents { get; }
        public TransportationItems.State TransportationItems { get; }
        public Freight.State Freight { get; }
        public Wheels.State Wheels { get; }
        public Misc.State Misc { get; }

        public List<EntityStateSlice> sliceList { get; }

        public ListWrapper<Entity> allEntities =>
            sliceList.Aggregate(new ListWrapper<Entity>(), (acc, stateSlice) =>
            {
                acc.Add(stateSlice.list);
                return acc;
            });

        public State(AppState appState, Input input) : base(appState)
        {
            Foundations = new Foundations.State(appState, input.Foundations);
            Floors = new Floors.State(appState, input.Floors);
            Windows = new Windows.State(appState, input.Windows);
            InteriorLights = new InteriorLights.State(appState, input.InteriorLights);
            Furnitures = new Furnitures.State(appState, input.Furnitures);
            Residents = new Residents.State(appState, input.Residents);
            TransportationItems = new TransportationItems.State(appState, input.TransportationItems);
            Freight = new Freight.State(appState, input.Freight);
            Wheels = new Wheels.State(appState, input.Wheels);
            Misc = new Misc.State(appState, input.Misc);

            sliceList = new List<EntityStateSlice>() {
                Foundations,
                Floors,
                Windows,
                InteriorLights,
                Furnitures,
                Residents,
                TransportationItems,
                Freight,
                Wheels,
                Misc
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

            void AddListeners(EntityStateSlice stateSlice)
            {
                stateSlice.onItemsAdded += OnItemsAdded;
                stateSlice.onItemsRemoved += OnItemsRemoved;
                stateSlice.onItemsBuilt += OnItemsBuilt;

                stateSlice.onEntityPositionUpdated += OnEntityPositionUpdated;
            }
        }

        public override void Teardown()
        {
            base.Teardown();

            sliceList.ForEach((slice) =>
            {
                slice.Teardown();
                RemoveListeners(slice);
            });

            void RemoveListeners(EntityStateSlice stateSlice)
            {
                stateSlice.onItemsAdded -= OnItemsAdded;
                stateSlice.onItemsRemoved -= OnItemsRemoved;
                stateSlice.onItemsBuilt -= OnItemsBuilt;

                stateSlice.onEntityPositionUpdated -= OnEntityPositionUpdated;
            }
        }

        /*
            Public Interface
        */
        public void Add(Entity entity)
        {
            GetStateSlice(entity)?.Add(entity);
        }

        // TODO: Use grouped entities
        public void Add(ListWrapper<Entity> entities)
        {
            foreach (Entity entity in entities.items)
            {
                Add(entity);
            }
        }

        public void Build(Entity entity)
        {
            GetStateSlice(entity)?.Build(entity);
        }

        // TODO: Use gropued entities
        public void Build(ListWrapper<Entity> entities)
        {
            Debug.Log("Building " + entities.Count + " entities");

            foreach (Entity entity in entities.items)
            {
                Build(entity);
            }
        }

        public void Remove(Entity entity)
        {
            GetStateSlice(entity)?.Remove(entity);
        }

        // TODO: use grouped entiites
        public void Remove(ListWrapper<Entity> entities)
        {
            foreach (Entity entity in entities.items)
            {
                Remove(entity);
            }
        }

        public void SetBlueprintMode(Entity entity, bool isInBlueprintMode)
        {
            entity.isInBlueprintMode = isInBlueprintMode;
        }

        public void SetBlueprintMode(ListWrapper<Entity> entities, bool isInBlueprintMode)
        {
            foreach (Entity entity in entities.items)
            {
                SetBlueprintMode(entity, isInBlueprintMode);
            }
        }

        public void UpdateOffsetCoordinates(Entity entity, CellCoordinates offsetCoordinates)
        {
            GetStateSlice(entity)?.UpdateEntityOffsetCoordinates(entity, offsetCoordinates);
        }

        public EntityStateSlice GetStateSlice(Entity entity) =>
            entity switch
            {
                DataTypes.Entities.Foundations.Foundation => Foundations,
                DataTypes.Entities.Floors.Floor => Floors,
                DataTypes.Entities.InteriorLights.InteriorLight => InteriorLights,
                DataTypes.Entities.Furnitures.Furniture => Furnitures,
                DataTypes.Entities.Residents.Resident => Residents,
                DataTypes.Entities.TransportationItems.TransportationItem => TransportationItems,
                DataTypes.Entities.Freights.FreightItem => Freight,
                DataTypes.Entities.Wheels.Wheel => Wheels,
                DataTypes.Entities.Windows.Window => Windows,
                _ => Misc
            };

        /*
            Queries
        */
        public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
            sliceList.Aggregate(new ListWrapper<Entity>(), (acc, slice) =>
            {
                acc.Add(slice.FindEntitiesAtCell(cellCoordinates));
                return acc;
            });

        /*
            Event Handlers
        */
        void OnItemsAdded(ListWrapper<Entity> entityList)
        {
            onItemsAdded?.Invoke(entityList);
        }

        void OnItemsRemoved(ListWrapper<Entity> entityList)
        {
            onItemsRemoved?.Invoke(entityList);
        }

        void OnItemsBuilt(ListWrapper<Entity> entityList)
        {
            Debug.Log("Entities/State OnItemsBuilt: " + entityList.Count);
            onItemsBuilt?.Invoke(entityList);
        }

        void OnEntityPositionUpdated(Entity entity)
        {
            onEntityPositionUpdated?.Invoke(entity);
        }
    }
}
