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
    public class DebugModal : MonoBehaviour
    {
        Button add1HourButton;
        Button subtract1HourButton;

        Button add1000Button;
        Button subtract1000Button;

        Button addTestNotificationButton;

        Button testSaveButton;
        Button testLoadButton;

        Transform contentWrapper;

        /*
            Lifecycle
        */
        public void Awake()
        {
            contentWrapper = TransformUtils.FindDeepChild(transform, "Content");
            contentWrapper.gameObject.SetActive(gameObject.activeSelf);

            add1HourButton = TransformUtils.FindDeepChild(contentWrapper, "Add1HourButton").GetComponent<Button>();
            add1HourButton.onClick.AddListener(Add1Hour);

            subtract1HourButton = TransformUtils.FindDeepChild(contentWrapper, "Subtract1HourButton").GetComponent<Button>();
            subtract1HourButton.onClick.AddListener(Subtract1Hour);

            add1000Button = TransformUtils.FindDeepChild(contentWrapper, "Add1000Button").GetComponent<Button>();
            subtract1000Button = TransformUtils.FindDeepChild(contentWrapper, "Subtract1000Button").GetComponent<Button>();

            subtract1000Button.onClick.AddListener(Subtract1000FromBalance);
            add1000Button.onClick.AddListener(Add1000ToBalance);

            addTestNotificationButton = TransformUtils.FindDeepChild(contentWrapper, "AddTestNotificationButton").GetComponent<Button>();
            addTestNotificationButton.onClick.AddListener(AddTestNotification);

            testSaveButton = TransformUtils.FindDeepChild(contentWrapper, "TestSaveButton").GetComponent<Button>();
            testSaveButton.onClick.AddListener(OnTestSaveButtonClick);

            testLoadButton = TransformUtils.FindDeepChild(contentWrapper, "TestLoadButton").GetComponent<Button>();
            testLoadButton.onClick.AddListener(OnTestLoadButtonClick);
        }

        /*
            Public Interface
        */
        public void Toggle()
        {
            bool isActive = !gameObject.activeSelf;

            gameObject.SetActive(isActive);
            // SetActive doesn't recurse I guess
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.SetActive(isActive);
            }
        }

        /*
            Internals
        */
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
            SaveLoadSystem.SaveToFileDebug();
        }

        void OnTestLoadButtonClick()
        {
            SaveLoadSystem.LoadFromFileDebug();
        }
    }
}