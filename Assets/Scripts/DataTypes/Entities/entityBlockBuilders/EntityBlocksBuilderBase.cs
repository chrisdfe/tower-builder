using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public abstract class EntityBlocksBuilderBase
    {
        protected EntityDefinition definition;

        protected Distance incrementAmount =>
            new Distance(
                definition.staticBlockSize ? definition.blockCellsTemplate.width : 1,
                definition.staticBlockSize ? definition.blockCellsTemplate.floorSpan : 1
            );

        public EntityBlocksBuilderBase(EntityDefinition definition)
        {
            this.definition = definition;
        }

        public CellCoordinatesBlockList Calculate(SelectionBox selectionBox)
        {
            CellCoordinatesBlockList blocksList = CalculateFromSelectionBox(selectionBox);
            // TODO 
            // 1) use a different origin than bottomLeftCoordinates
            blocksList.PositionAtCoordinates(selectionBox.cellCoordinatesList.bottomLeftCoordinates);
            return blocksList;
        }

        protected abstract CellCoordinatesBlockList CalculateFromSelectionBox(SelectionBox selectionBox);

        protected CellCoordinatesBlock CreateBlockAt(CellCoordinates cellCoordinates)
        {
            CellCoordinatesBlock blockCells = new CellCoordinatesBlock(definition.blockCellsTemplate.Clone());
            blockCells.PositionAtCoordinates(cellCoordinates);
            return blockCells;
        }
    }
}