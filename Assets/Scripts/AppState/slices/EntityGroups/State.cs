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

        public class Events
        {

        }

        public Rooms.State Rooms { get; }
        public Vehicles.State Vehicles { get; }
        public Buildings.State Buildings { get; }

        public Events events { get; }

        public State(AppState appState, Input input) : base(appState)
        {
            Rooms = new Rooms.State(appState, input.Rooms);
            Vehicles = new Vehicles.State(appState, input.Vehicles);
            Buildings = new Buildings.State(appState, input.Buildings);

            events = new Events();
        }

        public override void Setup()
        {
            base.Setup();

            Rooms.Setup();
            Vehicles.Setup();
            Buildings.Setup();
        }

        public override void Teardown()
        {
            base.Teardown();

            Rooms.Teardown();
            Vehicles.Teardown();
            Buildings.Teardown();
        }

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

        public EntityGroupStateSlice GetStateSlice(EntityGroup entityGroup) =>
            entityGroup switch
            {
                DataTypes.EntityGroups.Rooms.Room => Rooms,
                DataTypes.EntityGroups.Vehicles.Vehicle => Vehicles,
                DataTypes.EntityGroups.Buildings.Building => Buildings,
                _ => throw new NotSupportedException($"EntityGroup type not handled: {entityGroup.GetType()}")
            };
    }
}
