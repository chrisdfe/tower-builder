using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class SelectionBox
    {
        public CellCoordinates start { get; private set; }
        public CellCoordinates end { get; private set; }

        public CellCoordinatesList cellCoordinatesList
        {
            get
            {
                return CellCoordinatesList.CreateRectangle(start, end);
            }
        }

        public Dimensions dimensions
        {
            get
            {
                return new Dimensions(
                    (cellCoordinatesList.topRightCoordinates.x - cellCoordinatesList.bottomLeftCoordinates.x) + 1,
                    (cellCoordinatesList.topRightCoordinates.y - cellCoordinatesList.bottomLeftCoordinates.y) + 1
                );
            }
        }

        public SelectionBox(CellCoordinates start, CellCoordinates end)
        {
            this.start = start;
            this.end = end;
        }

        public SelectionBox(CellCoordinates coordinates) : this(coordinates, coordinates) { }

        public SelectionBox() : this(CellCoordinates.zero, CellCoordinates.zero) { }

        public void SetStart(CellCoordinates start)
        {
            this.start = start;
        }

        public void SetEnd(CellCoordinates end)
        {
            this.end = end;
        }

        public void SetStartAndEnd(CellCoordinates coordinates)
        {
            this.start = coordinates;
            this.end = coordinates;
        }
    }
}


