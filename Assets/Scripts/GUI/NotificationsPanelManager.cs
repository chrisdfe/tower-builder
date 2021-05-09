using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TowerBuilder.Domains;
using TowerBuilder.Domains.Notifications;

public class NotificationsPanelManager : MonoBehaviour
{
    Button button;
    Text text;


    NotificationsStore notificationsStore;

    void Awake()
    {
        notificationsStore = Registry.storeRegistry.notificationsStore;
        NotificationsEvents.onNotificationsStateUpdated += OnNotificationsStateUpdated;

        button = transform.Find("Button").GetComponent<Button>();
        text = transform.Find("NotificationsText").GetComponent<Text>();
        text.text = "";

        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        List<string> notifications = NotificationsSelectors.getNotificationsList(Registry.storeRegistry);

        NotificationsMutations.createNotification(Registry.storeRegistry, new NotificationInput()
        {
            message = "new message " + (notifications.Count + 1)
        });
    }

    void UpdateNotificationsText()
    {
    }

    void OnNotificationsStateUpdated(NotificationsStateEventPayload payload)
    {
        List<string> notifications = NotificationsSelectors.getNotificationsList(Registry.storeRegistry);
        notifications.Reverse();
        string newText = String.Join("\n", notifications.ToArray());
        text.text = newText;
    }
}