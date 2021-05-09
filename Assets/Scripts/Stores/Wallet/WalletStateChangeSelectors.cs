using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Wallet
{
    public partial class WalletStore
    {
        public static class StateChangeSelectors
        {
            public static int getBalanceChange(WalletStore.StateEventPayload payload)
            {
                WalletState state = payload.state;
                WalletState previousState = payload.previousState;
                return state.balance - previousState.balance;
            }
        }
    }
}