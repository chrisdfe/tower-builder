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
                definition.staticBlockSize ? definition.blockCellsTemplate.height : 1
            );

        public EntityBlocksBuilderBase(EntityDefinition definition)
        {
            this.definition = definition;
        }

        public static EntityBlocksBuilderBase FromDefinition(EntityDefinition definition) =>
            definition.resizability switch
            {
                Resizability.Horizontal =>
                    new HorizontalResizabilityEntityBlocksBuilder(definition),
                Resizability.Vertical =>
                    new VerticalResizabilityEntityBlocksBuilder(definition),
                Resizability.Diagonal =>
                    new DiagonalResizabilityEntityBlocksBuilder(definition),
                Resizability.Flexible =>
                    new FlexibleResizabilityEntityBlocksBuilder(definition),
                Resizability.Inflexible =>
                    new InflexibleResizabilityEntityBlocksBuilder(definition),
                _ => null
            };

        public abstract CellCoordinatesBlockList CalculateFromSelectionBox(SelectionBox selectionBox);

        protected CellCoordinatesBlock CreateBlockAt(CellCoordinates cellCoordinates)
        {
            CellCoordinatesBlock blockCells = new CellCoordinatesBlock(definition.blockCellsTemplate.Clone());
            blockCells.PositionAtCoordinates(cellCoordinates);
            return blockCells;
        }
    }
}