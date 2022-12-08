using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Residents
{
    using ResidentsListStateSlice = ListStateSlice<ResidentsList, Resident, State.Events>;

    public class State : ResidentsListStateSlice
    {
        public class Input
        {
            public ResidentsList residentsList = new ResidentsList();
        }

        public new class Events : ResidentsListStateSlice.Events
        {
            public ResidentsListStateSlice.Events.ItemEvent onItemPositionUpdated;
        }

        public class Queries
        {
            State state;

            public Queries(State state)
            {
                this.state = state;
            }

            public Resident FindResidentAtCell(CellCoordinates cellCoordinates)
            {
                return state.list.FindResidentAtCell(cellCoordinates);
            }
        }

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            if (input == null)
            {
                input = new Input();
            }

            this.list = input.residentsList ?? new ResidentsList();

            queries = new Queries(this);
        }

        public void BuildResident(Resident resident)
        {
            resident.OnBuild();

            events.onItemsBuilt?.Invoke(new ResidentsList(resident));
        }

        public void SetResidentPosition(Resident resident, CellCoordinates cellCoordinates)
        {
            resident.cellCoordinates = cellCoordinates;

            events.onItemPositionUpdated?.Invoke(resident);
        }
    }
}