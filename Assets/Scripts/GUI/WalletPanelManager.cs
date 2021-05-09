using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TowerBuilder.Domains;
using TowerBuilder.Domains.Notifications;
using TowerBuilder.Domains.Wallet;

public class WalletPanelManager : MonoBehaviour
{
    Button add1000Button;
    Button subtract1000Button;
    Text balanceText;

    WalletStore WalletStore;

    void Awake()
    {
        WalletStore = Registry.storeRegistry.walletStore;
        WalletEvents.onWalletStateUpdated += OnWalletStateUpdated;

        add1000Button = transform.Find("Add1000Button").GetComponent<Button>();
        subtract1000Button = transform.Find("Subtract1000Button").GetComponent<Button>();

        balanceText = transform.Find("BalanceText").GetComponent<Text>();
        UpdateBalanceText();

        subtract1000Button.onClick.AddListener(Subtract1000FromBalance);
        add1000Button.onClick.AddListener(Add1000ToBalance);
    }

    void Subtract1000FromBalance()
    {
        WalletMutations.subtractBalance(Registry.storeRegistry, 1000);
    }

    void Add1000ToBalance()
    {
        WalletMutations.addBalance(Registry.storeRegistry, 1000);
    }

    void OnWalletStateUpdated(WalletStateEventPayload payload)
    {
        UpdateBalanceText();

        int balanceChange = WalletStateChangeSelectors.getBalanceChange(payload);

        if (balanceChange > 0)
        {
            NotificationsMutations.createNotification(Registry.storeRegistry, "balance increased");
        }
        else if (balanceChange < 0)
        {
            NotificationsMutations.createNotification(Registry.storeRegistry, "balance decreased");
        }

    }

    void UpdateBalanceText()
    {
        int balance = WalletSelectors.getBalance(Registry.storeRegistry);
        balanceText.text = balance.ToString();
    }
}