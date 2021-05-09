using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Wallet
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