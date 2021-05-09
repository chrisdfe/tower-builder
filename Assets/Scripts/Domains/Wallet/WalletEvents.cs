using System;

namespace TowerBuilder.Domains.Wallet
{
    public struct WalletStateEventPayload
    {
        public WalletState state;
        public WalletState previousState;
    };

    public struct WalletBalanceEventPayload
    {
        public int balance;
    }
    public static class WalletEvents
    {
        public delegate void OnWalletStateUpdated(WalletStateEventPayload payload);
        public static OnWalletStateUpdated onWalletStateUpdated;

        public delegate void OnWalletBalanceUpdated(WalletBalanceEventPayload payload);
        public static OnWalletBalanceUpdated onWalletBalanceIncreased;
        public static OnWalletBalanceUpdated onWalletBalanceDecreased;

    }
}