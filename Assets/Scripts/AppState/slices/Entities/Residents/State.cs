using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Residents
{
    public class State : EntityStateSlice<Resident, State.Events>
    {
        public class Input
        {
            public ListWrapper<Resident> residentsList = new ListWrapper<Resident>();
        }

        public new class Events : EntityStateSlice<Resident, State.Events>.Events
        {
            public ItemEvent<Resident> onItemPositionUpdated;
        }

        public new Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public void SetResidentPosition(Resident resident, CellCoordinates cellCoordinates)
        {
            resident.cellCoordinatesList = resident.definition.blockCellsTemplate.Clone();
            resident.cellCoordinatesList.PositionAtCoordinates(cellCoordinates);

            events.onItemPositionUpdated?.Invoke(resident);
        }

        public new class Queries : EntityStateSlice<Resident, State.Events>.Queries
        {
            public Queries(AppState appState, State state) : base(appState, state)
            {
            }
        }
    }
}