using System.Linq;
using TowerBuilder.DataTypes;
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

        public StateQueries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            list = input.transportationItemsList ?? new TransportationItemsList();

            queries = new StateQueries(appState, this);
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public override void Add(TransportationItem entity)
        {
            base.Add(entity);
        }

        public class StateQueries
        {
            AppState appState;
            State state;

            public StateQueries(AppState appState, State state)
            {
                this.appState = appState;
                this.state = state;
            }

            public TransportationItemsList FindAtCell(CellCoordinates cellCoordinates) =>
                new TransportationItemsList(
                    state.list.items.FindAll(transportationItem =>
                        transportationItem.cellCoordinatesList.Contains(cellCoordinates)
                    ).ToList()
                );

            public TransportationItemsList FindTransportationItemsEnterableFromRoom(Room room) =>
                new TransportationItemsList(
                    state.list.items.FindAll(transportationItem =>
                    {
                        foreach (CellCoordinates entranceCoordinates in transportationItem.entranceCellCoordinatesList.items)
                        {
                            Room entranceCellRoom = appState.Entities.Rooms.queries.FindRoomAtCell(entranceCoordinates);
                            if (entranceCellRoom == room)
                            {
                                return true;
                            }
                        }

                        return false;
                    }).ToList()
                );
        }
    }
}