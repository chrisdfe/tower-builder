using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.DataTypes.Vehicles;
using TowerBuilder.Systems;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class VehiclePanelManager : MonoBehaviour
    {
        Transform contentWrapper;
        Text text;

        VehicleAttributesWrapper vehicleAttributesWrapper;

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
            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributesWrapperAdded += OnVehicleAttributesWrapperAdded;
            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributesWrapperRemoved += OnVehicleAttributesWrapperRemoved;
            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributesWrapperUpdated += OnVehicleAttributesWrapperUpdated;
        }

        void Teardown()
        {
            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributesWrapperAdded -= OnVehicleAttributesWrapperAdded;
            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributesWrapperRemoved += OnVehicleAttributesWrapperRemoved;
            Registry.appState.VehicleAttributesWrappers.events.onVehicleAttributesWrapperUpdated -= OnVehicleAttributesWrapperUpdated;
        }

        void UpdateText()
        {
            if (vehicleAttributesWrapper == null)
            {
                text.text = "nothing.";
                return;
            }

            string result = "";

            result += $"is moving: {vehicleAttributesWrapper.isMoving}\n";
            result += $"weight: {vehicleAttributesWrapper.weight}\n";
            result += $"engine power: {vehicleAttributesWrapper.enginePower}\n";

            text.text = result;
        }

        /*
            Event Handlers
        */
        void OnVehicleAttributesWrapperAdded(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            // TODO - not this forever
            this.vehicleAttributesWrapper = vehicleAttributesWrapper;
            UpdateText();
        }

        void OnVehicleAttributesWrapperRemoved(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            if (vehicleAttributesWrapper == this.vehicleAttributesWrapper)
            {
                this.vehicleAttributesWrapper = null;
                UpdateText();
            }
        }

        void OnVehicleAttributesWrapperUpdated(VehicleAttributesWrapper vehicleAttributesWrapper)
        {
            if (vehicleAttributesWrapper == this.vehicleAttributesWrapper)
            {
                UpdateText();
            }
        }
    }
}