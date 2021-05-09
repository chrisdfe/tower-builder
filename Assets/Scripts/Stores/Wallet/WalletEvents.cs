using System;

namespace TowerBuilder.Stores.Wallet
{
    public partial class WalletStore
    {
        public struct StateEventPayload
        {
            public WalletState state;
            public WalletState previousState;
        }

        public struct BalanceEventPayload
        {
            public int balance;
        }

        public static class Events
        {
            public delegate void OnWalletStateUpdated(StateEventPayload payload);
            public static OnWalletStateUpdated onWalletStateUpdated;

            public delegate void OnWalletBalanceUpdated(BalanceEventPayload payload);
            public static OnWalletBalanceUpdated onWalletBalanceIncreased;
            public static OnWalletBalanceUpdated onWalletBalanceDecreased;
        }
    }
}
