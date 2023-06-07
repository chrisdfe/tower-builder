using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class VerticalResizabilityEntityBlocksBuilder : EntityBlocksBuilderBase
    {
        public VerticalResizabilityEntityBlocksBuilder(EntityDefinition definition) : base(definition) { }

        protected override CellCoordinatesBlockList CalculateFromSelectionBox(SelectionBox selectionBox)
        {
            CellCoordinatesBlockList result = new CellCoordinatesBlockList();

            for (int floor = 0; floor < selectionBox.cellCoordinatesList.floorSpan; floor += incrementAmount.floors)
            {
                CellCoordinatesBlock block = CreateBlockAt(new CellCoordinates(0, floor));
                result.Add(block);
            }

            return result;
        }
    }
}