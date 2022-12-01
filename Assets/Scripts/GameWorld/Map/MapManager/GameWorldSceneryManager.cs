using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.DataTypes.Vehicles;
using TowerBuilder.GameWorld.Rooms;
using TowerBuilder.GameWorld.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class GameWorldSceneryManager : MonoBehaviour
    {
        public enum ModelKey
        {
            Tree,
        }

        public AssetList<ModelKey> assetList = new AssetList<ModelKey>();

        Transform wrapper;

        VehicleAttributesWrapper vehicleAttributesWrapper;

        float parallaxLevel = 0.1f;

        /* 
            Lifecycle Methods
        */
        void Awake()
        {
            wrapper = transform.Find("Wrapper");

            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            Registry.appState.Time.events.onTick += OnTick;

            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributesWrapperAdded += OnVehicleAttributesWrapperAdded;
            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributesWrapperRemoved += OnVehicleAttributesWrapperRemoved;
            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributeCurrentSpeedUpdated += onVehicleAttributeCurrentSpeedUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Time.events.onTick -= OnTick;

            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributesWrapperAdded -= OnVehicleAttributesWrapperAdded;
            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributesWrapperRemoved -= OnVehicleAttributesWrapperRemoved;
            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributeCurrentSpeedUpdated -= onVehicleAttributeCurrentSpeedUpdated;
        }

        void Update()
        {
            MoveScenery();
        }

        void MoveScenery()
        {
            if (vehicleAttributesWrapper == null) return;

            float currentTickInterval = Registry.appState.Time.queries.currentTickInterval;

            if (vehicleAttributesWrapper.isMoving)
            {
                float xMovement = -((vehicleAttributesWrapper.currentSpeed * (Time.deltaTime * parallaxLevel)) / currentTickInterval);
                wrapper.Translate(new Vector3(xMovement, 0, 0));
            }
        }

        /* 
            Event Handlers
        */
        public void OnTick(TimeValue timeValue) { }

        public void OnVehicleAttributesWrapperAdded(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            if (this.vehicleAttributesWrapper == null)
            {
                this.vehicleAttributesWrapper = vehicleAttributesWrapper;
            }
        }

        public void OnVehicleAttributesWrapperRemoved(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            if (vehicleAttributesWrapper == this.vehicleAttributesWrapper)
            {
                this.vehicleAttributesWrapper = null;
            }
        }

        public void onVehicleAttributeCurrentSpeedUpdated(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            if (vehicleAttributesWrapper == null) return;
        }
    }
}
