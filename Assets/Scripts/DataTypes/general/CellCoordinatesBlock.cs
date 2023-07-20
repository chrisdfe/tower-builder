using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class CellCoordinatesBlock : CellCoordinatesList
    {
        public CellCoordinatesBlock(List<CellCoordinates> cellCoordinatesList)
        {
            items = cellCoordinatesList;
        }

        public CellCoordinatesBlock(CellCoordinatesList cellCoordinatesList) : this(cellCoordinatesList.items) { }

        public CellCoordinatesBlock(CellCoordinates cellCoordinates)
        {
            items.Add(cellCoordinates);
        }

        public CellCoordinatesBlock() { }
    }
}


