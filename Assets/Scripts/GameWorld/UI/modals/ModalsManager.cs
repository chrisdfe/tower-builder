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
    public class ModalsManager : MonoBehaviour
    {
        DebugModal debugModal;

        /*
            Lifecycle
        */
        public void Awake()
        {

            debugModal = transform.Find("DebugModal").GetComponent<DebugModal>();
        }

        /*
            Public Interface
        */
        public void ToggleDebugModal()
        {
            debugModal.Toggle();
        }
    }
}