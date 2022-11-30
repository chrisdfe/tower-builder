using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.TransportationItems
{
    public class TransportationItem
    {
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

        public int id { get; private set; }

        public string title { get; private set; } = "None";
        public Key key { get; private set; } = Key.None;
        public string category { get; private set; } = "None";

        public int pricePerCell { get; private set; } = 0;

        public CellCoordinatesList cellCoordinatesList;

        public CellCoordinates entranceCellCoordinates = CellCoordinates.zero;
        public CellCoordinates exitCellCoordinates = CellCoordinates.zero;

        public bool isInBlueprintMode = false;

        public bool isOneWay = false;

        public TransportationItemTemplate template;

        public TransportationItem(TransportationItemTemplate template)
        {
            this.id = UIDGenerator.Generate("TransportationItem");

            this.title = template.title;
            this.key = template.key;
            this.category = template.category;
            this.pricePerCell = template.pricePerCell;

            this.cellCoordinatesList = template.cellCoordinatesList.Clone();

            this.entranceCellCoordinates = template.entranceCellCoordinates;
            this.exitCellCoordinates = template.exitCellCoordinates;

            this.template = template;
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
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