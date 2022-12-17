using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    [System.Serializable]
    public class CellCoordinatesList : ListWrapper<CellCoordinates>
    {
        public int lowestX =>
            items.Aggregate(int.MaxValue, (lowestX, coordinates) =>
                (coordinates.x < lowestX) ? coordinates.x : lowestX
            );

        public int highestX =>
            items.Aggregate(int.MinValue, (highestX, coordinates) =>
                (coordinates.x > highestX) ? coordinates.x : highestX
            );

        public int lowestFloor =>
            items.Aggregate(int.MaxValue, (lowestFloor, coordinates) =>
                (coordinates.floor < lowestFloor) ? coordinates.floor : lowestFloor
            );


        public int highestFloor =>
            items.Aggregate(int.MinValue, (highestFloor, coordinates) =>
                (coordinates.floor > highestFloor) ? coordinates.floor : highestFloor
            );

        public int width => (highestX - lowestX) + 1;

        public int floorSpan => (highestFloor - lowestFloor) + 1;

        public CellCoordinates bottomLeftCoordinates => new CellCoordinates(lowestX, lowestFloor);

        public CellCoordinates bottomRightCoordinates => new CellCoordinates(highestX, lowestFloor);

        public CellCoordinates topLeftCoordinates => new CellCoordinates(lowestX, highestFloor);

        public CellCoordinates topRightCoordinates => new CellCoordinates(highestX, highestFloor);

        public List<int> xValues =>
            items.Aggregate(new List<int>(), (acc, cellCoordinates) =>
            {
                if (!acc.Contains(cellCoordinates.x))
                {
                    acc.Add(cellCoordinates.x);
                }

                return acc;
            });

        public List<int> floorValues =>
            items.Aggregate(new List<int>(), (acc, cellCoordinates) =>
            {
                if (!acc.Contains(cellCoordinates.floor))
                {
                    acc.Add(cellCoordinates.floor);
                }

                return acc;
            });

        public CellCoordinatesList asRelativeCoordinates =>
            new CellCoordinatesList(
                items.Select(cellCoordinates => AsRelativeCoordinates(cellCoordinates)).ToList()
            );

        public CellCoordinatesList bottomRow =>
            new CellCoordinatesList(
                items.FindAll(cellCoordinates => cellCoordinates.floor == lowestFloor)
            );

        public CellCoordinatesList() { }

        public CellCoordinatesList(CellCoordinates cellCoordinates) : base(cellCoordinates) { }
        public CellCoordinatesList(List<CellCoordinates> cellCoordinatesList) : base(cellCoordinatesList) { }
        public CellCoordinatesList(CellCoordinatesList cellCoordinatesList) : base(cellCoordinatesList) { }

        public override bool Contains(CellCoordinates cellCoordinates) =>
            items.Find(otherCellCoordinates => otherCellCoordinates.Matches(cellCoordinates)) != null;

        public void PositionAtCoordinates(CellCoordinates newBaseCoordinates)
        {
            List<CellCoordinates> result = new List<CellCoordinates>();

            foreach (CellCoordinates cellCoordinates in items)
            {
                result.Add(Add(newBaseCoordinates, cellCoordinates));
            }

            items = result;
        }


        public bool OverlapsWith(CellCoordinatesList otherCellCoordinatesList) =>
            GetOverlapBetween(otherCellCoordinatesList).Count != 0;

        public CellCoordinatesList GetOverlapBetween(CellCoordinatesList otherCellCoordinatesList) =>
            new CellCoordinatesList(
                otherCellCoordinatesList.items.FindAll((cellCoordinates) => Contains(cellCoordinates)).ToList()
            );

        public List<CellCoordinates> GetPerimeterCellCoordinates()
        {
            List<CellCoordinates> result = new List<CellCoordinates>();

            foreach (CellCoordinates cellCoordinates in items)
            {
                CellCoordinates[] adjacentCellCoordinatesList = new CellCoordinates[] {
                    cellCoordinates.coordinatesAbove,
                    cellCoordinates.coordinatesRight,
                    cellCoordinates.coordinatesBelow,
                    cellCoordinates.coordinatesLeft
                };

                foreach (CellCoordinates adjacentCellCoordinates in adjacentCellCoordinatesList)
                {
                    if (!result.Contains(adjacentCellCoordinates))
                    {
                        result.Add(adjacentCellCoordinates);
                    }
                }
            }

            return result;
        }

        public CellCoordinates AsRelativeCoordinates(CellCoordinates cellCoordinates) =>
            Subtract(cellCoordinates, bottomLeftCoordinates);

        public CellCoordinatesList Clone() =>
            new CellCoordinatesList(
                items.Select(cellCoordinates => cellCoordinates.Clone()).ToList()
            );

        public void ForEach(Predicate<CellCoordinates> predicate) => items.ForEach(cellCoordinates => predicate(cellCoordinates));

        /*
            Static API
        */
        public static CellCoordinatesList one => new CellCoordinatesList(CellCoordinates.zero);

        // TODO - this should be in cellcoordates
        public static CellCoordinates Add(CellCoordinates a, CellCoordinates b) =>
            new CellCoordinates(a.x + b.x, a.floor + b.floor);

        // TODO - this should be in cellcoordates
        public static CellCoordinates Subtract(CellCoordinates a, CellCoordinates b) =>
            new CellCoordinates(a.x - b.x, a.floor - b.floor);

        public static CellCoordinatesList CreateRectangle(int xWidth, int floors)
        {
            List<CellCoordinates> list = new List<CellCoordinates>();

            for (int x = 0; x < xWidth; x++)
            {
                for (int floor = 0; floor < floors; floor++)
                {
                    list.Add(new CellCoordinates(x, floor));
                }
            }

            return new CellCoordinatesList(list);
        }

        public static CellCoordinatesList CreateRectangle(CellCoordinates a, CellCoordinates b)
        {
            List<CellCoordinates> list = new List<CellCoordinates>();

            // startCoordinates = top left room
            // endCoordinates = bottom right room
            CellCoordinates startCoordinates = new CellCoordinates(Mathf.Min(a.x, b.x), Mathf.Min(a.floor, b.floor));
            CellCoordinates endCoordinates = new CellCoordinates(Mathf.Max(a.x, b.x), Mathf.Max(a.floor, b.floor));

            for (int floor = startCoordinates.floor; floor <= endCoordinates.floor; floor++)
            {
                for (int x = startCoordinates.x; x <= endCoordinates.x; x++)
                {
                    list.Add(new CellCoordinates(x, floor));
                }
            }

            return new CellCoordinatesList(list);
        }
    }
}


