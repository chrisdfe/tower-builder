using TowerBuilder.DataTypes.TransportationItems;
using UnityEngine;

namespace TowerBuilder.ApplicationState.TransportationItems
{
    public class State : StateSlice
    {
        public class Input
        {
            public TransportationItemsList transportationItemsList;
        }

        public class Events
        {
            public delegate void TransportationItemEvent(TransportationItem transportationItem);
            public TransportationItemEvent onTransportationItemAdded;
            public TransportationItemEvent onTransportationItemBuilt;
            public TransportationItemEvent onTransportationItemRemoved;
        }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }
        }

        public Events events { get; private set; }
        public Queries queries { get; private set; }

        public TransportationItemsList transportationItemsList { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            transportationItemsList = input.transportationItemsList ?? new TransportationItemsList();

            events = new Events();
            queries = new Queries(this);
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public void AddTransportationItem(TransportationItem transportationItem)
        {
            transportationItemsList.Add(transportationItem);

            if (events.onTransportationItemAdded != null)
            {
                events.onTransportationItemAdded(transportationItem);
            }
        }

        public void BuildTransportationItem(TransportationItem transportationItem)
        {
            transportationItem.OnBuild();

            if (events.onTransportationItemBuilt != null)
            {
                events.onTransportationItemBuilt(transportationItem);
            }
        }

        public void RemoveTransportationItem(TransportationItem transportationItem)
        {
            transportationItemsList.Remove(transportationItem);

            if (events.onTransportationItemRemoved != null)
            {
                events.onTransportationItemRemoved(transportationItem);
            }
        }
    }
}