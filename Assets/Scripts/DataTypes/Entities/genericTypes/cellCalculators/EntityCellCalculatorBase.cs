using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityCellCalculatorBase
    {
        public (CellCoordinatesList, CellCoordinatesBlockList) CalculateFromSelectionBox(SelectionBox selectionBox, EntityDefinition definition)
        {
            CellCoordinatesList blockStartingCoordinates = new CellCoordinatesList(GetBlockStartingCoordinates(selectionBox, definition));

            blockStartingCoordinates.PositionAtCoordinates(selectionBox.cellCoordinatesList.bottomLeftCoordinates.Clone());

            CellCoordinatesList newCellCoordinatesList = new CellCoordinatesList();
            CellCoordinatesBlockList newBlocksList = new CellCoordinatesBlockList();

            foreach (CellCoordinates startingCoordinates in blockStartingCoordinates.items)
            {
                CellCoordinatesBlock blockCells = new CellCoordinatesBlock(definition.blockCellsTemplate.Clone());
                blockCells.PositionAtCoordinates(startingCoordinates);

                newCellCoordinatesList.Add(blockCells);

                newBlocksList.Add(blockCells);
            }

            return (newCellCoordinatesList, newBlocksList);
        }

        List<CellCoordinates> GetBlockStartingCoordinates(SelectionBox selectionBox, EntityDefinition definition)
        {
            List<CellCoordinates> result = new List<CellCoordinates>();
            Distance incrementAmount = new Distance(
                definition.staticBlockSize ? definition.blockCellsTemplate.width : 1,
                definition.staticBlockSize ? definition.blockCellsTemplate.floorSpan : 1
            );

            Debug.Log(definition.resizability);

            switch (definition.resizability)
            {
                case Resizability.Horizontal:
                    CalculateHorizontal();
                    break;
                case Resizability.Vertical:
                    CalculateVertical();
                    break;
                case Resizability.Flexible:
                    CalculateFlexible();
                    break;
                case Resizability.Diagonal:
                    CalculateDiagonal();
                    break;
                case Resizability.Inflexible:
                    result.Add(CellCoordinates.zero);
                    break;
            }

            return result;

            void CalculateFlexible()
            {
                for (int x = 0; x < selectionBox.cellCoordinatesList.width; x += incrementAmount.x)
                {
                    for (int floor = 0; floor < selectionBox.cellCoordinatesList.floorSpan; floor += incrementAmount.floors)
                    {
                        result.Add(new CellCoordinates(x, floor));
                    }
                }
            }

            void CalculateHorizontal()
            {
                for (int x = 0; x < selectionBox.cellCoordinatesList.width; x += incrementAmount.x)
                {
                    result.Add(new CellCoordinates(x, 0));
                }
            }

            void CalculateVertical()
            {
                for (int floor = 0; floor < selectionBox.cellCoordinatesList.floorSpan; floor += incrementAmount.floors)
                {
                    result.Add(new CellCoordinates(0, floor));
                }
            }

            void CalculateDiagonal()
            {
                int x = 0;
                int floor = 0;

                while (x < selectionBox.cellCoordinatesList.width && floor < selectionBox.cellCoordinatesList.floorSpan)
                {
                    result.Add(new CellCoordinates(x, floor));
                    x += incrementAmount.x;
                    floor += incrementAmount.floors;
                }
            }
        }
    }
}