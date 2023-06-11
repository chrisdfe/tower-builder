using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Behaviors.Residents;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Residents
{
    public class ResidentDefinitionsList : EntityDefinitionsList
    {
        public override List<EntityDefinition> Definitions { get; } = new List<EntityDefinition>()
        {
            new ResidentDefinition() {
                key = "Default",
                title = "Default",
                category = "Default",

                blockCellsTemplate = CellCoordinatesList.CreateRectangle(1, 2),
            },

            new ResidentDefinition() {
                key = "OtherDefault",
                title = "OtherDefault",
                category = "OtherDefault",

                blockCellsTemplate = CellCoordinatesList.CreateRectangle(2, 3)
            },
        };

        public ResidentDefinitionsList() : base() { }
    }
}