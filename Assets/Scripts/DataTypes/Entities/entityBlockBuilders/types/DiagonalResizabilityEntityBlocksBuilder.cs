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
            int floor = 0;

            while (x < selectionBox.cellCoordinatesList.width && floor < selectionBox.cellCoordinatesList.floorSpan)
            {
                CellCoordinatesBlock block = CreateBlockAt(new CellCoordinates(x, floor));
                result.Add(block);

                x += incrementAmount.x;
                floor += incrementAmount.floors;
            }

            return result;
        }
    }
}