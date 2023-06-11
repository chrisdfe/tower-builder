using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Foundations;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Windows;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class OfficeRoomBuilder : RoomBuilderBase
    {
        public OfficeRoomBuilder(EntityGroupDefinition definition) : base(definition) { }

        protected override void BuildWindows(SelectionBox selectionBox)
        {
            base.BuildWindows(selectionBox);

            Window officeWindow = new Window(Entities.Definitions.Windows.defaultDefinition as WindowDefinition);
            officeWindow.CalculateCellsFromSelectionBox(selectionBox);

            currentRoom.Add(
                officeWindow
            );
        }

        protected override void BuildFoundation(SelectionBox selectionBox)
        {
            base.BuildFoundation(selectionBox);

            Foundation officeFoundation = new Foundation(
                new FoundationDefinition()
                {
                    blockCellsTemplate = CellCoordinatesList.CreateRectangle(7, Entities.Constants.FLOOR_HEIGHT),
                    resizability = Resizability.Inflexible
                }
            );

            officeFoundation.CalculateCellsFromSelectionBox(selectionBox);

            currentRoom.Add(
                officeFoundation
            );
        }
    }
}
