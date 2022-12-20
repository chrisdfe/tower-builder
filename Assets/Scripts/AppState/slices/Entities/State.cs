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
            public InteriorWalls.State.Input InteriorWalls;

            public TransportationItems.State.Input TransportationItems;
            public Freight.State.Input Freight;
            public Wheels.State.Input Wheels;
            public Vehicles.State.Input Vehicles;

            public Input()
            {
                Rooms = new Rooms.State.Input();
                Floors = new Floors.State.Input();
                InteriorWalls = new InteriorWalls.State.Input();

                Furnitures = new Furnitures.State.Input();
                Residents = new Residents.State.Input();
                TransportationItems = new TransportationItems.State.Input();
                Freight = new Freight.State.Input();
                Wheels = new Wheels.State.Input();
                Vehicles = new Vehicles.State.Input();
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
        public InteriorWalls.State InteriorWalls { get; }

        public Furnitures.State Furnitures { get; }
        public Residents.State Residents { get; }
        public TransportationItems.State TransportationItems { get; }
        public Freight.State Freight { get; }
        public Wheels.State Wheels { get; }
        public Vehicles.State Vehicles { get; }

        public Events events { get; }
        public StateQueries Queries { get; }

        public Dictionary<Entity.Type, IEntityStateSlice> sliceMap { get; }
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
            InteriorWalls = new InteriorWalls.State(appState, input.InteriorWalls);

            Furnitures = new Furnitures.State(appState, input.Furnitures);
            Residents = new Residents.State(appState, input.Residents);
            TransportationItems = new TransportationItems.State(appState, input.TransportationItems);
            Freight = new Freight.State(appState, input.Freight);
            Wheels = new Wheels.State(appState, input.Wheels);
            Vehicles = new Vehicles.State(appState, input.Vehicles);

            sliceMap = new Dictionary<Entity.Type, IEntityStateSlice> {
                {Entity.Type.Room,               Rooms },
                {Entity.Type.Floor,              Floors },
                {Entity.Type.InteriorWall,       InteriorWalls },
                {Entity.Type.Furniture,          Furnitures },
                {Entity.Type.Resident,           Residents },
                {Entity.Type.TransportationItem, TransportationItems },
                {Entity.Type.Freight,            Freight },
                {Entity.Type.Wheel,              Wheels },
                {Entity.Type.Vehicle,            Vehicles }
            };

            sliceList = sliceMap.Values.ToList();

            Queries = new StateQueries(this);
            events = new Events();
        }

        public override void Setup()
        {
            Rooms.Setup();
            AddListeners(Rooms);

            Floors.Setup();
            AddListeners(Floors);

            InteriorWalls.Setup();
            AddListeners(InteriorWalls);

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

            void AddListeners(IEntityStateSlice stateSlice)
            {
                stateSlice.entityEvents.onEntitiesAdded += OnEntitiesAdded;
                stateSlice.entityEvents.onEntitiesRemoved += OnEntitiesRemoved;
                stateSlice.entityEvents.onEntitiesBuilt += OnEntitiesBuilt;
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

        public IEntityStateSlice GetStateSlice(Entity.Type type)
        {
            if (!sliceMap.ContainsKey(type))
            {
                throw new NotSupportedException($"Entity type not handled: {type}");
            }

            return sliceMap[type];
        }

        public IEntityStateSlice GetStateSlice(Entity entity) => GetStateSlice(entity.type);

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
