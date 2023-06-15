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
            }
        }

        /*
            Events
        */
        public ListEvent<Entity> onEntitiesAdded { get; set; }
        public ListEvent<Entity> onEntitiesRemoved { get; set; }
        public ListEvent<Entity> onEntitiesBuilt { get; set; }

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

        public List<EntityStateSlice> sliceList { get; }

        public ListWrapper<Entity> allEntities =>
            sliceList.Aggregate(new ListWrapper<Entity>(), (acc, stateSlice) =>
            {
                acc.Add(stateSlice.entityList);
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

            sliceList = new List<EntityStateSlice>() {
                Foundations,
                Floors,
                Windows,
                InteriorLights,
                Furnitures,
                Residents,
                TransportationItems,
                Freight,
                Wheels
            };
        }

        /*
            Lifecycle
        */
        public override void Setup()
        {
            sliceList.ForEach(slice =>
            {
                slice.Setup();
                AddListeners(slice);
            });

            void AddListeners(EntityStateSlice stateSlice)
            {
                stateSlice.onItemsAdded += OnEntitiesAdded;
                stateSlice.onItemsRemoved += OnEntitiesRemoved;
                stateSlice.onItemsBuilt += OnEntitiesBuilt;

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
                stateSlice.onItemsAdded -= OnEntitiesAdded;
                stateSlice.onItemsRemoved -= OnEntitiesRemoved;
                stateSlice.onItemsBuilt -= OnEntitiesBuilt;

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

        /*
            TODO
        */
        // This needs to be done individually because entities could include Entities
        // of many different types
        // Ideally I'd group entities by type first and then Add(entities), to cut down
        // on the number of method calls potentially
        public void Add(ListWrapper<Entity> entities)
        {
            foreach (Entity entity in entities.items)
            {
                GetStateSlice(entity)?.Add(entity);
            }
        }

        public void Build(Entity entity)
        {
            GetStateSlice(entity)?.Build(entity);
        }

        public void Remove(Entity entity)
        {
            GetStateSlice(entity)?.Remove(entity);
        }

        public void Remove(ListWrapper<Entity> entities)
        {
            // Warning - this assumes all entities are of the same type
            GetStateSlice(entities.items[0])?.Remove(entities);
        }

        public void UpdateEntityOffsetCoordinates(Entity entity, CellCoordinates offsetCoordinates)
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
                _ => throw new NotSupportedException($"Entity type not handled: {entity.GetType()}")
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
        void OnEntitiesAdded(ListWrapper<Entity> entityList)
        {
            onEntitiesAdded?.Invoke(entityList);
        }

        void OnEntitiesRemoved(ListWrapper<Entity> entityList)
        {
            onEntitiesRemoved?.Invoke(entityList);
        }

        void OnEntitiesBuilt(ListWrapper<Entity> entityList)
        {
            onEntitiesBuilt?.Invoke(entityList);
        }

        void OnEntityPositionUpdated(Entity entity)
        {
            onEntityPositionUpdated?.Invoke(entity);
        }
    }
}
