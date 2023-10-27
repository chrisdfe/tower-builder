using TowerBuilder.DataTypes;

using TowerBuilder.DataTypes.EntityGroups;
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
            Registry.appState.EntityGroups.Vehicles.onItemsAdded += OnVehiclesAdded;
            Registry.appState.EntityGroups.Vehicles.onItemsRemoved += OnVehiclesRemoved;

            // Registry.appState.Attributes.Vehicles.events.onItemsUpdated += OnVehicleAttributesUpdated;
            // Registry.appState.Attributes.Vehicles.events.onVehicleDerivedAttributesRecalculated += OnVehicleDerivedAttributesRecalculated;
        }

        void Teardown()
        {
            Registry.appState.EntityGroups.Vehicles.onItemsAdded -= OnVehiclesAdded;
            Registry.appState.EntityGroups.Vehicles.onItemsRemoved -= OnVehiclesRemoved;

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

            vehicle.attributes.asTupleList.ForEach(tuple =>
                {
                    var (key, attribute) = tuple;
                    result += $"{key}: {attribute.value}\n";
                });

            text.text = result;
        }

        /*
            Event Handlers
        */
        void OnVehiclesAdded(ListWrapper<EntityGroup> vehicleList)
        {
            this.vehicle = vehicleList.items[0] as Vehicle;
        }

        void OnVehiclesRemoved(ListWrapper<EntityGroup> vehicleList)
        {
            this.vehicle = null;
        }
    }
}