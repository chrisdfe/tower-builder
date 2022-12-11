using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Residents
{
    using ResidentEntityStateSlice = EntityStateSlice<ResidentsList, Resident, State.Events>;

    public class State : ResidentEntityStateSlice
    {
        public class Input
        {
            public ResidentsList residentsList = new ResidentsList();
        }

        public new class Events : ResidentEntityStateSlice.Events
        {
            public ResidentEntityStateSlice.Events.ItemEvent onItemPositionUpdated;
        }

        public StateQueries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            this.list = input.residentsList ?? new ResidentsList();

            queries = new StateQueries(this);
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public void SetResidentPosition(Resident resident, CellCoordinates cellCoordinates)
        {
            resident.cellCoordinatesList = resident.definition.cellCoordinatesList.Clone();
            resident.cellCoordinatesList.PositionAtCoordinates(cellCoordinates);

            events.onItemPositionUpdated?.Invoke(resident);
        }

        public class StateQueries
        {
            State state;

            public StateQueries(State state)
            {
                this.state = state;
            }

            public Resident FindResidentAtCell(CellCoordinates cellCoordinates)
            {
                return state.list.FindResidentAtCell(cellCoordinates);
            }
        }
    }
}