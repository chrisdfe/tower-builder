using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.ApplicationState;
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

        protected override void BuildWindows(AppState appState)
        {
            base.BuildWindows(appState);

            Window officeWindow = new Window(Entities.Definitions.Windows.defaultDefinition as WindowDefinition);
            officeWindow.CalculateCellsFromSelectionBox(appState.UI.selectionBox.asRelativeSelectionBox);

            currentRoom.Add(
                officeWindow
            );
        }

        protected override void BuildFoundation(AppState appState)
        {
            base.BuildFoundation(appState);

            Foundation officeFoundation = new Foundation(
                new FoundationDefinition()
                {
                    blockCellsTemplate = CellCoordinatesList.CreateRectangle(7, Entities.Constants.FLOOR_HEIGHT),
                    resizability = Resizability.Inflexible
                }
            );

            officeFoundation.CalculateCellsFromSelectionBox(appState.UI.selectionBox.asRelativeSelectionBox);

            currentRoom.Add(
                officeFoundation
            );
        }
    }
}
