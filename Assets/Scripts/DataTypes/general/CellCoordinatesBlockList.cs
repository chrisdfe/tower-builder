using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    [System.Serializable]
    public class CellCoordinatesBlockList : ListWrapper<CellCoordinatesBlock>
    {
        public CellCoordinatesBlockList() : base() { }
        public CellCoordinatesBlockList(CellCoordinatesBlock cellCoordinatesBlock) : base(cellCoordinatesBlock) { }
        public CellCoordinatesBlockList(List<CellCoordinatesBlock> cellCoordinatesBlocks) : base(cellCoordinatesBlocks) { }
        public CellCoordinatesBlockList(CellCoordinatesBlockList cellCoordinatesBlockList) : base(cellCoordinatesBlockList) { }

        public CellCoordinatesList Flatten() =>
        items.Aggregate(new CellCoordinatesList(), (acc, cellCoordinatesBlock) =>
            {
                acc.Add(cellCoordinatesBlock.items);
                return acc;
            });
    }
}


