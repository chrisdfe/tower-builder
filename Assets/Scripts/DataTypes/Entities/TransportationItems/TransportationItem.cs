using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.TransportationItems
{
    public class TransportationItem : Entity<TransportationItem.Key>
    {
        public override string idKey => "TransportationItem";

        public enum Key
        {
            None,
            Ladder,
            Escalator,
            Doorway,
        }

        public static EnumStringMap<Key> KeyLabelMap = new EnumStringMap<Key>(
            new Dictionary<Key, string>() {
                { Key.None, "None" },
                { Key.Ladder, "Ladder" },
                { Key.Escalator, "Escalator" },
                { Key.Doorway, "Doorway" },
            }
        );

        public CellCoordinatesList entranceCellCoordinatesList = new CellCoordinatesList();
        public CellCoordinatesList exitCellCoordinatesList = new CellCoordinatesList();
        List<(CellCoordinates, CellCoordinates)> entranceExitTuples;

        // TODO - replace this with Transportation direction
        public bool isOneWay = false;

        public TransportationItem(TransportationItemDefinition definition) : base(definition)
        {
            entranceExitTuples = (definition as TransportationItemDefinition).entranceExitBuilder(this);
        }


        /*
            Public Interface
        */
        public override void OnBuild()
        {
            base.OnBuild();

            TransportationItemDefinition transportationItemDefinition = this.definition as TransportationItemDefinition;
            entranceCellCoordinatesList = new CellCoordinatesList();
            exitCellCoordinatesList = new CellCoordinatesList();

            entranceExitTuples = (definition as TransportationItemDefinition).entranceExitBuilder(this);

            foreach ((CellCoordinates, CellCoordinates) entranceExitTuple in entranceExitTuples)
            {
                var (entranceCellCoordinates, exitCellCoordinates) = entranceExitTuple;
                entranceCellCoordinatesList.Add(entranceCellCoordinates);
                exitCellCoordinatesList.Add(exitCellCoordinates);
            }
        }

        public override void PositionAtCoordinates(CellCoordinates cellCoordinates)
        {
            base.PositionAtCoordinates(cellCoordinates);
        }

        // public CellCoordinates GetEntranceOrExit(CellCoordinates cellCoordinates)
        // {
        //     if (cellCoordinates.Matches(entranceCellCoordinates))
        //     {
        //         return exitCellCoordinates;
        //     }

        //     if (cellCoordinates.Matches(exitCellCoordinates))
        //     {
        //         return entranceCellCoordinates;
        //     }

        //     return null;
        // }
    }
}