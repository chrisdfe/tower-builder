using System.Collections;
using System.Collections.Generic;

using TowerBuilder;
using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Wallet
{
    public partial class WalletStore : TowerBuilder.Stores.StoreBase<TowerBuilder.Stores.Wallet.WalletState>
    {
        public WalletStore()
        {
            state.balance = WalletConstants.STARTING_BALANCE;
        }
    }
}
