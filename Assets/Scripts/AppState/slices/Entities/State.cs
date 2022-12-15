using System;
using System.Collections.Generic;
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

            public Input()
            {
                Rooms = new Rooms.State.Input();
                Floors = new Floors.State.Input();
                InteriorWalls = new InteriorWalls.State.Input();

                Furnitures = new Furnitures.State.Input();
                Residents = new Residents.State.Input();
                TransportationItems = new TransportationItems.State.Input();
                Freight = new Freight.State.Input();
            }
        }

        public Rooms.State Rooms { get; }
        public Floors.State Floors { get; }
        public InteriorWalls.State InteriorWalls { get; }

        public Furnitures.State Furnitures { get; }
        public Residents.State Residents { get; }
        public TransportationItems.State TransportationItems { get; }
        public Freight.State Freight { get; }

        public StateQueries Queries;

        Dictionary<Entity.Type, IEntityStateSlice> sliceMap;

        public State(AppState appState, Input input) : base(appState)
        {
            Rooms = new Rooms.State(appState, input.Rooms);
            Floors = new Floors.State(appState, input.Floors);
            InteriorWalls = new InteriorWalls.State(appState, input.InteriorWalls);

            Furnitures = new Furnitures.State(appState, input.Furnitures);
            Residents = new Residents.State(appState, input.Residents);
            TransportationItems = new TransportationItems.State(appState, input.TransportationItems);
            Freight = new Freight.State(appState, input.Freight);

            sliceMap = new Dictionary<Entity.Type, IEntityStateSlice> {
                {Entity.Type.Room,               Rooms },
                {Entity.Type.Floor,              Floors },
                {Entity.Type.InteriorWall,       InteriorWalls },
                {Entity.Type.Furniture,          Furnitures },
                {Entity.Type.Resident,           Residents },
                {Entity.Type.TransportationItem, TransportationItems },
                {Entity.Type.Freight,            Freight }
            };

            this.Queries = new StateQueries(this);
        }

        public override void Setup()
        {
            Rooms.Setup();
            Floors.Setup();
            InteriorWalls.Setup();

            Furnitures.Setup();
            Residents.Setup();
            TransportationItems.Setup();
            Freight.Setup();
        }

        public void Add(Entity entity)
        {
            Debug.Log("Add");
            Debug.Log(GetStateSlice(entity)?.GetType());
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

        IEntityStateSlice GetStateSlice(Entity.Type type)
        {
            if (!sliceMap.ContainsKey(type))
            {
                throw new NotSupportedException($"Entity type not handled: {type}");
            }

            return sliceMap[type];
        }
        IEntityStateSlice GetStateSlice(Entity entity) => GetStateSlice(entity.type);

        public class StateQueries
        {
            State state;

            public StateQueries(State state)
            {
                this.state = state;
            }
        }
    }
}
