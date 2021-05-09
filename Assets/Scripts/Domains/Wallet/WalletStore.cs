using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Wallet
{
    public class WalletStore : StoreBase<WalletState>
    {
        public WalletStore()
        {
            state.balance = WalletConstants.STARTING_BALANCE;
        }
    }
}
