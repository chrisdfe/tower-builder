using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Residents
{
    public class State : StateSlice
    {
        public class Input
        {
            public ResidentsList allResidents = new ResidentsList();
        }

        public class Events
        {
            public delegate void ResidentsEvent(ResidentsList residents);
            public ResidentsEvent onResidentsAdded;
            public ResidentsEvent onResidentsRemoved;
            public ResidentsEvent onResidentsBuilt;

            public delegate void ResidentEvent(Resident resident);
            public ResidentEvent onResidentPositionUpdated;
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
                return state.allResidents.FindResidentAtCell(cellCoordinates);
            }
        }

        public ResidentsList allResidents { get; private set; } = new ResidentsList();

        public Events events { get; private set; }
        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            if (input == null)
            {
                input = new Input();
            }

            this.allResidents = input.allResidents ?? new ResidentsList();

            events = new Events();
            queries = new Queries(this);
        }

        public void AddResident(Resident resident)
        {
            allResidents.Add(resident);

            if (events.onResidentsAdded != null)
            {
                events.onResidentsAdded(new ResidentsList(resident));
            }
        }

        public void BuildResident(Resident resident)
        {
            resident.OnBuild();

            if (events.onResidentsBuilt != null)
            {
                events.onResidentsBuilt(new ResidentsList(resident));
            }
        }

        public void RemoveResident(Resident resident)
        {
            allResidents.Remove(resident);

            if (events.onResidentsRemoved != null)
            {
                events.onResidentsRemoved(new ResidentsList(resident));
            }
        }

        public void SetResidentPosition(Resident resident, CellCoordinates cellCoordinates)
        {
            resident.cellCoordinates = cellCoordinates;

            if (events.onResidentPositionUpdated != null)
            {
                events.onResidentPositionUpdated(resident);
            }
        }
    }
}