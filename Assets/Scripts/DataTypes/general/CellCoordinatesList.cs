using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    [System.Serializable]
    public class CellCoordinatesList
    {
        public List<CellCoordinates> items { get; private set; } = new List<CellCoordinates>();

        public int Count { get { return items.Count; } }

        public int lowestX
        {
            get
            {
                return items.Aggregate(int.MaxValue, (lowestX, coordinates) =>
                    (coordinates.x < lowestX) ? coordinates.x : lowestX
                );
            }
        }

        public int highestX
        {
            get
            {
                return items.Aggregate(int.MinValue, (highestX, coordinates) =>
                    (coordinates.x > highestX) ? coordinates.x : lowestX
                );
            }
        }

        public int lowestFloor
        {
            get
            {
                return items.Aggregate(int.MaxValue, (lowestFloor, coordinates) =>
                    (coordinates.floor < lowestFloor) ? coordinates.floor : lowestFloor
                );
            }
        }


        public int highestFloor
        {
            get
            {
                return items.Aggregate(int.MinValue, (highestFloor, coordinates) =>
                    (coordinates.floor > highestFloor) ? coordinates.floor : highestFloor
                );
            }
        }

        public int width
        {
            get
            {
                return (highestX - lowestX) + 1;
            }
        }

        public int floorSpan
        {
            get
            {
                return (highestFloor - lowestFloor) + 1;
            }
        }

        public CellCoordinates bottomLeftCoordinates
        {
            get
            {
                return new CellCoordinates(
                    lowestX,
                    lowestFloor
                );
            }
        }

        public CellCoordinates bottomRightCoordinates
        {
            get
            {
                return new CellCoordinates(
                    highestX,
                    lowestFloor
                );
            }
        }

        public CellCoordinates topLeftCoordinates
        {
            get
            {
                return new CellCoordinates(
                    lowestX,
                    highestFloor
                );
            }
        }

        public CellCoordinates topRightCoordinates
        {
            get
            {
                return new CellCoordinates(
                    highestX,
                    highestFloor
                );
            }
        }

        public List<int> xValues
        {
            get
            {
                List<int> result = new List<int>();

                foreach (CellCoordinates cellCoordinates in items)
                {
                    if (!result.Contains(cellCoordinates.x))
                    {
                        result.Add(cellCoordinates.x);
                    }
                }

                return result;
            }
        }

        public List<int> floorValues
        {
            get
            {
                List<int> result = new List<int>();

                foreach (CellCoordinates cellCoordinates in items)
                {
                    if (!result.Contains(cellCoordinates.x))
                    {
                        result.Add(cellCoordinates.x);
                    }
                }

                return result;
            }
        }

        public CellCoordinatesList asRelativeCoordinates
        {
            get =>
                new CellCoordinatesList(
                    items.Select(cellCoordinates => Subtract(cellCoordinates, bottomLeftCoordinates)).ToList()
                );
        }


        public CellCoordinatesList() { }

        public CellCoordinatesList(CellCoordinates cellCoordinates)
        {
            this.items = new List<CellCoordinates>() { cellCoordinates };
        }

        public CellCoordinatesList(List<CellCoordinates> items)
        {
            this.items = items;
        }

        public void Set(List<CellCoordinates> cellCoordinatesList)
        {
            this.items = cellCoordinatesList;
        }

        public void Add(List<CellCoordinates> cellCoordinatesList)
        {
            items = items.Concat(cellCoordinatesList).ToList();
        }

        public void Add(CellCoordinates cellCoordinates)
        {
            items.Add(cellCoordinates);
        }

        public void Remove(CellCoordinatesList cellCoordinatesListToDelete)
        {
            items.RemoveAll(cellCoordinates => cellCoordinatesListToDelete.Contains(cellCoordinates));
        }

        public void PositionAtCoordinates(CellCoordinates newBaseCoordinates)
        {
            List<CellCoordinates> result = new List<CellCoordinates>();

            foreach (CellCoordinates cellCoordinates in items)
            {
                result.Add(Add(newBaseCoordinates, cellCoordinates));
            }

            items = result;
        }

        public bool Contains(CellCoordinates cellCoordinates)
        {
            return items.Find(otherCellCoordinates => otherCellCoordinates.Matches(cellCoordinates)) != null;
        }

        public bool OverlapsWith(CellCoordinatesList otherCellCoordinatesList)
        {
            return GetOverlapBetween(otherCellCoordinatesList).Count != 0;
        }

        public CellCoordinatesList GetOverlapBetween(CellCoordinatesList otherCellCoordinatesList)
        {
            List<CellCoordinates> result = new List<CellCoordinates>();

            foreach (CellCoordinates cellCoordinates in otherCellCoordinatesList.items)
            {
                if (Contains(cellCoordinates))
                {
                    result.Add(cellCoordinates);
                }
            }

            return new CellCoordinatesList(result);
        }

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

        public CellCoordinatesList Clone()
        {
            return new CellCoordinatesList(
                items.Select(cellCoordinates => cellCoordinates.Clone()).ToList()
            );
        }

        /*
            Static API
        */
        public static CellCoordinates Add(CellCoordinates a, CellCoordinates b)
        {
            return new CellCoordinates(a.x + b.x, a.floor + b.floor);
        }

        public static CellCoordinates Subtract(CellCoordinates a, CellCoordinates b)
        {
            return new CellCoordinates(a.x - b.x, a.floor - b.floor);
        }

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


