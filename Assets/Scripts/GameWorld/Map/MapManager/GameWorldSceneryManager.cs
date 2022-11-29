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

        VehicleAttributeGroup vehicleAttributeGroup;

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

            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeGroupAdded += OnVehicleAttributeGroupAdded;
            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeGroupRemoved += OnVehicleAttributeGroupRemoved;
            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeCurrentSpeedUpdated += onVehicleAttributeCurrentSpeedUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Time.events.onTick -= OnTick;

            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeGroupAdded -= OnVehicleAttributeGroupAdded;
            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeGroupRemoved -= OnVehicleAttributeGroupRemoved;
            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeCurrentSpeedUpdated -= onVehicleAttributeCurrentSpeedUpdated;
        }

        void Update()
        {
            MoveScenery();
        }

        void MoveScenery()
        {
            if (vehicleAttributeGroup == null) return;

            float currentTickInterval = Registry.appState.Time.queries.currentTickInterval;

            if (vehicleAttributeGroup.isMoving)
            {
                float xMovement = -((vehicleAttributeGroup.currentSpeed * (Time.deltaTime * parallaxLevel)) / currentTickInterval);
                wrapper.Translate(new Vector3(xMovement, 0, 0));
            }
        }

        /* 
            Event Handlers
        */
        public void OnTick(TimeValue timeValue) { }

        public void OnVehicleAttributeGroupAdded(VehicleAttributeGroup vehicleAttributeGroup)
        {
            if (this.vehicleAttributeGroup == null)
            {
                this.vehicleAttributeGroup = vehicleAttributeGroup;
            }
        }

        public void OnVehicleAttributeGroupRemoved(VehicleAttributeGroup vehicleAttributeGroup)
        {
            if (vehicleAttributeGroup == this.vehicleAttributeGroup)
            {
                this.vehicleAttributeGroup = null;
            }
        }

        public void onVehicleAttributeCurrentSpeedUpdated(VehicleAttributeGroup vehicleAttributeGroup)
        {
            if (vehicleAttributeGroup == null) return;
        }
    }
}
