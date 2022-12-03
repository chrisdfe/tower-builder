using TowerBuilder.DataTypes.Time;
using TowerBuilder.DataTypes.Vehicles;
using UnityEngine;

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
            Registry.appState.VehicleAttributesWrappers.events.onAttributesWrapperAdded += OnVehicleAttributesWrapperAdded;
            Registry.appState.VehicleAttributesWrappers.events.onAttributesWrapperRemoved += OnVehicleAttributesWrapperRemoved;
            Registry.appState.VehicleAttributesWrappers.events.onAttributeValueUpdated += OnVehicleAttributeValueUpdated;
        }

        public void Teardown()
        {
            Registry.appState.VehicleAttributesWrappers.events.onAttributesWrapperAdded -= OnVehicleAttributesWrapperAdded;
            Registry.appState.VehicleAttributesWrappers.events.onAttributesWrapperRemoved -= OnVehicleAttributesWrapperRemoved;
            Registry.appState.VehicleAttributesWrappers.events.onAttributeValueUpdated -= OnVehicleAttributeValueUpdated;
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
                float currentSpeed = vehicleAttributesWrapper.FindByKey(VehicleAttribute.Key.CurrentSpeed).value;
                float xMovement = -((currentSpeed * (Time.deltaTime * parallaxLevel)) / currentTickInterval);
                wrapper.Translate(new Vector3(xMovement, 0, 0));
            }
        }

        /* 
            Event Handlers
        */
        void OnVehicleAttributesWrapperAdded(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            if (this.vehicleAttributesWrapper == null)
            {
                this.vehicleAttributesWrapper = vehicleAttributesWrapper;
            }
        }

        void OnVehicleAttributesWrapperRemoved(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            if (vehicleAttributesWrapper == this.vehicleAttributesWrapper)
            {
                this.vehicleAttributesWrapper = null;
            }
        }

        void OnVehicleAttributeValueUpdated(VehicleAttributesWrapper vehicleAttributesWrapper, VehicleAttribute attribute)
        {
        }
    }
}
