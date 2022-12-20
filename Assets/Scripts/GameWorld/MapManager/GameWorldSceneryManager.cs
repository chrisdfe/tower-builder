using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Vehicles;
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
            Registry.appState.VehicleAttributesWrappers.events.onItemsAdded += OnVehicleAttributesWrappersAdded;
            Registry.appState.VehicleAttributesWrappers.events.onItemsRemoved += OnVehicleAttributesWrappersRemoved;
            Registry.appState.VehicleAttributesWrappers.events.onAttributeValueUpdated += OnVehicleAttributeValueUpdated;
        }

        public void Teardown()
        {
            Registry.appState.VehicleAttributesWrappers.events.onItemsAdded -= OnVehicleAttributesWrappersAdded;
            Registry.appState.VehicleAttributesWrappers.events.onItemsRemoved -= OnVehicleAttributesWrappersRemoved;
            Registry.appState.VehicleAttributesWrappers.events.onAttributeValueUpdated -= OnVehicleAttributeValueUpdated;
        }

        void Update()
        {
            MoveScenery();
        }

        void MoveScenery()
        {
            // Debug.Log(vehicleAttributesWrapper);
            if (vehicleAttributesWrapper == null) return;

            float currentTickInterval = Registry.appState.Time.queries.currentTickInterval;

            Debug.Log(vehicleAttributesWrapper.FindByKey(VehicleAttribute.Key.CurrentSpeed).value);

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

            // Debug.Log("OnVehicleAttributesWrapperAdded");
            if (this.vehicleAttributesWrapper == null)
            {
                this.vehicleAttributesWrapper = vehicleAttributesWrapper;
            }
        }

        void OnVehicleAttributesWrappersRemoved(ListWrapper<VehicleAttributesWrapper> vehicleAttributesWrappers)
        {
            foreach (VehicleAttributesWrapper vehicleAttributesWrapper in vehicleAttributesWrappers.items)
            {
                // Debug.Log("OnVehicleAttributesWrapperRemoved");
                if (vehicleAttributesWrapper == this.vehicleAttributesWrapper)
                {
                    this.vehicleAttributesWrapper = null;
                }
            }
        }

        void OnVehicleAttributeValueUpdated(VehicleAttributesWrapper vehicleAttributesWrapper, VehicleAttribute attribute)
        {
            // Debug.Log("OnVehicleAttributeValueUpdated");
            // Debug.Log(attribute.key);
            // Debug.Log(attribute.value);
        }
    }
}
