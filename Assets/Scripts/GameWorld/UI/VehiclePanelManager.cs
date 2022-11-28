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

        VehicleAttributeGroup vehicleAttributeGroup;

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
            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeGroupAdded += OnVehicleAttributeGroupAdded;
            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeGroupRemoved += OnVehicleAttributeGroupRemoved;
            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeGroupUpdated += OnVehicleAttributeGroupUpdated;
        }

        void Teardown()
        {
            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeGroupAdded -= OnVehicleAttributeGroupAdded;
            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeGroupRemoved += OnVehicleAttributeGroupRemoved;
            Registry.appState.VehicleAttributeGroups.events.onVehicleAttributeGroupUpdated -= OnVehicleAttributeGroupUpdated;
        }

        void UpdateText()
        {
            if (vehicleAttributeGroup == null)
            {
                text.text = "nothing.";
                return;
            }

            string result = "";

            result += $"is moving: {vehicleAttributeGroup.isMoving}\n";
            result += $"weight: {vehicleAttributeGroup.weight}\n";
            result += $"engine power: {vehicleAttributeGroup.enginePower}\n";

            text.text = result;
        }

        /*
            Event Handlers
        */
        void OnVehicleAttributeGroupAdded(VehicleAttributeGroup vehicleAttributeGroup)
        {
            // TODO - not this forever
            this.vehicleAttributeGroup = vehicleAttributeGroup;
            UpdateText();
        }

        void OnVehicleAttributeGroupRemoved(VehicleAttributeGroup vehicleAttributeGroup)
        {
            if (vehicleAttributeGroup == this.vehicleAttributeGroup)
            {
                this.vehicleAttributeGroup = null;
                UpdateText();
            }
        }

        void OnVehicleAttributeGroupUpdated(VehicleAttributeGroup vehicleAttributeGroup)
        {
            if (vehicleAttributeGroup == this.vehicleAttributeGroup)
            {
                UpdateText();
            }
        }
    }
}