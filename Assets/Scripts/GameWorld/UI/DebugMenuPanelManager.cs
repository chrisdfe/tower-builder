using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Buildings;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.Systems;
using UnityEditor;
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

        void Awake()
        {
            panel = transform.Find("DebugMenuPanel");
            panel.gameObject.SetActive(false);

            add1HourButton = panel.Find("Add1HourButton").GetComponent<Button>();
            add1HourButton.onClick.AddListener(Add1Hour);

            subtract1HourButton = panel.Find("Subtract1HourButton").GetComponent<Button>();
            subtract1HourButton.onClick.AddListener(Subtract1Hour);

            add1000Button = panel.Find("Add1000Button").GetComponent<Button>();
            subtract1000Button = panel.Find("Subtract1000Button").GetComponent<Button>();

            subtract1000Button.onClick.AddListener(Subtract1000FromBalance);
            add1000Button.onClick.AddListener(Add1000ToBalance);

            addTestNotificationButton = panel.Find("AddTestNotificationButton").GetComponent<Button>();
            addTestNotificationButton.onClick.AddListener(AddTestNotification);

            testSaveButton = panel.Find("TestSaveButton").GetComponent<Button>();
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
            Registry.appState.Time.AddTime(new TimeInput()
            {
                hour = 1
            });
        }

        void Subtract1Hour()
        {
            Registry.appState.Time.SubtractTime(new TimeInput()
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
            int notificationsLength = Registry.appState.Notifications.notifications.Count;
            Notification[] notifications = new Notification[notificationsLength];
            Registry.appState.Notifications.notifications.CopyTo(notifications);

            Registry.appState.Notifications.createNotification("new message " + (notificationsLength + 1));
        }

        void OnTestSaveButtonClick()
        {
            if (Registry.appState.Rooms.buildings.Count > 0)
            {
                Building building = Registry.appState.Rooms.buildings.buildings[0];

                if (building.roomList.Count > 0)
                {
                    Room room = building.roomList.rooms[0];

                    SaveLoadSystem.SaveToFile<Room>(room);
                }
            }


            // JsonWriter.WriteString("test.json", jsonifiedRoomStore);
            // JsonWriter.ReadString();
        }
    }
}