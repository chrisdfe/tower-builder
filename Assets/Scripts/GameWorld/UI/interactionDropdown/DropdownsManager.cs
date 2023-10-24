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
        public UIInteractionDropdown interactionDropdown { get; private set; }

        public AssetList assetList;

        /*
            Lifecycle
        */
        public void Awake()
        {
            interactionDropdown = transform.Find("InteractionDropdown").GetComponent<UIInteractionDropdown>();
        }

        /* 
            Public API
        */
        public void CloseAll()
        {
            interactionDropdown.Close();
        }

        /*
            Static API
        */
        public static DropdownsManager Find() =>
            GameObject.Find("DropdownsManager").GetComponent<DropdownsManager>();
    }
}