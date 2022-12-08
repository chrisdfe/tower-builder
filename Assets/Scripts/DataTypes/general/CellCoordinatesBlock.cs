using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    [System.Serializable]
    public class CellCoordinatesBlock : CellCoordinatesList
    {
        public CellCoordinatesBlock(CellCoordinatesList cellCoordinatesList)
        {
            items = cellCoordinatesList.items;
        }

        public CellCoordinatesBlock(CellCoordinates cellCoordinates)
        {
            items.Add(cellCoordinates);
        }
    }
}


