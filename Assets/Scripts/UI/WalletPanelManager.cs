using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Notifications;
using TowerBuilder.Stores.Wallet;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.UI
{
    public class WalletPanelManager : MonoBehaviour
    {
        Button add1000Button;
        Button subtract1000Button;
        Text balanceText;

        WalletStore WalletStore;

        void Awake()
        {
            WalletStore = Registry.Stores.walletStore;
            WalletStore.Events.onWalletStateUpdated += OnWalletStateUpdated;

            add1000Button = transform.Find("Add1000Button").GetComponent<Button>();
            subtract1000Button = transform.Find("Subtract1000Button").GetComponent<Button>();

            balanceText = transform.Find("BalanceText").GetComponent<Text>();
            UpdateBalanceText();

            subtract1000Button.onClick.AddListener(Subtract1000FromBalance);
            add1000Button.onClick.AddListener(Add1000ToBalance);
        }

        void Subtract1000FromBalance()
        {
            WalletStore.Mutations.subtractBalance(1000);
        }

        void Add1000ToBalance()
        {
            WalletStore.Mutations.addBalance(1000);
        }

        void OnWalletStateUpdated(WalletStore.StateEventPayload payload)
        {
            UpdateBalanceText();

            int balanceChange = WalletStore.StateChangeSelectors.getBalanceChange(payload);

            if (balanceChange > 0)
            {
                Registry.Stores.Notifications.createNotification("balance increased");
            }
            else if (balanceChange < 0)
            {
                Registry.Stores.Notifications.createNotification("balance decreased");
            }

        }

        void UpdateBalanceText()
        {
            int balance = WalletStore.Selectors.getBalance(Registry.Stores);
            balanceText.text = balance.ToString();
        }
    }
}