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
            List<Notification> notifications = Registry.Stores.Notifications.notifications;

            Registry.Stores.Notifications.createNotification("new message " + (notifications.Count + 1));
        }

        void OnNotificationAdded(Notification newNotification)
        {
            List<string> notifications = (List<string>)Registry.Stores.Notifications.notifications.Select(notification => notification.message);
            notifications.Reverse();
            string newText = String.Join("\n", notifications.ToArray());
            text.text = newText;
        }
    }
}