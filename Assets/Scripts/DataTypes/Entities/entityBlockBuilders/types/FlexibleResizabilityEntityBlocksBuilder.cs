using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class FlexibleResizabilityEntityBlocksBuilder : EntityBlocksBuilderBase
    {
        public FlexibleResizabilityEntityBlocksBuilder(EntityDefinition definition) : base(definition) { }

        public override CellCoordinatesBlockList CalculateFromSelectionBox(SelectionBox selectionBox)
        {
            CellCoordinatesBlockList result = new CellCoordinatesBlockList();

            for (int x = 0; x < selectionBox.cellCoordinatesList.width; x += incrementAmount.x)
            {
                for (int y = 0; y < selectionBox.cellCoordinatesList.height; y += incrementAmount.y)
                {
                    CellCoordinatesBlock block = CreateBlockAt(new CellCoordinates(x, y));
                    result.Add(block);
                }
            }

            return result;
        }
    }
}