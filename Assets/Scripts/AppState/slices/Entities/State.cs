using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Freights;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;

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
            public TransportationItems.State.Input TransportationItems;
            public Vehicles.State.Input Vehicles;
            public Freight.State.Input Freight;

            public Input()
            {
                Rooms = new Rooms.State.Input();
                Floors = new Floors.State.Input();
                Furnitures = new Furnitures.State.Input();
                Residents = new Residents.State.Input();
                TransportationItems = new TransportationItems.State.Input();
                Vehicles = new Vehicles.State.Input();
                Freight = new Freight.State.Input();
            }
        }

        public Rooms.State Rooms { get; }
        public Floors.State Floors { get; }

        public Furnitures.State Furnitures { get; }
        public Residents.State Residents { get; }
        public TransportationItems.State TransportationItems { get; }
        public Vehicles.State Vehicles { get; }
        public Freight.State Freight { get; }

        public StateQueries Queries;

        public State(AppState appState, Input input) : base(appState)
        {
            Rooms = new Rooms.State(appState, input.Rooms);
            Floors = new Floors.State(appState, input.Floors);

            Furnitures = new Furnitures.State(appState, input.Furnitures);
            Residents = new Residents.State(appState, input.Residents);
            TransportationItems = new TransportationItems.State(appState, input.TransportationItems);
            Vehicles = new Vehicles.State(appState, input.Vehicles);
            Freight = new Freight.State(appState, input.Freight);

            Queries = new StateQueries(this);
        }

        public override void Setup()
        {
            Rooms.Setup();
            Floors.Setup();

            Furnitures.Setup();
            Residents.Setup();
            TransportationItems.Setup();
            Vehicles.Setup();
            Freight.Setup();
        }

        public void Add(Entity entity)
        {
            switch (entity)
            {
                case Room room:
                    Rooms.Add(room);
                    break;
                case Floor floor:
                    Floors.Add(floor);
                    break;
                case Furniture furniture:
                    Furnitures.Add(furniture);
                    break;
                case Resident resident:
                    Residents.Add(resident);
                    break;
                case TransportationItem transportationItem:
                    TransportationItems.Add(transportationItem);
                    break;
                case FreightItem freightItem:
                    Freight.FreightItems.Add(freightItem);
                    break;
                default:
                    throw new NotSupportedException($"Entity type not handled: {entity.GetType()}");
            }
        }

        public void Build(Entity entity)
        {
            switch (entity)
            {
                case Room room:
                    Rooms.Build(room);
                    break;
                case Floor floor:
                    Floors.Build(floor);
                    break;
                case Furniture furniture:
                    Furnitures.Build(furniture);
                    break;
                case Resident resident:
                    Residents.Build(resident);
                    break;
                case TransportationItem transportationItems:
                    TransportationItems.Build(transportationItems);
                    break;
                case FreightItem freightItem:
                    Freight.FreightItems.Build(freightItem);
                    break;
                default:
                    throw new NotSupportedException($"Entity type not handled: {entity.GetType()}");
            }
        }

        public void Remove(Entity entity)
        {
            switch (entity)
            {
                case Room room:
                    Rooms.Remove(room);
                    break;
                case Floor floor:
                    Floors.Remove(floor);
                    break;
                case Furniture furniture:
                    Furnitures.Remove(furniture);
                    break;
                case Resident resident:
                    Residents.Remove(resident);
                    break;
                case TransportationItem transportationItems:
                    TransportationItems.Remove(transportationItems);
                    break;
                case FreightItem freightItem:
                    Freight.FreightItems.Remove(freightItem);
                    break;
                default:
                    throw new NotSupportedException($"Entity type not handled: {entity.GetType()}");
            }
        }

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
