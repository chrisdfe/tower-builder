using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Wallet
{
    public partial class WalletStore
    {
        public static class Selectors
        {
            public static int getBalance(StoreRegistry storeRegistry)
            {
                return storeRegistry.walletStore.state.balance;
            }
        }
    }
}