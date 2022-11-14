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

        /*         public CellCoordinates bottomLeft { get; private set; }
                public CellCoordinates topRight { get; private set; }

                public CellCoordinates topLeft { get { return new CellCoordinates(topRight.floor, bottomLeft.x); } }
                public CellCoordinates bottomRight { get { return new CellCoordinates(bottomLeft.floor, topRight.x); } } */

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
                    (cellCoordinatesList.GetTopRightCoordinates().x - cellCoordinatesList.GetBottomLeftCoordinates().x) + 1,
                    (cellCoordinatesList.GetTopRightCoordinates().floor - cellCoordinatesList.GetBottomLeftCoordinates().floor) + 1
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


