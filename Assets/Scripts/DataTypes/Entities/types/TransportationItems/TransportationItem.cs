using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.TransportationItems
{
    public class TransportationItem : Entity
    {
        public override string idKey => "TransportationItem";

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
    }
}