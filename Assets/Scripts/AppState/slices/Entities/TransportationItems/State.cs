using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.TransportationItems
{
    public class State : EntityStateSlice<TransportationItem, State.Events>
    {
        public class Input
        {
            public ListWrapper<TransportationItem> transportationItemsList;
        }

        public new class Events : EntityStateSlice<TransportationItem, State.Events>.Events { }

        public new Queries queries { get; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public new class Queries : EntityStateSlice<TransportationItem, State.Events>.Queries
        {
            public Queries(AppState appState, State state) : base(appState, state) { }

            public ListWrapper<TransportationItem> FindAtCell(CellCoordinates cellCoordinates) =>
                new ListWrapper<TransportationItem>(
                    state.list.items.FindAll(transportationItem =>
                        transportationItem.cellCoordinatesList.Contains(cellCoordinates)
                    ).ToList()
                );

            public ListWrapper<TransportationItem> FindTransportationItemsEnterableFromRoom(Room room) =>
                new ListWrapper<TransportationItem>(
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