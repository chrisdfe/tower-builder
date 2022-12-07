using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
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

        public static List<(Key, string)> KeyLabelMap = new List<(Key, string)>() {
            (Key.None, "None"),
            (Key.Ladder, "Ladder"),
            (Key.Escalator, "Escalator"),
            (Key.Doorway, "Doorway"),
        };

        public override int pricePerCell => 1000;

        public CellCoordinates entranceCellCoordinates = CellCoordinates.zero;
        public CellCoordinates exitCellCoordinates = CellCoordinates.zero;

        // TODO - replace this with Transportation direction
        public bool isOneWay = false;

        public new TransportationItemTemplate template;

        public TransportationItem(TransportationItemTemplate template) : base(template)
        {
            this.entranceCellCoordinates = template.entranceCellCoordinates;
            this.exitCellCoordinates = template.exitCellCoordinates;
        }

        /*
            Public Interface
        */
        public void PositionAtCoordinates(CellCoordinates cellCoordinates)
        {
            cellCoordinatesList.PositionAtCoordinates(cellCoordinates);
            entranceCellCoordinates = CellCoordinates.Add(cellCoordinates, template.entranceCellCoordinates);
            exitCellCoordinates = CellCoordinates.Add(cellCoordinates, template.exitCellCoordinates);
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

        /* 
            Static Interface
        */
        public static string GetLabelByKey(Key targetKey)
        {
            foreach (var (key, label) in KeyLabelMap)
            {
                if (key == targetKey)
                {
                    return label;
                }
            }

            return null;
        }

        public static Key GetKeyByLabel(string targetLabel)
        {
            foreach (var (key, label) in KeyLabelMap)
            {
                if (label == targetLabel)
                {
                    return key;
                }
            }

            return Key.None;
        }
    }
}