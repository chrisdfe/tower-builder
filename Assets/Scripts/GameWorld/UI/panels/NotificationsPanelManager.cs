using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.Utils;
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
            Registry.appState.Notifications.onItemsAdded += OnNotificationsAdded;
            text = TransformUtils.FindDeepChild(transform, "NotificationsText").GetComponent<Text>();
            text.text = "";
        }

        void OnNotificationsAdded(ListWrapper<Notification> newNotifications)
        {
            ListWrapper<Notification> allNotifications = Registry.appState.Notifications.list;
            int notificationsLength = allNotifications.Count;

            // TODO - why don't I need Enumerable.Reverse here?
            // Get the n most recent notifications
            ListWrapper<Notification> displayNotifications = new ListWrapper<Notification>(
                    // Enumerable.Reverse(allNotifications.items)
                    allNotifications.items
                        .Take(NOTIFICATIONS_LIMIT)
                        .ToList()
            );

            string newText = String.Join(
                "\n\n",
                displayNotifications.items
                    .Select(notification => $"{notification.message}")
            );

            text.text = newText;
        }
    }
}