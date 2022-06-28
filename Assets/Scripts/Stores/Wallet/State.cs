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

        public int balance { get; private set; }

        public delegate void BalanceUpdatedEvent(int balance);
        public BalanceUpdatedEvent onBalanceUpdated;


        public State() : this(new Input()) { }

        public State(Input input)
        {
            balance = input.balance ?? 1000000;
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
