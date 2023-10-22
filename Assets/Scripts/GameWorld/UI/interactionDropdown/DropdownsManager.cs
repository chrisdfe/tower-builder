using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.Systems;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class DropdownsManager : MonoBehaviour
    {
        UIInteractionDropdown interactionDropdown;

        /*
            Lifecycle
        */
        public void Awake()
        {
            interactionDropdown = transform.Find("InteractionDropdown").GetComponent<UIInteractionDropdown>();
            Debug.Log("interactionDropdown");
            Debug.Log(interactionDropdown);
        }

        public void ToggleInteractionModal()
        {
            interactionDropdown.Toggle();
        }
    }
}