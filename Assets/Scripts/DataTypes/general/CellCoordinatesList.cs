using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TowerBuilder.Systems;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class CellCoordinatesList : SaveableListWrapper<CellCoordinates, CellCoordinates.Input>
    {
        public int lowestX =>
            items.Aggregate(int.MaxValue, (lowestX, coordinates) =>
                (coordinates.x < lowestX) ? coordinates.x : lowestX
            );

        public int highestX =>
            items.Aggregate(int.MinValue, (highestX, coordinates) =>
                (coordinates.x > highestX) ? coordinates.x : highestX
            );

        public int lowestY =>
            items.Aggregate(int.MaxValue, (lowestY, coordinates) =>
                (coordinates.y < lowestY) ? coordinates.y : lowestY
            );


        public int highestY =>
            items.Aggregate(int.MinValue, (highestY, coordinates) =>
                (coordinates.y > highestY) ? coordinates.y : highestY
            );

        public int width => (highestX - lowestX) + 1;

        public int height => (highestY - lowestY) + 1;

        public CellCoordinates bottomLeftCoordinates => new CellCoordinates(lowestX, lowestY);

        public CellCoordinates bottomRightCoordinates => new CellCoordinates(highestX, lowestY);

        public CellCoordinates topLeftCoordinates => new CellCoordinates(lowestX, highestY);

        public CellCoordinates topRightCoordinates => new CellCoordinates(highestX, highestY);

        public List<int> xValues =>
            items.Aggregate(new List<int>(), (acc, cellCoordinates) =>
            {
                if (!acc.Contains(cellCoordinates.x))
                {
                    acc.Add(cellCoordinates.x);
                }

                return acc;
            });

        public List<int> yValues =>
            items.Aggregate(new List<int>(), (acc, cellCoordinates) =>
            {
                if (!acc.Contains(cellCoordinates.y))
                {
                    acc.Add(cellCoordinates.y);
                }

                return acc;
            });

        public CellCoordinatesList asRelativeCoordinates =>
            Subtract(this, bottomLeftCoordinates);

        public CellCoordinatesList bottomRow =>
            new CellCoordinatesList(
                items.FindAll(cellCoordinates => cellCoordinates.y == lowestY)
            );

        public CellCoordinatesList topRow =>
            new CellCoordinatesList(
                items.FindAll(cellCoordinates => cellCoordinates.y == highestY)
            );

        public CellCoordinatesList() { }
        public CellCoordinatesList(CellCoordinates cellCoordinates) : base(cellCoordinates) { }
        public CellCoordinatesList(List<CellCoordinates> cellCoordinatesList) : base(cellCoordinatesList) { }
        public CellCoordinatesList(CellCoordinatesList cellCoordinatesList) : base(cellCoordinatesList) { }

        // public override void ConsumeInput(SaveableInputBase input)
        // {
        //     items = input.items
        // }

        public override bool Contains(CellCoordinates cellCoordinates) =>
            items.Find(otherCellCoordinates => otherCellCoordinates.Matches(cellCoordinates)) != null;

        public void PositionAtCoordinates(CellCoordinates newBaseCoordinates)
        {
            List<CellCoordinates> result = new List<CellCoordinates>();

            foreach (CellCoordinates cellCoordinates in items)
            {
                result.Add(CellCoordinates.Add(newBaseCoordinates, cellCoordinates));
            }

            items = result;
        }

        public bool OverlapsWith(CellCoordinatesList otherCellCoordinatesList) =>
            GetOverlapBetween(otherCellCoordinatesList).Count != 0;

        public CellCoordinatesList GetOverlapBetween(CellCoordinatesList otherCellCoordinatesList) =>
            new CellCoordinatesList(
                otherCellCoordinatesList.items.FindAll((cellCoordinates) => Contains(cellCoordinates)).ToList()
            );

        public CellCoordinatesList GetPerimeterCellCoordinates()
        {
            CellCoordinatesList result = new CellCoordinatesList();

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
                    if (!result.Contains(adjacentCellCoordinates) && !Contains(adjacentCellCoordinates))
                    {
                        result.Add(adjacentCellCoordinates);
                    }
                }
            }

            return result;
        }

        public CellCoordinatesList Clone() =>
            new CellCoordinatesList(
                items.Select(cellCoordinates => cellCoordinates.Clone()).ToList()
            );

        public void ForEach(Predicate<CellCoordinates> predicate) => items.ForEach(cellCoordinates => predicate(cellCoordinates));

        /*
            Static Interface
        */
        public static CellCoordinatesList one => new CellCoordinatesList(CellCoordinates.zero);

        public static CellCoordinatesList Add(CellCoordinatesList list, CellCoordinates offset) =>
            new CellCoordinatesList(
                list.items
                    .Select(cellCoordinates => CellCoordinates.Add(cellCoordinates, offset))
                    .ToList()
            );

        public static CellCoordinatesList Subtract(CellCoordinatesList list, CellCoordinates offset) =>
            new CellCoordinatesList(
                list.items
                    .Select(cellCoordinates => CellCoordinates.Subtract(cellCoordinates, offset))
                    .ToList()
            );

        public static CellCoordinatesList CreateRectangle(CellCoordinates a, CellCoordinates b)
        {
            List<CellCoordinates> list = new List<CellCoordinates>();

            // startCoordinates = top left room
            // endCoordinates = bottom right room
            CellCoordinates startCoordinates = new CellCoordinates(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
            CellCoordinates endCoordinates = new CellCoordinates(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));

            for (int y = startCoordinates.y; y <= endCoordinates.y; y++)
            {
                for (int x = startCoordinates.x; x <= endCoordinates.x; x++)
                {
                    list.Add(new CellCoordinates(x, y));
                }
            }

            return new CellCoordinatesList(list);
        }

        public static CellCoordinatesList CreateRectangle(int width, int height) =>
            CreateRectangle(
                CellCoordinates.zero,
                new CellCoordinates(width - 1, height - 1)
            );

        public static CellCoordinatesList CreateRectangle(int size) => CreateRectangle(size, size);

        public static CellCoordinatesList FromBlocksList(CellCoordinatesBlockList blockList)
        {
            CellCoordinatesList result = new CellCoordinatesList();

            foreach (var block in blockList.items)
            {
                result.Add(block.items);
            }

            return result;
        }
    }
}


