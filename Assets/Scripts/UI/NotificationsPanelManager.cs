using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TowerBuilder.Stores;
using TowerBuilder.Stores.Notifications;

namespace TowerBuilder.UI
{
    public class NotificationsPanelManager : MonoBehaviour
    {
        Button button;
        Text text;

        NotificationsStore notificationsStore;

        void Awake()
        {
            notificationsStore = Registry.storeRegistry.notificationsStore;
            NotificationsStore.Events.onNotificationsStateUpdated += OnNotificationsStateUpdated;

            button = transform.Find("Button").GetComponent<Button>();
            text = transform.Find("NotificationsText").GetComponent<Text>();
            text.text = "";

            button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            List<string> notifications = NotificationsStore.Selectors.getNotificationsList(Registry.storeRegistry);

            NotificationsStore.Mutations.createNotification(Registry.storeRegistry, "new message " + (notifications.Count + 1));
        }

        void OnNotificationsStateUpdated(NotificationsStore.NotificationsStateEventPayload payload)
        {
            List<string> notifications = NotificationsStore.Selectors.getNotificationsList(Registry.storeRegistry);
            notifications.Reverse();
            string newText = String.Join("\n", notifications.ToArray());
            text.text = newText;
        }
    }
}