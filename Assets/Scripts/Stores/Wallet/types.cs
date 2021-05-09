using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Wallet
{
    public struct WalletTransaction
    {
        int amount;
        string description;
    }

    public struct WalletState
    {
        public int balance;
    }
}