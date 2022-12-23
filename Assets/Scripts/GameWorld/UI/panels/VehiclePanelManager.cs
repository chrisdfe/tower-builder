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
            Registry.appState.Entities.Vehicles.events.onItemsAdded += OnVehiclesAdded;
            Registry.appState.Entities.Vehicles.events.onItemsRemoved += OnVehiclesRemoved;

            Registry.appState.Attributes.Vehicles.events.onItemsUpdated += OnVehicleAttributesGroupsUpdated;
        }

        void Teardown()
        {
            Registry.appState.Entities.Vehicles.events.onItemsAdded -= OnVehiclesAdded;
            Registry.appState.Entities.Vehicles.events.onItemsRemoved -= OnVehiclesRemoved;

            Registry.appState.Attributes.Vehicles.events.onItemsUpdated += OnVehicleAttributesGroupsUpdated;
        }

        void UpdateText()
        {
            if (vehicle == null)
            {
                text.text = "nothing.";
                return;
            }

            string result = $"{vehicle}";

            VehicleAttributesGroup vehicleAttributesGroup = Registry.appState.Attributes.Vehicles.queries.FindByVehicle(vehicle);

            if (vehicleAttributesGroup != null)
            {
                vehicleAttributesGroup.attributes.ForEach(attribute =>
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

        void OnVehicleAttributesGroupsUpdated(ListWrapper<VehicleAttributesGroup> vehicleAttributesGroupList)
        {
            foreach (VehicleAttributesGroup vehicleAttributesGroup in vehicleAttributesGroupList.items)
            {
                if (vehicleAttributesGroup.vehicle == this.vehicle)
                {
                    UpdateText();
                }
            }
        }
    }
}