using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Behaviors.Residents;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class ResidentDefinitionsList : EntityDefinitionsList<Resident.Key, ResidentDefinition>
    {
        public override List<ResidentDefinition> Definitions { get; } = new List<ResidentDefinition>()
        {
            new ResidentDefinition() {
                key = Resident.Key.Default,
                title = "Default",
                category = "Default",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),

                // ResidentBehaviorFactory = (AppState appState, Resident Resident) => new CockpitBehavior(appState, Resident),
            },

            new ResidentDefinition() {
                key = Resident.Key.OtherDefault,
                title = "OtherDefault",
                category = "OtherDefault",

                blockCellsTemplate = new CellCoordinatesList(
                    new List<CellCoordinates>() {
                        new CellCoordinates(0, 0),
                    }
                ),

                // ResidentBehaviorFactory = (AppState appState, Resident Resident) => new EngineBehavior(appState, Resident),
            },
        };

        public ResidentDefinitionsList() : base() { }
    }
}