using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class NotificationsPanelManager : MonoBehaviour
    {
        static int NOTIFICATIONS_LIMIT = 3;

        Button button;
        Text text;

        void Awake()
        {
            Registry.appState.Notifications.onNotificationAdded += OnNotificationAdded;

            button = transform.Find("Button").GetComponent<Button>();
            text = transform.Find("NotificationsText").GetComponent<Text>();
            text.text = "";

            button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            int notificationsLength = Registry.appState.Notifications.notifications.Count;
            Notification[] notifications = new Notification[notificationsLength];
            Registry.appState.Notifications.notifications.CopyTo(notifications);

            Registry.appState.Notifications.createNotification("new message " + (notificationsLength + 1));
        }

        void OnNotificationAdded(Notification newNotification)
        {
            int notificationsLength = Registry.appState.Notifications.notifications.Count;
            List<Notification> notifications = Registry.appState.Notifications.notifications;
            // Get the n most recent notifications
            List<Notification> displayNotifications = Enumerable.Reverse(notifications).Take(NOTIFICATIONS_LIMIT).ToList();

            string newText = "";
            foreach (Notification notification in displayNotifications)
            {
                newText += notification.message + "\n";
            }
            text.text = newText;
        }
    }
}