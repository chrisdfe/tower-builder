using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class DiagonalResizabilityEntityBlocksBuilder : EntityBlocksBuilderBase
    {
        public DiagonalResizabilityEntityBlocksBuilder(EntityDefinition definition) : base(definition) { }

        protected override CellCoordinatesBlockList CalculateFromSelectionBox(SelectionBox selectionBox)
        {
            CellCoordinatesBlockList result = new CellCoordinatesBlockList();

            int x = 0;
            int y = 0;

            while (x < selectionBox.cellCoordinatesList.width && y < selectionBox.cellCoordinatesList.height)
            {
                CellCoordinatesBlock block = CreateBlockAt(new CellCoordinates(x, y));
                result.Add(block);

                x += incrementAmount.x;
                y += incrementAmount.y;
            }

            return result;
        }
    }
}