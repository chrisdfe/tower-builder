using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.State
{
    public class SelectionBox
    {
        public CellCoordinates bottomLeft { get; private set; }
        public CellCoordinates topRight { get; private set; }

        public CellCoordinates topLeft { get { return new CellCoordinates(topRight.floor, bottomLeft.x); } }
        public CellCoordinates bottomRight { get { return new CellCoordinates(bottomLeft.floor, topRight.x); } }

        public Dimensions dimensions
        {
            get
            {
                return new Dimensions(
                    (topRight.x - bottomLeft.x) + 1,
                    (topRight.floor - bottomLeft.floor) + 1
                );
            }
        }

        public SelectionBox(CellCoordinates start, CellCoordinates end)
        {
            CalculateBetween(start, end);
        }

        public void CalculateBetween(CellCoordinates start, CellCoordinates end)
        {
            bottomLeft = new CellCoordinates(
                Math.Min(start.x, end.x),
                Math.Min(start.floor, end.floor)
            );

            topRight = new CellCoordinates(
                Math.Max(start.x, end.x),
                Math.Max(start.floor, end.floor)
            );
        }
    }
}


