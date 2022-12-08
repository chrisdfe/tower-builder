using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class CellNeighbors
    {
        public class Neighbor
        {
            public CellOrientation cellOrientation;
            public bool isOccupied;

            public Neighbor(CellOrientation cellOrientation, bool isOccupied)
            {
                this.cellOrientation = cellOrientation;
                this.isOccupied = isOccupied;
            }
        }

        public Dictionary<CellOrientation, bool> map { get; private set; } = new Dictionary<CellOrientation, bool>();

        public List<Neighbor> list
        {
            get
            {
                List<Neighbor> result = new List<Neighbor>();

                foreach (KeyValuePair<CellOrientation, bool> entry in map)
                {
                    result.Add(new Neighbor(entry.Key, entry.Value));
                }

                return result;
            }
        }

        public List<Neighbor> occupiedList => list.FindAll(neighbor => neighbor.isOccupied).ToList();
        public CellOrientation occupied => NeighborListToCellOrientation(occupiedList);

        public List<Neighbor> occupiedOrthogonalList => occupiedList.FindAll(neighbor => IsOrthogonal(neighbor.cellOrientation)).ToList();
        public CellOrientation occupiedOrthogonal => NeighborListToCellOrientation(occupiedOrthogonalList);

        public List<Neighbor> notOccupiedList => list.FindAll(neighbor => !neighbor.isOccupied).ToList();
        public CellOrientation notOccupied => NeighborListToCellOrientation(notOccupiedList);

        public List<Neighbor> notOccupiedOrthogonalList => notOccupiedList.FindAll(neighbor => IsOrthogonal(neighbor.cellOrientation)).ToList();
        public CellOrientation notOccupiedOrthogonal => NeighborListToCellOrientation(notOccupiedOrthogonalList);

        public CellNeighbors(Dictionary<CellOrientation, bool> map)
        {
            this.map = map;
        }

        public override string ToString()
        {
            string occupiedText = $"occupied: {occupied}";
            string notOccupiedText = $"not occupied: {notOccupied}";
            return $"{occupiedText}\n{notOccupiedText}";
        }

        List<Neighbor> FilterByOrthogonal(List<Neighbor> list) =>
            list.FindAll(neighbor => neighbor.cellOrientation == (CellOrientation.Above | CellOrientation.Right | CellOrientation.Below | CellOrientation.Left));

        CellOrientation NeighborListToCellOrientation(List<Neighbor> neighbors) =>
            neighbors.Aggregate(CellOrientation.None, (acc, neighbor) => (acc | neighbor.cellOrientation));

        bool IsOrthogonal(CellOrientation cellOrientation) =>
            cellOrientation == CellOrientation.Above ||
            cellOrientation == CellOrientation.Right ||
            cellOrientation == CellOrientation.Below ||
            cellOrientation == CellOrientation.Left;

        /*
            Static API
        */
        public static CellNeighbors FromCellCoordinatesList(CellCoordinates cellCoordinates, CellCoordinatesList cellCoordinatesList) =>
            new CellNeighbors(
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


