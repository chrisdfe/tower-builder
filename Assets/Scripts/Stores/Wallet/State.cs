using System.Collections;
using System.Collections.Generic;

using TowerBuilder;
using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Wallet
{
    public class State
    {
        public int balance { get; private set; }

        public delegate void BalanceUpdatedEvent(int balance);
        public BalanceUpdatedEvent onBalanceUpdated;

        public State()
        {
            balance = Constants.STARTING_BALANCE;
        }

        public void UpdateBalance(int balance)
        {
            this.balance = balance;

            if (onBalanceUpdated != null)
            {
                onBalanceUpdated(balance);
            }
        }

        public void AddBalance(int amount)
        {
            int newBalance = balance + amount;
            UpdateBalance(newBalance);
        }

        public void SubtractBalance(int amount)
        {
            int newBalance = balance - amount;
            UpdateBalance(newBalance);
        }
    }
}
