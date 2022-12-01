using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.VehicleAttributesWrappers
{
    public class State : StateSlice
    {
        public class Input
        {
            public VehicleAttributesWrapperList vehicleAttributesWrapperList;
        }

        public class Events
        {
            public delegate void VehicleAttributesWrapperEvent(VehicleAttributesWrapper vehicleAttributesWrapper);
            public VehicleAttributesWrapperEvent onVehicleAttributesWrapperAdded;
            public VehicleAttributesWrapperEvent onVehicleAttributesWrapperRemoved;
            public VehicleAttributesWrapperEvent onVehicleAttributesWrapperUpdated;

            public VehicleAttributesWrapperEvent onVehicleAttributeWeightUpdated;
            public VehicleAttributesWrapperEvent onVehicleAttributeCurrentSpeedUpdated;
            public VehicleAttributesWrapperEvent onVehicleAttributeEnginePowerUpdated;

            public VehicleAttributesWrapperEvent onVehicleAttributesWrapperStartedMoving;
            public VehicleAttributesWrapperEvent onVehicleAttributesWrapperStoppedMoving;
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

            public VehicleAttributesWrapper FindByVehicle(Vehicle vehicle)
            {
                return state.vehicleAttributesWrapperList.FindByVehicle(vehicle);
            }

            public VehicleAttributesWrapper FindByFurnitureBehavior(FurnitureBehaviorBase furnitureBehavior)
            {
                // Find vehicle by furniture
                Vehicle vehicle = appState.Vehicles.queries.FindVehicleByFurniture(furnitureBehavior.furniture);
                VehicleAttributesWrapper vehicleAttributesWrapper = FindByVehicle(vehicle);
                return vehicleAttributesWrapper;
            }
        }

        public VehicleAttributesWrapperList vehicleAttributesWrapperList { get; private set; } = new VehicleAttributesWrapperList();

        public Events events;
        public Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            vehicleAttributesWrapperList = input.vehicleAttributesWrapperList ?? new VehicleAttributesWrapperList();

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

        public void AddVehicleAttributesWrapper(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            vehicleAttributesWrapperList.Add(vehicleAttributesWrapper);

            if (events.onVehicleAttributesWrapperAdded != null)
            {
                events.onVehicleAttributesWrapperAdded(vehicleAttributesWrapper);
            }
        }

        public void AddVehicleAttributesWrapperForVehicle(Vehicle vehicle)
        {
            VehicleAttributesWrapper vehicleAttributesWrapper = new VehicleAttributesWrapper(appState, vehicle);
            vehicleAttributesWrapper.Setup();
            AddVehicleAttributesWrapper(vehicleAttributesWrapper);
        }

        public void RemoveVehicleAttributesWrapper(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            vehicleAttributesWrapperList.Remove(vehicleAttributesWrapper);

            if (events.onVehicleAttributesWrapperRemoved != null)
            {
                events.onVehicleAttributesWrapperRemoved(vehicleAttributesWrapper);
            }
        }

        public void StartVehicleAttributesMoving(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            vehicleAttributesWrapper.StartMoving();

            if (events.onVehicleAttributesWrapperStartedMoving != null)
            {
                events.onVehicleAttributesWrapperStartedMoving(vehicleAttributesWrapper);
            }

            if (events.onVehicleAttributeCurrentSpeedUpdated != null)
            {
                events.onVehicleAttributeCurrentSpeedUpdated(vehicleAttributesWrapper);
            }

            if (events.onVehicleAttributesWrapperUpdated != null)
            {
                events.onVehicleAttributesWrapperUpdated(vehicleAttributesWrapper);
            }
        }

        public void StopVehicleAttributesMoving(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            vehicleAttributesWrapper.StopMoving();

            if (events.onVehicleAttributesWrapperStoppedMoving != null)
            {
                events.onVehicleAttributesWrapperStoppedMoving(vehicleAttributesWrapper);
            }

            if (events.onVehicleAttributeCurrentSpeedUpdated != null)
            {
                events.onVehicleAttributeCurrentSpeedUpdated(vehicleAttributesWrapper);
            }

            if (events.onVehicleAttributesWrapperUpdated != null)
            {
                events.onVehicleAttributesWrapperUpdated(vehicleAttributesWrapper);
            }
        }

        public void RecalculateVehicleWeight(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            vehicleAttributesWrapper.RecalculateWeight();

            if (events.onVehicleAttributeWeightUpdated != null)
            {
                events.onVehicleAttributeWeightUpdated(vehicleAttributesWrapper);
            }

            if (events.onVehicleAttributesWrapperUpdated != null)
            {
                events.onVehicleAttributesWrapperUpdated(vehicleAttributesWrapper);
            }
        }

        public void RecalculateVehicleEnginePower(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            vehicleAttributesWrapper.RecalculateEnginePower();

            if (events.onVehicleAttributeEnginePowerUpdated != null)
            {
                events.onVehicleAttributeEnginePowerUpdated(vehicleAttributesWrapper);
            }

            if (events.onVehicleAttributesWrapperUpdated != null)
            {
                events.onVehicleAttributesWrapperUpdated(vehicleAttributesWrapper);
            }
        }

        /* 
            Event Handlers
        */
        void OnVehicleAdded(Vehicle vehicle)
        {
            AddVehicleAttributesWrapperForVehicle(vehicle);
        }

        void OnVehicleRoomAdded(Vehicle vehicle, Room room)
        {
            VehicleAttributesWrapper vehicleAttributesWrapper = vehicleAttributesWrapperList.FindByVehicle(vehicle);
            RecalculateVehicleWeight(vehicleAttributesWrapper);
        }

        void OnVehicleRoomRemoved(Vehicle vehicle, Room room)
        {
            VehicleAttributesWrapper vehicleAttributesWrapper = vehicleAttributesWrapperList.FindByVehicle(vehicle);
            RecalculateVehicleWeight(vehicleAttributesWrapper);

            if (events.onVehicleAttributesWrapperUpdated != null)
            {
                events.onVehicleAttributesWrapperUpdated(vehicleAttributesWrapper);
            }
        }

        void OnVehicleRemoved(Vehicle vehicle)
        {
            VehicleAttributesWrapper vehicleAttributesWrapper = vehicleAttributesWrapperList.FindByVehicle(vehicle);

            vehicleAttributesWrapperList.Remove(vehicleAttributesWrapper);

            if (events.onVehicleAttributesWrapperRemoved != null)
            {
                events.onVehicleAttributesWrapperRemoved(vehicleAttributesWrapper);
            }
        }

        void OnFurnitureBehaviorInteractStart(FurnitureBehaviorBase furnitureBehavior)
        {
            if (furnitureBehavior is CockpitBehavior)
            {
                VehicleAttributesWrapper vehicleAttributesWrapper = queries.FindByFurnitureBehavior(furnitureBehavior);
                StartVehicleAttributesMoving(vehicleAttributesWrapper);
            }
        }

        void OnFurnitureBehaviorInteractEnd(FurnitureBehaviorBase furnitureBehavior)
        {
            if (furnitureBehavior is CockpitBehavior)
            {
                VehicleAttributesWrapper vehicleAttributesWrapper = queries.FindByFurnitureBehavior(furnitureBehavior);
                StopVehicleAttributesMoving(vehicleAttributesWrapper);
            }
        }

        void OnFurnitureBehaviorAdded(FurnitureBehaviorBase furnitureBehavior)
        {
            VehicleAttributesWrapper vehicleAttributesWrapper = queries.FindByFurnitureBehavior(furnitureBehavior);

            switch (furnitureBehavior.key)
            {
                case FurnitureBehaviorBase.Key.Engine:
                    RecalculateVehicleEnginePower(vehicleAttributesWrapper);
                    break;
            }
        }

        void OnFurnitureBehaviorRemoved(FurnitureBehaviorBase furnitureBehavior)
        {
            VehicleAttributesWrapper vehicleAttributesWrapper = queries.FindByFurnitureBehavior(furnitureBehavior);

            switch (furnitureBehavior.key)
            {
                case FurnitureBehaviorBase.Key.Engine:
                    RecalculateVehicleEnginePower(vehicleAttributesWrapper);
                    break;
            }
        }
    }
}