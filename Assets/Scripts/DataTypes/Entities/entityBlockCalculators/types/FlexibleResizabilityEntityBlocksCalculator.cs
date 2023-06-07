using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class FlexibleResizabilityEntityBlocksCalculator : EntityBlocksCalculatorBase
    {
        public FlexibleResizabilityEntityBlocksCalculator(EntityDefinition definition) : base(definition) { }

        protected override CellCoordinatesBlockList CalculateFromSelectionBox(SelectionBox selectionBox)
        {
            CellCoordinatesBlockList result = new CellCoordinatesBlockList();

            for (int x = 0; x < selectionBox.cellCoordinatesList.width; x += incrementAmount.x)
            {
                for (int floor = 0; floor < selectionBox.cellCoordinatesList.floorSpan; floor += incrementAmount.floors)
                {
                    CellCoordinatesBlock block = CreateBlockAt(new CellCoordinates(x, floor));
                    result.Add(block);
                }
            }

            return result;
        }
    }
}