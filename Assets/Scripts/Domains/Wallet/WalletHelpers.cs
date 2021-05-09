using System;

namespace TowerBuilder.Domains.Wallet
{
    public static class WalletHelpers
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