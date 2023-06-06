using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class VerticalResizabilityEntityBlocksCalculator : EntityBlocksCalculatorBase
    {
        public VerticalResizabilityEntityBlocksCalculator(EntityDefinition definition) : base(definition) { }

        public override CellCoordinatesBlockList CalculateFromSelectionBox(SelectionBox selectionBox)
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