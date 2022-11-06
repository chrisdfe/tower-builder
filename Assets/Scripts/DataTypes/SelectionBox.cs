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
            this.start = start;
            this.end = end;
            CalculateBox();
        }

        public SelectionBox(CellCoordinates coordinates) : this(coordinates, coordinates) { }

        public SelectionBox() : this(CellCoordinates.zero, CellCoordinates.zero) { }

        public List<CellCoordinates> GetCells()
        {
            List<CellCoordinates> result = new List<CellCoordinates>();

            for (int x = topLeft.x; x <= topRight.x; x++)
            {
                for (int floor = topLeft.floor; floor >= bottomLeft.floor; floor--)
                {
                    result.Add(new CellCoordinates(x, floor));
                }
            }

            return result;
        }

        public void SetStart(CellCoordinates start)
        {
            this.start = start;
            CalculateBox();
        }

        public void SetEnd(CellCoordinates end)
        {
            this.end = end;
            CalculateBox();
        }

        public void SetStartAndEnd(CellCoordinates coordinates)
        {
            this.start = coordinates;
            this.end = coordinates;
            CalculateBox();
        }

        void CalculateBox()
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


