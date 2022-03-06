using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Notifications;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.UI
{
    public class NotificationsPanelManager : MonoBehaviour
    {
        Button button;
        Text text;

        // TODO - why does it have to be Stores.Notification and not just Notifications?
        Stores.Notifications.State notificationsStore;

        void Awake()
        {
            notificationsStore = Registry.Stores.Notifications;
            Registry.Stores.Notifications.onNotificationAdded += OnNotificationAdded;

            button = transform.Find("Button").GetComponent<Button>();
            text = transform.Find("NotificationsText").GetComponent<Text>();
            text.text = "";

            button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            int notificationsLength = Registry.Stores.Notifications.notifications.Count;
            Notification[] notifications = new Notification[notificationsLength];
            Registry.Stores.Notifications.notifications.CopyTo(notifications);

            Registry.Stores.Notifications.createNotification("new message " + (notificationsLength + 1));
        }

        void OnNotificationAdded(Notification newNotification)
        {

            int notificationsLength = Registry.Stores.Notifications.notifications.Count;
            Notification[] notifications = new Notification[notificationsLength];
            Registry.Stores.Notifications.notifications.CopyTo(notifications);
            notifications.Reverse();
            string newText = "";
            foreach (Notification notification in notifications)
            {
                newText += notification.message + "\n";
            }
            text.text = newText;
        }
    }
}