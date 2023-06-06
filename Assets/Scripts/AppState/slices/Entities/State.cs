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

        public class Events
        {
            public ListEvent<Entity> onEntitiesAdded;
            public ListEvent<Entity> onEntitiesRemoved;
            public ListEvent<Entity> onEntitiesBuilt;
        }

        public Floors.State Floors { get; }
        public Windows.State Windows { get; }
        public InteriorLights.State InteriorLights { get; }

        public Furnitures.State Furnitures { get; }
        public Residents.State Residents { get; }
        public TransportationItems.State TransportationItems { get; }
        public Freight.State Freight { get; }
        public Wheels.State Wheels { get; }

        public Events events { get; }
        public StateQueries Queries { get; }

        public List<IEntityStateSlice> sliceList { get; }

        public ListWrapper<Entity> allEntities =>
            sliceList.Aggregate(new ListWrapper<Entity>(), (acc, stateSlice) =>
            {
                acc.Add(stateSlice.entityList);
                return acc;
            });

        public State(AppState appState, Input input) : base(appState)
        {
            Floors = new Floors.State(appState, input.Floors);
            Windows = new Windows.State(appState, input.Windows);
            InteriorLights = new InteriorLights.State(appState, input.InteriorLights);
            Furnitures = new Furnitures.State(appState, input.Furnitures);
            Residents = new Residents.State(appState, input.Residents);
            TransportationItems = new TransportationItems.State(appState, input.TransportationItems);
            Freight = new Freight.State(appState, input.Freight);
            Wheels = new Wheels.State(appState, input.Wheels);

            sliceList = new List<IEntityStateSlice>() {
                Floors,
                Windows,
                InteriorLights,
                Furnitures,
                Residents,
                TransportationItems,
                Freight,
                Wheels,
                Windows
            };

            Queries = new StateQueries(this);
            events = new Events();
        }

        public override void Setup()
        {
            sliceList.ForEach(slice =>
            {
                slice.Setup();
                AddListeners(slice);
            });

            void AddListeners(IEntityStateSlice stateSlice)
            {
                stateSlice.entityEvents.onEntitiesAdded += OnEntitiesAdded;
                stateSlice.entityEvents.onEntitiesRemoved += OnEntitiesRemoved;
                stateSlice.entityEvents.onEntitiesBuilt += OnEntitiesBuilt;
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

            void RemoveListeners(IEntityStateSlice stateSlice)
            {
                stateSlice.entityEvents.onEntitiesAdded -= OnEntitiesAdded;
                stateSlice.entityEvents.onEntitiesRemoved -= OnEntitiesRemoved;
                stateSlice.entityEvents.onEntitiesBuilt -= OnEntitiesBuilt;
            }
        }

        public void Add(Entity entity)
        {
            GetStateSlice(entity)?.Add(entity);
        }

        public void Build(Entity entity)
        {
            GetStateSlice(entity)?.Build(entity);
        }

        public void Remove(Entity entity)
        {
            GetStateSlice(entity)?.Remove(entity);
        }

        public IEntityStateSlice GetStateSlice(Entity entity) =>
            entity switch
            {
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

        void OnEntitiesAdded(ListWrapper<Entity> entityList)
        {
            events.onEntitiesAdded?.Invoke(entityList);
        }

        void OnEntitiesRemoved(ListWrapper<Entity> entityList)
        {
            events.onEntitiesRemoved?.Invoke(entityList);
        }

        void OnEntitiesBuilt(ListWrapper<Entity> entityList)
        {
            events.onEntitiesBuilt?.Invoke(entityList);
        }

        public class StateQueries
        {
            State state;

            public StateQueries(State state)
            {
                this.state = state;
            }

            public ListWrapper<Entity> FindEntitiesAtCell(CellCoordinates cellCoordinates) =>
                state.sliceList.Aggregate(new ListWrapper<Entity>(), (acc, slice) =>
                {
                    acc.Add(slice.entityQueries.FindEntitiesAtCell(cellCoordinates));
                    return acc;
                });
        }
    }
}
