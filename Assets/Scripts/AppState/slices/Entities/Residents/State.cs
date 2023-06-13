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
    public class State : EntityStateSlice
    {
        public class Input
        {
            public ListWrapper<Resident> residentsList = new ListWrapper<Resident>();
        }

        /*
            Events
        */
        public ItemEvent<Resident> onItemPositionUpdated;

        public State(AppState appState, Input input) : base(appState)
        {
        }

        public State(AppState appState) : this(appState, new Input()) { }

        /*
            Public API
        */
        public void SetResidentPosition(Resident resident, CellCoordinates cellCoordinates)
        {
            // resident.cellCoordinatesList = resident.definition.blockCellsTemplate.Clone();
            resident.offsetCoordinates = cellCoordinates;

            onItemPositionUpdated?.Invoke(resident);
        }
    }
}