using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Wallet
{
    public static class WalletStateChangeSelectors
    {
        public static int getBalanceChange(WalletStateEventPayload payload)
        {
            WalletState state = payload.state;
            WalletState previousState = payload.previousState;
            return state.balance - previousState.balance;
        }
    }
}