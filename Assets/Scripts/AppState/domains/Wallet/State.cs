using System.Collections;
using System.Collections.Generic;

using TowerBuilder;
using TowerBuilder.State;

namespace TowerBuilder.State.Wallet
{
    public class State
    {
        static int DEFAULT_STARTING_BALANCE = 1000000;
        // static int DEFAULT_STARTING_BALANCE = 1000;

        public struct Input
        {
            public int? balance;
        }

        public int balance { get; private set; } = DEFAULT_STARTING_BALANCE;

        public delegate void BalanceUpdatedEvent(int newBalance, int prevBalance);
        public BalanceUpdatedEvent onBalanceUpdated;

        public State(Input input)
        {
            balance = input.balance ?? DEFAULT_STARTING_BALANCE;
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
