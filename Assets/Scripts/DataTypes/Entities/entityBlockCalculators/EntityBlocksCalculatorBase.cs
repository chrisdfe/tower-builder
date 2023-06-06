using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public abstract class EntityBlocksCalculatorBase
    {
        protected EntityDefinition definition;

        protected Distance incrementAmount =>
            new Distance(
                definition.staticBlockSize ? definition.blockCellsTemplate.width : 1,
                definition.staticBlockSize ? definition.blockCellsTemplate.floorSpan : 1
            );

        public EntityBlocksCalculatorBase(EntityDefinition definition)
        {
            this.definition = definition;
        }

        public abstract CellCoordinatesBlockList CalculateFromSelectionBox(SelectionBox selectionBox);

        protected CellCoordinatesBlock CreateBlockAt(CellCoordinates cellCoordinates)
        {
            CellCoordinatesBlock blockCells = new CellCoordinatesBlock(definition.blockCellsTemplate.Clone());
            blockCells.PositionAtCoordinates(cellCoordinates);
            return blockCells;
        }
    }
}