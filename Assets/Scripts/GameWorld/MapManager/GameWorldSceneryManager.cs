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

        VehicleAttributesGroup vehicleAttributesGroup;

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
            Registry.appState.Attributes.Vehicles.events.onItemsAdded += OnVehicleAttributesGroupsAdded;
            Registry.appState.Attributes.Vehicles.events.onItemsRemoved += OnVehicleAttributesGroupsRemoved;
            Registry.appState.Attributes.Vehicles.events.onAttributeValueUpdated += OnVehicleAttributeValueUpdated;
        }

        public void Teardown()
        {
            Registry.appState.Attributes.Vehicles.events.onItemsAdded -= OnVehicleAttributesGroupsAdded;
            Registry.appState.Attributes.Vehicles.events.onItemsRemoved -= OnVehicleAttributesGroupsRemoved;
            Registry.appState.Attributes.Vehicles.events.onAttributeValueUpdated -= OnVehicleAttributeValueUpdated;
        }

        void Update()
        {
            MoveScenery();
        }

        void MoveScenery()
        {
            if (vehicleAttributesGroup == null) return;

            float currentTickInterval = Registry.appState.Time.queries.currentTickInterval;

            if (vehicleAttributesGroup.isMoving)
            {
                float currentSpeed = vehicleAttributesGroup.FindByKey(VehicleAttribute.Key.CurrentSpeed).value;
                float xMovement = -((currentSpeed * (Time.deltaTime * parallaxLevel)) / currentTickInterval);
                wrapper.Translate(new Vector3(xMovement, 0, 0));
            }
        }

        /* 
            Event Handlers
        */
        void OnVehicleAttributesGroupsAdded(ListWrapper<VehicleAttributesGroup> vehicleAttributesGroups)
        {
            VehicleAttributesGroup vehicleAttributesGroup = vehicleAttributesGroups.items[0];

            if (this.vehicleAttributesGroup == null)
            {
                this.vehicleAttributesGroup = vehicleAttributesGroup;
            }
        }

        void OnVehicleAttributesGroupsRemoved(ListWrapper<VehicleAttributesGroup> vehicleAttributesGroups)
        {
            foreach (VehicleAttributesGroup vehicleAttributesGroup in vehicleAttributesGroups.items)
            {
                if (vehicleAttributesGroup == this.vehicleAttributesGroup)
                {
                    this.vehicleAttributesGroup = null;
                }
            }
        }

        void OnVehicleAttributeValueUpdated(VehicleAttributesGroup vehicleAttributesGroup, VehicleAttribute attribute)
        {
        }
    }
}
