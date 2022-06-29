using System.Collections;
using System.Collections.Generic;

using TowerBuilder;
using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Wallet
{
    public class State
    {
        public struct Input
        {
            public int? balance;
        }

        public int balance { get; private set; } = 1000000;

        public delegate void BalanceUpdatedEvent(int newBalance, int prevBalance);
        public BalanceUpdatedEvent onBalanceUpdated;

        public State(Input input)
        {
            balance = input.balance ?? 1000000;
        }

        public State() : this(new Input()) { }

        public void UpdateBalance(int balance)
        {
            int prevBalance = this.balance;
            this.balance = balance;

            if (onBalanceUpdated != null)
            {
                onBalanceUpdated(this.balance, prevBalance);
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
