using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class InflexibleResizabilityEntityBlocksBuilder : EntityBlocksBuilderBase
    {
        public InflexibleResizabilityEntityBlocksBuilder(EntityDefinition definition) : base(definition) { }

        public override CellCoordinatesBlockList CalculateFromSelectionBox(SelectionBox selectionBox)
        {
            CellCoordinatesBlockList result = new CellCoordinatesBlockList();

            CellCoordinatesBlock block = CreateBlockAt(new CellCoordinates(0, 0));
            result.Add(block);

            return result;
        }
    }
}