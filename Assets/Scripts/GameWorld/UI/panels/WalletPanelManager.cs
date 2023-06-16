using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Notifications;
using TowerBuilder.ApplicationState.Wallet;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class WalletPanelManager : MonoBehaviour
    {
        Text balanceText;

        void Awake()
        {
            Registry.appState.Wallet.onBalanceUpdated += OnBalanceUpdated;

            balanceText = TransformUtils.FindDeepChild(transform, "Text").GetComponent<Text>();
            UpdateBalanceText();
        }

        void OnBalanceUpdated(int newBalance, int prevBalance)
        {
            UpdateBalanceText();
        }

        void UpdateBalanceText()
        {
            int balance = Registry.appState.Wallet.balance;
            string formattedBalance = String.Format("${0:n0}", balance);
            balanceText.text = formattedBalance;
        }
    }
}