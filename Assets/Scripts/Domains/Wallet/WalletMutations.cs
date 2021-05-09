using System;

namespace TowerBuilder.Domains.Wallet
{
    public static class WalletMutations
    {
        public static void updateBalance(StoreRegistry storeRegistry, int balance)
        {
            WalletStore walletStore = storeRegistry.walletStore;
            WalletState previousState = walletStore.state;
            walletStore.state.balance = balance;
            WalletState state = walletStore.state;

            if (WalletEvents.onWalletStateUpdated != null)
            {
                WalletEvents.onWalletStateUpdated(new WalletStateEventPayload()
                {
                    previousState = previousState,
                    state = state
                });
            }
        }

        public static void addBalance(StoreRegistry storeRegistry, int amount)
        {
            WalletStore walletStore = storeRegistry.walletStore;
            WalletState state = walletStore.state;
            int newBalance = WalletHelpers.addBalance(walletStore.state.balance, amount);
            updateBalance(storeRegistry, newBalance);

            if (WalletEvents.onWalletBalanceIncreased != null)
            {
                WalletEvents.onWalletBalanceIncreased(new WalletBalanceEventPayload()
                {
                    balance = walletStore.state.balance
                });
            }
        }

        public static void subtractBalance(StoreRegistry storeRegistry, int amount)
        {
            WalletStore walletStore = storeRegistry.walletStore;
            WalletState state = walletStore.state;
            int newBalance = WalletHelpers.subtractBalance(walletStore.state.balance, amount);
            updateBalance(storeRegistry, newBalance);

            if (WalletEvents.onWalletBalanceIncreased != null)
            {
                WalletEvents.onWalletBalanceIncreased(new WalletBalanceEventPayload()
                {
                    balance = walletStore.state.balance
                });
            }
        }
    }
}