using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes.Vehicles;
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
            Registry.appState.Attributes.Vehicles.events.onItemsAdded += OnVehicleAttributesWrappersAdded;
            Registry.appState.Attributes.Vehicles.events.onItemsRemoved += OnVehicleAttributesWrappersRemoved;
            Registry.appState.Attributes.Vehicles.events.onAttributeValueUpdated += OnVehicleAttributeValueUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Attributes.Vehicles.events.onItemsAdded -= OnVehicleAttributesWrappersAdded;
            Registry.appState.Attributes.Vehicles.events.onItemsRemoved -= OnVehicleAttributesWrappersRemoved;
            Registry.appState.Attributes.Vehicles.events.onAttributeValueUpdated -= OnVehicleAttributeValueUpdated;
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
        void OnVehicleAttributesWrappersAdded(ListWrapper<VehicleAttributesWrapper> vehicleAttributesWrappers)
        {
            VehicleAttributesWrapper vehicleAttributesWrapper = vehicleAttributesWrappers.items[0];

            if (this.vehicleAttributesWrapper == null)
            {
                this.vehicleAttributesWrapper = vehicleAttributesWrapper;
            }
        }

        void OnVehicleAttributesWrappersRemoved(ListWrapper<VehicleAttributesWrapper> vehicleAttributesWrappers)
        {
            foreach (VehicleAttributesWrapper vehicleAttributesWrapper in vehicleAttributesWrappers.items)
            {
                if (vehicleAttributesWrapper == this.vehicleAttributesWrapper)
                {
                    this.vehicleAttributesWrapper = null;
                }
            }
        }

        void OnVehicleAttributeValueUpdated(VehicleAttributesWrapper vehicleAttributesWrapper, VehicleAttribute attribute)
        {
        }
    }
}
