using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class OccupiedCellMap
    {
        public Dictionary<CellOrientation, bool> map { get; private set; } = new Dictionary<CellOrientation, bool>();

        public List<(CellOrientation, bool)> asTupleList
        {
            get
            {
                List<(CellOrientation, bool)> result = new List<(CellOrientation, bool)>();

                foreach (KeyValuePair<CellOrientation, bool> entry in map)
                {
                    result.Add((entry.Key, entry.Value));
                }

                return result;
            }
        }

        public List<CellOrientation> occupied => asTupleList.Aggregate(new List<CellOrientation>(), (acc, tuple) =>
        {
            (CellOrientation cellOrientation, bool isOccupied) = tuple;

            if (isOccupied)
            {
                acc.Add(cellOrientation);
            }

            return acc;
        });

        public List<CellOrientation> notOccupied => asTupleList.Aggregate(new List<CellOrientation>(), (acc, tuple) =>
        {
            (CellOrientation cellOrientation, bool isOccupied) = tuple;

            if (!isOccupied)
            {
                acc.Add(cellOrientation);
            }

            return acc;
        });

        public OccupiedCellMap(Dictionary<CellOrientation, bool> map)
        {
            this.map = map;
        }

        public override string ToString()
        {
            string occupiedText = $"occupied: {String.Join(", ", occupied.ToArray())}";
            string notOccupiedText = $"not occupied: {String.Join(", ", notOccupied.ToArray())}";
            return $"{occupiedText}\n{notOccupiedText}";
        }

        public bool Has(CellOrientation cellOrientation) => map.ContainsKey(cellOrientation) && map[cellOrientation];

        public bool DoesNotHave(CellOrientation cellOrientation) => (
            (!map.ContainsKey(cellOrientation)) ||
            (map.ContainsKey(cellOrientation) && map[cellOrientation])
        );

        public bool HasAll(CellOrientation[] cellOrientations) =>
            GetOccupiedCells(cellOrientations).Count == cellOrientations.Count();

        public bool HasSome(CellOrientation[] cellOrientations) =>
            GetOccupiedCells(cellOrientations).Count > 0;

        public bool HasNone(CellOrientation[] cellOrientations) =>
            GetOccupiedCells(cellOrientations).Count == 0;

        public bool HasOnly(CellOrientation cellOrientation) => (
            occupied.Count == 1 &&
            occupied[0] == cellOrientation
        );

        // TODO - I'm not sure if this works
        public bool HasOnly(CellOrientation[] cellOrientations) => (
            cellOrientations.Count() == occupied.Count &&
            new HashSet<CellOrientation>(occupied).SetEquals(cellOrientations)
        );

        List<CellOrientation> GetOccupiedCells(CellOrientation[] cellOrientations) =>
            cellOrientations.ToList().FindAll(orientation => Has(orientation));

        /*
            Static API
        */
        public static OccupiedCellMap FromCellCoordinatesList(CellCoordinates cellCoordinates, CellCoordinatesList cellCoordinatesList) =>
            new OccupiedCellMap(
                new Dictionary<CellOrientation, bool>() {
                    { CellOrientation.Above,      cellCoordinatesList.Contains(cellCoordinates.coordinatesAbove) },
                    { CellOrientation.AboveRight, cellCoordinatesList.Contains(cellCoordinates.coordinatesAboveRight) },
                    { CellOrientation.Right,      cellCoordinatesList.Contains(cellCoordinates.coordinatesRight) },
                    { CellOrientation.BelowRight, cellCoordinatesList.Contains(cellCoordinates.coordinatesBelowRight) },
                    { CellOrientation.Below,      cellCoordinatesList.Contains(cellCoordinates.coordinatesBelow) },
                    { CellOrientation.BelowLeft,  cellCoordinatesList.Contains(cellCoordinates.coordinatesBelowLeft) },
                    { CellOrientation.Left,       cellCoordinatesList.Contains(cellCoordinates.coordinatesLeft) },
                    { CellOrientation.AboveLeft,  cellCoordinatesList.Contains(cellCoordinates.coordinatesAboveLeft) },
                }
            );
    }
}


