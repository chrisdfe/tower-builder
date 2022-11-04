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

        Text text;

        void Awake()
        {
            Registry.appState.Notifications.events.onNotificationAdded += OnNotificationAdded;
            text = transform.Find("NotificationsText").GetComponent<Text>();
            text.text = "";
        }

        void OnNotificationAdded(Notification newNotification)
        {
            List<Notification> allNotifications = Registry.appState.Notifications.allNotifications;
            int notificationsLength = allNotifications.Count;

            // Get the n most recent notifications
            List<Notification> displayNotifications = Enumerable.Reverse(allNotifications).Take(NOTIFICATIONS_LIMIT).ToList();

            string newText = "";
            foreach (Notification notification in displayNotifications)
            {
                newText += notification.message + "\n";
            }
            text.text = newText;
        }
    }
}