using System;

namespace TowerBuilder.Stores.Wallet
{
    public partial class WalletStore
    {
        public static class Mutations
        {
            public static void updateBalance(int balance)
            {
                WalletStore walletStore = Registry.Stores.walletStore;
                WalletState previousState = walletStore.state;
                walletStore.state.balance = balance;
                WalletState state = walletStore.state;

                if (WalletStore.Events.onWalletStateUpdated != null)
                {
                    WalletStore.Events.onWalletStateUpdated(new WalletStore.StateEventPayload()
                    {
                        previousState = previousState,
                        state = state
                    });
                }
            }

            public static void addBalance(int amount)
            {
                WalletStore walletStore = Registry.Stores.walletStore;
                WalletState state = walletStore.state;
                int newBalance = WalletStore.Helpers.addBalance(walletStore.state.balance, amount);
                updateBalance(newBalance);

                if (WalletStore.Events.onWalletBalanceIncreased != null)
                {
                    WalletStore.Events.onWalletBalanceIncreased(new WalletStore.BalanceEventPayload()
                    {
                        balance = walletStore.state.balance
                    });
                }
            }

            public static void subtractBalance(int amount)
            {
                WalletStore walletStore = Registry.Stores.walletStore;
                WalletState state = walletStore.state;
                int newBalance = WalletStore.Helpers.subtractBalance(walletStore.state.balance, amount);
                updateBalance(newBalance);

                if (WalletStore.Events.onWalletBalanceIncreased != null)
                {
                    WalletStore.Events.onWalletBalanceIncreased(new WalletStore.BalanceEventPayload()
                    {
                        balance = walletStore.state.balance
                    });
                }
            }
        }
    }
}