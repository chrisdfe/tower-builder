using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class HorizontalResizabilityEntityBlocksCalculator : EntityBlocksCalculatorBase
    {
        public HorizontalResizabilityEntityBlocksCalculator(EntityDefinition definition) : base(definition) { }

        public override CellCoordinatesBlockList CalculateFromSelectionBox(SelectionBox selectionBox)
        {
            CellCoordinatesBlockList result = new CellCoordinatesBlockList();

            for (int x = 0; x < selectionBox.cellCoordinatesList.width; x += incrementAmount.x)
            {
                CellCoordinatesBlock block = CreateBlockAt(new CellCoordinates(x, 0));
                result.Add(block);
            }

            return result;
        }
    }
}