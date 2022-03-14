using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Notifications;
using TowerBuilder.Stores.Wallet;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class WalletPanelManager : MonoBehaviour
    {
        Button add1000Button;
        Button subtract1000Button;
        Text balanceText;

        void Awake()
        {
            Registry.Stores.Wallet.onBalanceUpdated += OnBalanceUpdated;

            add1000Button = transform.Find("Add1000Button").GetComponent<Button>();
            subtract1000Button = transform.Find("Subtract1000Button").GetComponent<Button>();

            balanceText = transform.Find("BalanceText").GetComponent<Text>();
            UpdateBalanceText();

            subtract1000Button.onClick.AddListener(Subtract1000FromBalance);
            add1000Button.onClick.AddListener(Add1000ToBalance);
        }

        void Subtract1000FromBalance()
        {
            Registry.Stores.Wallet.SubtractBalance(1000);
        }

        void Add1000ToBalance()
        {
            Registry.Stores.Wallet.AddBalance(1000);
        }

        void OnBalanceUpdated(int amount)
        {
            UpdateBalanceText();
        }

        void UpdateBalanceText()
        {
            int balance = Registry.Stores.Wallet.balance;
            balanceText.text = balance.ToString();
        }
    }
}