using System.Collections;
using System.Collections.Generic;

using TowerBuilder;
using TowerBuilder.ApplicationState;

namespace TowerBuilder.ApplicationState.Wallet
{
    public class State : StateSlice
    {
        static int DEFAULT_STARTING_BALANCE = 1000000;

        public struct Input
        {
            public int? balance;
        }

        public int balance { get; private set; } = DEFAULT_STARTING_BALANCE;

        public delegate void BalanceUpdatedEvent(int newBalance, int prevBalance);
        public BalanceUpdatedEvent onBalanceUpdated;

        public State(AppState appState, Input input) : base(appState)
        {
            balance = input.balance ?? DEFAULT_STARTING_BALANCE;
        }
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
