using System.Linq;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.TransportationItems
{
    using TransportationItemsEntityStateSlice = EntityStateSlice<TransportationItemsList, TransportationItem, State.Events>;

    public class State : TransportationItemsEntityStateSlice
    {
        public class Input
        {
            public TransportationItemsList transportationItemsList;
        }

        public new class Events : TransportationItemsEntityStateSlice.Events { }

        public class Queries
        {
            AppState appState;
            State state;

            public Queries(AppState appState, State state)
            {
                this.appState = appState;
                this.state = state;
            }

            public TransportationItemsList FindTransportationItemsConnectingToRoom(Room room) =>
                new TransportationItemsList(
                    state.list.items.FindAll(transportationItem =>
                    {
                        Room entranceCellRoom = appState.Entities.Rooms.queries.FindRoomAtCell(transportationItem.entranceCellCoordinates);
                        Room exitCellRoom = appState.Entities.Rooms.queries.FindRoomAtCell(transportationItem.exitCellCoordinates);
                        return (entranceCellRoom == room || exitCellRoom == room);
                    }).ToList()
                );
        }

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            list = input.transportationItemsList ?? new TransportationItemsList();

            queries = new Queries(appState, this);
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}