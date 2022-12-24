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
            Registry.appState.Attributes.Vehicles.events.onItemsAdded += OnVehicleAttributesAdded;
            Registry.appState.Attributes.Vehicles.events.onItemsRemoved += OnVehicleAttributesRemoved;

            Registry.appState.Attributes.Vehicles.events.onAttributeValueUpdated += OnVehicleAttributeValueUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Attributes.Vehicles.events.onItemsAdded -= OnVehicleAttributesAdded;
            Registry.appState.Attributes.Vehicles.events.onItemsRemoved -= OnVehicleAttributesRemoved;

            Registry.appState.Attributes.Vehicles.events.onAttributeValueUpdated -= OnVehicleAttributeValueUpdated;
        }

        void Update()
        {
            MoveScenery();
        }

        void MoveScenery()
        {
            if (vehicleAttributes == null) return;

            float currentTickInterval = Registry.appState.Time.queries.currentTickInterval;

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
