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
        static int NOTIFICATIONS_LIMIT = 10;

        Text text;

        void Awake()
        {
            Registry.appState.Notifications.events.onItemsAdded += OnNotificationsAdded;
            text = transform.Find("NotificationsText").GetComponent<Text>();
            text.text = "";
        }

        void OnNotificationsAdded(NotificationsList newNotifications)
        {
            NotificationsList allNotifications = Registry.appState.Notifications.list;
            int notificationsLength = allNotifications.Count;

            // TODO - why don't I need Enumerable.Reverse here?
            // Get the n most recent notifications
            NotificationsList displayNotifications = new NotificationsList(
                    // Enumerable.Reverse(allNotifications.items)
                    allNotifications.items
                        .Take(NOTIFICATIONS_LIMIT)
                        .ToList()
            );

            string newText = "";
            foreach (Notification notification in displayNotifications.items)
            {
                newText += notification.message + "\n\n";
            }

            text.text = newText;
        }
    }
}