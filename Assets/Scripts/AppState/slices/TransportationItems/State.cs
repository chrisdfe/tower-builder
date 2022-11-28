using System.Linq;
using TowerBuilder.DataTypes.Rooms;
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
            AppState appState;
            State state;

            public Queries(AppState appState, State state)
            {
                this.appState = appState;
                this.state = state;
            }

            public TransportationItemsList FindTransportationItemsConnectingToRoom(Room room)
            {
                return new TransportationItemsList(
                    state.transportationItemsList.items.FindAll(transportationItem =>
                    {
                        Room entranceCellRoom = appState.Rooms.queries.FindRoomAtCell(transportationItem.entranceCellCoordinates);
                        Room exitCellRoom = appState.Rooms.queries.FindRoomAtCell(transportationItem.exitCellCoordinates);
                        return (entranceCellRoom == room || exitCellRoom == room);
                    }).ToList()
                );
            }
        }

        public Events events { get; private set; }
        public Queries queries { get; private set; }

        public TransportationItemsList transportationItemsList { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            transportationItemsList = input.transportationItemsList ?? new TransportationItemsList();

            events = new Events();
            queries = new Queries(appState, this);
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public void AddTransportationItem(TransportationItem transportationItem)
        {
            transportationItemsList.Add(transportationItem);

            // transportationItem.entranceNode.room = appState.Rooms.queries.FindRoomAtCell(transportationItem.entranceNode.cellCoordinates);
            // transportationItem.exitNode.room = appState.Rooms.queries.FindRoomAtCell(transportationItem.exitNode.cellCoordinates);

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