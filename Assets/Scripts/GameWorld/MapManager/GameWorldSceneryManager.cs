using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes.Vehicles;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class GameWorldSceneryManager : MonoBehaviour
    {
        public AssetList assetList = new AssetList();

        Transform wrapper;

        VehicleAttributes vehicleAttributes;

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
            Registry.appState.Attributes.Vehicles.onItemsAdded += OnVehicleAttributesAdded;
            Registry.appState.Attributes.Vehicles.onItemsRemoved += OnVehicleAttributesRemoved;

            Registry.appState.Attributes.Vehicles.onAttributeValueUpdated += OnVehicleAttributeValueUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Attributes.Vehicles.onItemsAdded -= OnVehicleAttributesAdded;
            Registry.appState.Attributes.Vehicles.onItemsRemoved -= OnVehicleAttributesRemoved;

            Registry.appState.Attributes.Vehicles.onAttributeValueUpdated -= OnVehicleAttributeValueUpdated;
        }

        void Update()
        {
            MoveScenery();
        }

        void MoveScenery()
        {
            if (vehicleAttributes == null) return;

            float currentTickInterval = Registry.appState.Time.currentTickInterval;

            if (vehicleAttributes.isMoving)
            {
                // TODO - cache this stuff to avoid re-querying everything every frame
                float currentSpeed = vehicleAttributes.currentSpeed;
                float xMovement = -((currentSpeed * (Time.deltaTime * parallaxLevel)) / currentTickInterval);
                wrapper.Translate(new Vector3(xMovement, 0, 0));
            }
        }

        /* 
            Event Handlers
        */
        void OnVehicleAttributesAdded(ListWrapper<VehicleAttributes> vehicleAttributess)
        {
            VehicleAttributes vehicleAttributes = vehicleAttributess.items[0];

            if (this.vehicleAttributes == null)
            {
                this.vehicleAttributes = vehicleAttributes;
            }
        }

        void OnVehicleAttributesRemoved(ListWrapper<VehicleAttributes> vehicleAttributess)
        {
            foreach (VehicleAttributes vehicleAttributes in vehicleAttributess.items)
            {
                if (vehicleAttributes == this.vehicleAttributes)
                {
                    this.vehicleAttributes = null;
                }
            }
        }

        void OnVehicleAttributeValueUpdated(VehicleAttributes vehicleAttributes, Attribute attribute)
        {
        }
    }
}
