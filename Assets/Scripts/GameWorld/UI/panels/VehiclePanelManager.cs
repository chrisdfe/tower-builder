using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes.Vehicles;
using TowerBuilder.DataTypes.Entities.Vehicles;
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
            Registry.appState.Vehicles.events.onItemsAdded += OnVehiclesAdded;
            Registry.appState.Vehicles.events.onItemsRemoved += OnVehiclesRemoved;

            Registry.appState.Attributes.Vehicles.events.onItemsUpdated += OnVehicleAttributesWrappersUpdated;
        }

        void Teardown()
        {
            Registry.appState.Vehicles.events.onItemsAdded -= OnVehiclesAdded;
            Registry.appState.Vehicles.events.onItemsRemoved -= OnVehiclesRemoved;

            Registry.appState.Attributes.Vehicles.events.onItemsUpdated += OnVehicleAttributesWrappersUpdated;
        }

        void UpdateText()
        {
            if (vehicle == null)
            {
                text.text = "nothing.";
                return;
            }

            string result = $"{vehicle}";

            VehicleAttributesWrapper vehicleAttributesWrapper = Registry.appState.Attributes.Vehicles.queries.FindByVehicle(vehicle);

            if (vehicleAttributesWrapper != null)
            {
                vehicleAttributesWrapper.attributes.ForEach(attribute =>
                {
                    result += $"{attribute.key}: {attribute.value}\n";
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

        void OnVehicleAttributesWrappersUpdated(ListWrapper<VehicleAttributesWrapper> vehicleAttributesWrapperList)
        {
            foreach (VehicleAttributesWrapper vehicleAttributesWrapper in vehicleAttributesWrapperList.items)
            {
                if (vehicleAttributesWrapper.vehicle == this.vehicle)
                {
                    UpdateText();
                }
            }
        }
    }
}