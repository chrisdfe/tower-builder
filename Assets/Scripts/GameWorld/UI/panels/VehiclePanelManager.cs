using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes.Vehicles;
using TowerBuilder.DataTypes.EntityGroups.Vehicles;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class VehiclePanelManager : MonoBehaviour
    {
        Transform contentWrapper;
        Text text;

        Vehicle vehicle;

        void Awake()
        {
            contentWrapper = transform.Find("Content");
            text = contentWrapper.Find("Text").GetComponent<Text>();

            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        void Setup()
        {
            // Registry.appState.Entities.Vehicles.events.onItemsAdded += OnVehiclesAdded;
            // Registry.appState.Entities.Vehicles.events.onItemsRemoved += OnVehiclesRemoved;

            // Registry.appState.Attributes.Vehicles.events.onItemsUpdated += OnVehicleAttributesUpdated;
            // Registry.appState.Attributes.Vehicles.events.onVehicleDerivedAttributesRecalculated += OnVehicleDerivedAttributesRecalculated;
        }

        void Teardown()
        {
            // Registry.appState.Entities.Vehicles.events.onItemsAdded -= OnVehiclesAdded;
            // Registry.appState.Entities.Vehicles.events.onItemsRemoved -= OnVehiclesRemoved;

            // Registry.appState.Attributes.Vehicles.events.onItemsUpdated += OnVehicleAttributesUpdated;
            // Registry.appState.Attributes.Vehicles.events.onVehicleDerivedAttributesRecalculated -= OnVehicleDerivedAttributesRecalculated;
        }

        void UpdateText()
        {
            if (vehicle == null)
            {
                text.text = "nothing.";
                return;
            }

            string result = $"{vehicle}";

            VehicleAttributes vehicleAttributes = Registry.appState.Attributes.Vehicles.FindByVehicle(vehicle);

            if (vehicleAttributes != null)
            {
                vehicleAttributes.asTupleList.ForEach(tuple =>
                {
                    var (key, attribute) = tuple;
                    result += $"{key}: {attribute.value}\n";
                });
            }

            text.text = result;
        }

        /*
            Event Handlers
        */
        void OnVehiclesAdded(ListWrapper<Vehicle> vehicleList)
        {
            this.vehicle = vehicleList.items[0];
        }

        void OnVehiclesRemoved(ListWrapper<Vehicle> vehicleList)
        {
            this.vehicle = null;
        }

        void OnVehicleAttributesUpdated(ListWrapper<VehicleAttributes> vehicleAttributesList)
        {
            foreach (VehicleAttributes vehicleAttributes in vehicleAttributesList.items)
            {
                if (vehicleAttributes.vehicle == this.vehicle)
                {
                    UpdateText();
                }
            }
        }

        void OnVehicleDerivedAttributesRecalculated(VehicleAttributes vehicleAttributes)
        {
            if (vehicleAttributes.vehicle == this.vehicle)
            {
                UpdateText();
            }
        }
    }
}