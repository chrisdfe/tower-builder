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

        public CellCoordinatesList() { }

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

        public int GetLowestX()
        {
            int lowestX = int.MaxValue;

            foreach (CellCoordinates cellCoordinates in items)
            {
                if (cellCoordinates.x < lowestX)
                {
                    lowestX = cellCoordinates.x;
                }
            }

            return lowestX;
        }

        public int GetHighestX()
        {
            int highestX = int.MinValue;

            foreach (CellCoordinates cellCoordinates in items)
            {
                if (cellCoordinates.x > highestX)
                {
                    highestX = cellCoordinates.x;
                }
            }

            return highestX;
        }


        public int GetLowestFloor()
        {
            int lowestFloor = int.MaxValue;

            foreach (CellCoordinates cellCoordinates in items)
            {
                if (cellCoordinates.floor < lowestFloor)
                {
                    lowestFloor = cellCoordinates.floor;
                }
            }

            return lowestFloor;
        }

        public int GetHighestFloor()
        {
            int highestFloor = int.MinValue;

            foreach (CellCoordinates cellCoordinates in items)
            {
                if (cellCoordinates.floor > highestFloor)
                {
                    highestFloor = cellCoordinates.floor;
                }
            }

            return highestFloor;
        }

        public List<int> GetXValues()
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

        public List<int> GetFloorValues()
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

        public CellCoordinatesList ToRelativeCoordinates()
        {
            List<CellCoordinates> list = new List<CellCoordinates>();
            CellCoordinates bottomLeftCoordinates = GetBottomLeftCoordinates();

            foreach (CellCoordinates cellCoordinates in items)
            {
                list.Add(Subtract(cellCoordinates, bottomLeftCoordinates));
            }

            return new CellCoordinatesList(list);
        }

        public CellCoordinates GetBottomLeftCoordinates()
        {
            return new CellCoordinates(
                GetLowestX(),
                GetLowestFloor()
            );
        }

        public CellCoordinates GetBottomRightCoordinates()
        {
            return new CellCoordinates(
                GetHighestX(),
                GetLowestFloor()
            );
        }

        public CellCoordinates GetTopLeftCoordinates()
        {
            return new CellCoordinates(
                GetLowestX(),
                GetHighestFloor()
            );
        }

        public CellCoordinates GetTopRightCoordinates()
        {
            return new CellCoordinates(
                GetHighestX(),
                GetHighestFloor()
            );
        }

        public int GetWidth()
        {
            int highestX = GetHighestX();
            int lowestX = GetLowestX();
            return (highestX - lowestX) + 1;
        }

        public int GetFloorSpan()
        {
            int highestFloor = GetHighestFloor();
            int lowestFloor = GetLowestFloor();
            return (highestFloor - lowestFloor) + 1;
        }

        public List<CellCoordinates> GetPerimeterCellCoordinates()
        {
            List<CellCoordinates> result = new List<CellCoordinates>();

            foreach (CellCoordinates cellCoordinates in items)
            {
                CellCoordinates[] adjacentCellCoordinatesList = new CellCoordinates[] {
                    cellCoordinates.GetCoordinatesAbove(),
                    cellCoordinates.GetCoordinatesRight(),
                    cellCoordinates.GetCoordinatesBelow(),
                    cellCoordinates.GetCoordinatesLeft()
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


