using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.VehicleAttributeGroups
{
    public class State : StateSlice
    {
        public class Input
        {
            public VehicleAttributeGroupList vehicleAttributeGroupList;
        }

        public class Events
        {
            public delegate void VehicleAttributeGroupEvent(VehicleAttributeGroup vehicleAttributeGroup);
            public VehicleAttributeGroupEvent onVehicleAttributeGroupAdded;
            public VehicleAttributeGroupEvent onVehicleAttributeGroupRemoved;
            public VehicleAttributeGroupEvent onVehicleAttributeGroupUpdated;

            public VehicleAttributeGroupEvent onVehicleAttributeWeightUpdated;
            public VehicleAttributeGroupEvent onVehicleAttributeCurrentSpeedUpdated;
            public VehicleAttributeGroupEvent onVehicleAttributeEnginePowerUpdated;

            public VehicleAttributeGroupEvent onVehicleAttributeGroupStartedMoving;
            public VehicleAttributeGroupEvent onVehicleAttributeGroupStoppedMoving;
        }

        public class Queries
        {
            AppState appState;
            State state;

            public Queries(AppState appState, State state)
            {
                this.appState = appState;
                this.state = state;
            }

            public VehicleAttributeGroup FindByVehicle(Vehicle vehicle)
            {
                return state.vehicleAttributeGroupList.FindByVehicle(vehicle);
            }

            public VehicleAttributeGroup FindByFurnitureBehavior(FurnitureBehaviorBase furnitureBehavior)
            {
                // Find vehicle by furniture
                Vehicle vehicle = appState.Vehicles.queries.FindVehicleByFurniture(furnitureBehavior.furniture);
                VehicleAttributeGroup vehicleAttributeGroup = FindByVehicle(vehicle);
                return vehicleAttributeGroup;
            }
        }

        public VehicleAttributeGroupList vehicleAttributeGroupList { get; private set; } = new VehicleAttributeGroupList();

        public Events events;
        public Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            vehicleAttributeGroupList = input.vehicleAttributeGroupList ?? new VehicleAttributeGroupList();

            events = new Events();
            queries = new Queries(appState, this);

            Setup();
        }

        public void Setup()
        {
            appState.Vehicles.events.onVehicleAdded += OnVehicleAdded;
            appState.Vehicles.events.onVehicleRoomAdded += OnVehicleRoomAdded;
            appState.Vehicles.events.onVehicleRoomAdded += OnVehicleRoomRemoved;

            appState.FurnitureBehaviors.events.onFurnitureBehaviorAdded += OnFurnitureBehaviorAdded;
            appState.FurnitureBehaviors.events.onFurnitureBehaviorRemoved += OnFurnitureBehaviorRemoved;
            appState.FurnitureBehaviors.events.onFurnitureBehaviorInteractStart += OnFurnitureBehaviorInteractStart;
            appState.FurnitureBehaviors.events.onFurnitureBehaviorInteractEnd += OnFurnitureBehaviorInteractEnd;
        }

        public void Teardown()
        {
            appState.Vehicles.events.onVehicleAdded -= OnVehicleAdded;
            appState.Vehicles.events.onVehicleRoomAdded -= OnVehicleRoomAdded;
            appState.Vehicles.events.onVehicleRoomAdded -= OnVehicleRoomRemoved;

            appState.FurnitureBehaviors.events.onFurnitureBehaviorAdded -= OnFurnitureBehaviorAdded;
            appState.FurnitureBehaviors.events.onFurnitureBehaviorRemoved -= OnFurnitureBehaviorRemoved;
            appState.FurnitureBehaviors.events.onFurnitureBehaviorInteractStart -= OnFurnitureBehaviorInteractStart;
            appState.FurnitureBehaviors.events.onFurnitureBehaviorInteractEnd -= OnFurnitureBehaviorInteractEnd;
        }

        public void AddVehicleAttributeGroup(VehicleAttributeGroup vehicleAttributeGroup)
        {
            vehicleAttributeGroupList.Add(vehicleAttributeGroup);

            if (events.onVehicleAttributeGroupAdded != null)
            {
                events.onVehicleAttributeGroupAdded(vehicleAttributeGroup);
            }
        }

        public void AddVehicleAttributeGroupForVehicle(Vehicle vehicle)
        {
            VehicleAttributeGroup vehicleAttributeGroup = new VehicleAttributeGroup(appState, vehicle);
            vehicleAttributeGroup.Setup();
            AddVehicleAttributeGroup(vehicleAttributeGroup);
        }

        public void RemoveVehicleAttributeGroup(VehicleAttributeGroup vehicleAttributeGroup)
        {
            vehicleAttributeGroupList.Remove(vehicleAttributeGroup);

            if (events.onVehicleAttributeGroupRemoved != null)
            {
                events.onVehicleAttributeGroupRemoved(vehicleAttributeGroup);
            }
        }

        public void StartVehicleAttributesMoving(VehicleAttributeGroup vehicleAttributeGroup)
        {
            vehicleAttributeGroup.StartMoving();

            if (events.onVehicleAttributeGroupStartedMoving != null)
            {
                events.onVehicleAttributeGroupStartedMoving(vehicleAttributeGroup);
            }

            if (events.onVehicleAttributeCurrentSpeedUpdated != null)
            {
                events.onVehicleAttributeCurrentSpeedUpdated(vehicleAttributeGroup);
            }

            if (events.onVehicleAttributeGroupUpdated != null)
            {
                events.onVehicleAttributeGroupUpdated(vehicleAttributeGroup);
            }
        }

        public void StopVehicleAttributesMoving(VehicleAttributeGroup vehicleAttributeGroup)
        {
            vehicleAttributeGroup.StopMoving();

            if (events.onVehicleAttributeGroupStoppedMoving != null)
            {
                events.onVehicleAttributeGroupStoppedMoving(vehicleAttributeGroup);
            }

            if (events.onVehicleAttributeCurrentSpeedUpdated != null)
            {
                events.onVehicleAttributeCurrentSpeedUpdated(vehicleAttributeGroup);
            }

            if (events.onVehicleAttributeGroupUpdated != null)
            {
                events.onVehicleAttributeGroupUpdated(vehicleAttributeGroup);
            }
        }

        public void RecalculateVehicleWeight(VehicleAttributeGroup vehicleAttributeGroup)
        {
            vehicleAttributeGroup.RecalculateWeight();

            if (events.onVehicleAttributeWeightUpdated != null)
            {
                events.onVehicleAttributeWeightUpdated(vehicleAttributeGroup);
            }

            if (events.onVehicleAttributeGroupUpdated != null)
            {
                events.onVehicleAttributeGroupUpdated(vehicleAttributeGroup);
            }
        }

        public void RecalculateVehicleEnginePower(VehicleAttributeGroup vehicleAttributeGroup)
        {
            vehicleAttributeGroup.RecalculateEnginePower();

            if (events.onVehicleAttributeEnginePowerUpdated != null)
            {
                events.onVehicleAttributeEnginePowerUpdated(vehicleAttributeGroup);
            }

            if (events.onVehicleAttributeGroupUpdated != null)
            {
                events.onVehicleAttributeGroupUpdated(vehicleAttributeGroup);
            }
        }

        /* 
            Event Handlers
        */
        void OnVehicleAdded(Vehicle vehicle)
        {
            AddVehicleAttributeGroupForVehicle(vehicle);
        }

        void OnVehicleRoomAdded(Vehicle vehicle, Room room)
        {
            VehicleAttributeGroup vehicleAttributeGroup = vehicleAttributeGroupList.FindByVehicle(vehicle);
            RecalculateVehicleWeight(vehicleAttributeGroup);
        }

        void OnVehicleRoomRemoved(Vehicle vehicle, Room room)
        {
            VehicleAttributeGroup vehicleAttributeGroup = vehicleAttributeGroupList.FindByVehicle(vehicle);
            RecalculateVehicleWeight(vehicleAttributeGroup);

            if (events.onVehicleAttributeGroupUpdated != null)
            {
                events.onVehicleAttributeGroupUpdated(vehicleAttributeGroup);
            }
        }

        void OnVehicleRemoved(Vehicle vehicle)
        {
            VehicleAttributeGroup vehicleAttributeGroup = vehicleAttributeGroupList.FindByVehicle(vehicle);

            vehicleAttributeGroupList.Remove(vehicleAttributeGroup);

            if (events.onVehicleAttributeGroupRemoved != null)
            {
                events.onVehicleAttributeGroupRemoved(vehicleAttributeGroup);
            }
        }

        void OnFurnitureBehaviorInteractStart(FurnitureBehaviorBase furnitureBehavior)
        {
            if (furnitureBehavior is CockpitBehavior)
            {
                VehicleAttributeGroup vehicleAttributeGroup = queries.FindByFurnitureBehavior(furnitureBehavior);
                StartVehicleAttributesMoving(vehicleAttributeGroup);
            }
        }

        void OnFurnitureBehaviorInteractEnd(FurnitureBehaviorBase furnitureBehavior)
        {
            if (furnitureBehavior is CockpitBehavior)
            {
                VehicleAttributeGroup vehicleAttributeGroup = queries.FindByFurnitureBehavior(furnitureBehavior);
                StopVehicleAttributesMoving(vehicleAttributeGroup);
            }
        }

        void OnFurnitureBehaviorAdded(FurnitureBehaviorBase furnitureBehavior)
        {
            VehicleAttributeGroup vehicleAttributeGroup = queries.FindByFurnitureBehavior(furnitureBehavior);

            switch (furnitureBehavior.key)
            {
                case FurnitureBehaviorBase.Key.Engine:
                    RecalculateVehicleEnginePower(vehicleAttributeGroup);
                    break;
            }
        }

        void OnFurnitureBehaviorRemoved(FurnitureBehaviorBase furnitureBehavior)
        {
            VehicleAttributeGroup vehicleAttributeGroup = queries.FindByFurnitureBehavior(furnitureBehavior);

            switch (furnitureBehavior.key)
            {
                case FurnitureBehaviorBase.Key.Engine:
                    RecalculateVehicleEnginePower(vehicleAttributeGroup);
                    break;
            }
        }
    }
}