using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes.Entities.Foundations;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Windows;
using UnityEngine;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms
{
    public class LobbyRoomBuilder : RoomBuilderBase
    {
        public LobbyRoomBuilder(EntityGroupDefinition definition) : base(definition) { }

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
                    blockCellsTemplate = CellCoordinatesList.CreateRectangle(1, 3 * 3)
                }
            );

            officeFoundation.CalculateCellsFromSelectionBox(selectionBox);

            currentRoom.Add(
                officeFoundation
            );
        }
    }
}
