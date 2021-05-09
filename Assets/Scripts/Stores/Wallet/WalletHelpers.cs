using System;

namespace TowerBuilder.Stores.Wallet
{
    public partial class WalletStore
    {
        public static class Helpers
        {
            public static int addBalance(int balance, int amount)
            {
                return balance + amount;
            }

            public static int subtractBalance(int balance, int amount)
            {
                return balance - amount;
            }
        }
    }
}