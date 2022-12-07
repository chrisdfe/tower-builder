using System.Linq;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using UnityEngine;

namespace TowerBuilder.ApplicationState.TransportationItems
{
    using TransportationItemsListStateSlice = ListStateSlice<TransportationItemsList, TransportationItem, State.Events>;

    public class State : TransportationItemsListStateSlice
    {
        public class Input
        {
            public TransportationItemsList transportationItemsList;
        }

        public new class Events : TransportationItemsListStateSlice.Events
        {
            public TransportationItemsListStateSlice.Events.ItemEvent onItemBuilt;
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
                    state.list.items.FindAll(transportationItem =>
                    {
                        Room entranceCellRoom = appState.Rooms.queries.FindRoomAtCell(transportationItem.entranceCellCoordinates);
                        Room exitCellRoom = appState.Rooms.queries.FindRoomAtCell(transportationItem.exitCellCoordinates);
                        return (entranceCellRoom == room || exitCellRoom == room);
                    }).ToList()
                );
            }
        }

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            list = input.transportationItemsList ?? new TransportationItemsList();

            queries = new Queries(appState, this);
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public void Build(TransportationItem transportationItem)
        {
            transportationItem.OnBuild();

            events.onItemBuilt?.Invoke(transportationItem);
        }
    }
}