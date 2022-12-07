using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.Systems;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class DebugMenuPanelManager : MonoBehaviour
    {
        Button add1HourButton;
        Button subtract1HourButton;

        Button add1000Button;
        Button subtract1000Button;

        Button addTestNotificationButton;

        Button testSaveButton;

        Transform panel;
        Transform contentWrapper;

        void Awake()
        {
            panel = transform.Find("DebugMenuPanel");
            panel.gameObject.SetActive(false);

            add1HourButton = TransformUtils.FindDeepChild(panel, "Add1HourButton").GetComponent<Button>();
            add1HourButton.onClick.AddListener(Add1Hour);

            subtract1HourButton = TransformUtils.FindDeepChild(panel, "Subtract1HourButton").GetComponent<Button>();
            subtract1HourButton.onClick.AddListener(Subtract1Hour);

            add1000Button = TransformUtils.FindDeepChild(panel, "Add1000Button").GetComponent<Button>();
            subtract1000Button = TransformUtils.FindDeepChild(panel, "Subtract1000Button").GetComponent<Button>();

            subtract1000Button.onClick.AddListener(Subtract1000FromBalance);
            add1000Button.onClick.AddListener(Add1000ToBalance);

            addTestNotificationButton = TransformUtils.FindDeepChild(panel, "AddTestNotificationButton").GetComponent<Button>();
            addTestNotificationButton.onClick.AddListener(AddTestNotification);

            testSaveButton = TransformUtils.FindDeepChild(panel, "TestSaveButton").GetComponent<Button>();
            testSaveButton.onClick.AddListener(OnTestSaveButtonClick);
        }

        void Update()
        {
            if (Input.GetKeyDown("\\"))
            {
                panel.gameObject.SetActive(!panel.gameObject.activeInHierarchy);
            }
        }

        void Add1Hour()
        {
            Registry.appState.Time.AddTime(new TimeValue.Input()
            {
                hour = 1
            });
        }

        void Subtract1Hour()
        {
            Registry.appState.Time.SubtractTime(new TimeValue.Input()
            {
                hour = 1
            });
        }

        void Subtract1000FromBalance()
        {
            Registry.appState.Wallet.SubtractBalance(1000);
        }

        void Add1000ToBalance()
        {
            Registry.appState.Wallet.AddBalance(1000);
        }

        void AddTestNotification()
        {
            int notificationsLength = Registry.appState.Notifications.list.Count;
            Registry.appState.Notifications.Add(
                new Notification("new message " + (notificationsLength + 1))
            );
        }

        void OnTestSaveButtonClick()
        {
            if (Registry.appState.Rooms.list.Count > 0)
            {
                Room room = Registry.appState.Rooms.list.items[0];
                SaveLoadSystem.SaveToFile<Room>(room);
            }
        }
    }
}