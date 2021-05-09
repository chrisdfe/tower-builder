using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Wallet
{
    public static class WalletSelectors
    {
        public static int getBalance(StoreRegistry storeRegistry)
        {
            return storeRegistry.walletStore.state.balance;
        }
    }
}