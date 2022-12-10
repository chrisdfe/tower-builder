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

        public CellCoordinates entranceCellCoordinates = CellCoordinates.zero;
        public CellCoordinates exitCellCoordinates = CellCoordinates.zero;

        // TODO - replace this with Transportation direction
        public bool isOneWay = false;

        public TransportationItem(TransportationItemDefinition definition) : base(definition)
        {
            this.entranceCellCoordinates = definition.entranceCellCoordinates;
            this.exitCellCoordinates = definition.exitCellCoordinates;
        }

        /*
            Public Interface
        */
        public override void PositionAtCoordinates(CellCoordinates cellCoordinates)
        {
            base.PositionAtCoordinates(cellCoordinates);
            TransportationItemDefinition transportationItemDefinition = this.definition as TransportationItemDefinition;
            entranceCellCoordinates = CellCoordinates.Add(cellCoordinates, transportationItemDefinition.entranceCellCoordinates);
            exitCellCoordinates = CellCoordinates.Add(cellCoordinates, transportationItemDefinition.exitCellCoordinates);
        }

        public CellCoordinates GetEntranceOrExit(CellCoordinates cellCoordinates)
        {
            if (cellCoordinates.Matches(entranceCellCoordinates))
            {
                return exitCellCoordinates;
            }

            if (cellCoordinates.Matches(exitCellCoordinates))
            {
                return entranceCellCoordinates;
            }

            return null;
        }
    }
}