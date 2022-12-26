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
            public Rooms.State.Input Rooms;
            public Floors.State.Input Floors;
            public Windows.State.Input Windows;
            public InteriorWalls.State.Input InteriorWalls;
            public InteriorLights.State.Input InteriorLights;
            public TransportationItems.State.Input TransportationItems;
            public Freight.State.Input Freight;
            public Wheels.State.Input Wheels;
            public Vehicles.State.Input Vehicles;

            public Input()
            {
                Rooms = new Rooms.State.Input();
                Floors = new Floors.State.Input();
                Windows = new Windows.State.Input();
                InteriorWalls = new InteriorWalls.State.Input();
                InteriorLights = new InteriorLights.State.Input();
                Furnitures = new Furnitures.State.Input();
                Residents = new Residents.State.Input();
                TransportationItems = new TransportationItems.State.Input();
                Freight = new Freight.State.Input();
                Wheels = new Wheels.State.Input();
                Vehicles = new Vehicles.State.Input();
                Windows = new Windows.State.Input();
            }
        }

        public class Events
        {
            public ListEvent<Entity> onEntitiesAdded;
            public ListEvent<Entity> onEntitiesRemoved;
            public ListEvent<Entity> onEntitiesBuilt;
        }

        public Rooms.State Rooms { get; }
        public Floors.State Floors { get; }
        public Windows.State Windows { get; }
        public InteriorWalls.State InteriorWalls { get; }
        public InteriorLights.State InteriorLights { get; }

        public Furnitures.State Furnitures { get; }
        public Residents.State Residents { get; }
        public TransportationItems.State TransportationItems { get; }
        public Freight.State Freight { get; }
        public Wheels.State Wheels { get; }
        public Vehicles.State Vehicles { get; }

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
            Rooms = new Rooms.State(appState, input.Rooms);
            Floors = new Floors.State(appState, input.Floors);
            Windows = new Windows.State(appState, input.Windows);
            InteriorWalls = new InteriorWalls.State(appState, input.InteriorWalls);
            InteriorLights = new InteriorLights.State(appState, input.InteriorLights);
            Furnitures = new Furnitures.State(appState, input.Furnitures);
            Residents = new Residents.State(appState, input.Residents);
            TransportationItems = new TransportationItems.State(appState, input.TransportationItems);
            Freight = new Freight.State(appState, input.Freight);
            Wheels = new Wheels.State(appState, input.Wheels);
            Vehicles = new Vehicles.State(appState, input.Vehicles);

            sliceList = new List<IEntityStateSlice>() {
                Rooms,
                Floors,
                Windows,
                InteriorWalls,
                InteriorLights,
                Furnitures,
                Residents,
                TransportationItems,
                Freight,
                Wheels,
                Vehicles,
                Windows
            };

            Queries = new StateQueries(this);
            events = new Events();
        }

        public override void Setup()
        {
            Rooms.Setup();
            AddListeners(Rooms);

            Floors.Setup();
            AddListeners(Floors);

            Windows.Setup();
            AddListeners(Windows);

            InteriorWalls.Setup();
            AddListeners(InteriorWalls);

            InteriorLights.Setup();
            AddListeners(InteriorLights);

            Furnitures.Setup();
            AddListeners(Furnitures);

            Residents.Setup();
            AddListeners(Residents);

            TransportationItems.Setup();
            AddListeners(TransportationItems);

            Freight.Setup();
            AddListeners(Freight);

            Wheels.Setup();
            AddListeners(Wheels);

            Vehicles.Setup();
            AddListeners(Vehicles);

            Windows.Setup();
            AddListeners(Windows);

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
                DataTypes.Entities.Rooms.Room => Rooms,
                DataTypes.Entities.Floors.Floor => Floors,
                DataTypes.Entities.InteriorWalls.InteriorWall => InteriorWalls,
                DataTypes.Entities.InteriorLights.InteriorLight => InteriorLights,
                DataTypes.Entities.Furnitures.Furniture => Furnitures,
                DataTypes.Entities.Residents.Resident => Residents,
                DataTypes.Entities.TransportationItems.TransportationItem => TransportationItems,
                DataTypes.Entities.Freights.FreightItem => Freight,
                DataTypes.Entities.Wheels.Wheel => Wheels,
                DataTypes.Entities.Vehicles.Vehicle => Vehicles,
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
