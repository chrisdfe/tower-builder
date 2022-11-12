using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.State.Tools
{
    public class InspectToolState : ToolStateBase
    {
        public struct Input
        {
            public Room currentInspectedRoom;
        }

        public class Events
        {
            public delegate void CurrentSelectedEntityEvent(EntityBase entity);
            public CurrentSelectedEntityEvent onCurrentSelectedEntityUpdated;
        }

        public Events events;

        public int inspectedEntityIndex { get; private set; } = -1;

        public EntityBase inspectedEntity
        {
            get
            {
                if (inspectedEntityIndex == -1)
                {
                    return null;
                }

                return appState.UI.currentSelectedCellEntityList.entities[inspectedEntityIndex];
            }
        }

        public InspectToolState(AppState appState, State state, Input input) : base(appState, state)
        {
            events = new Events();
        }

        public override void OnCurrentSelectedRoomBlockUpdated(RoomCells roomBlock)
        {
            inspectedEntityIndex = -1;
        }

        public override void OnSelectionStart(SelectionBox selectionBox)
        {
            EntityList entityList = Registry.appState.UI.currentSelectedCellEntityList;
            Debug.Log("selectable entities: " + entityList.Count);

            if (entityList.Count > 0)
            {
                // Start at the top and work down as the user clicks
                if (inspectedEntityIndex > 0)
                {
                    // move down the list
                    SetInspectedEntityIndex(inspectedEntityIndex - 1);
                }
                else
                {
                    // back up to the top
                    SetInspectedEntityIndex(entityList.Count - 1);
                }
            }
            else
            {
                if (inspectedEntityIndex != -1)
                {
                    ResetInspectedEntityIndex();
                }
            }
        }

        void ResetInspectedEntityIndex()
        {
            SetInspectedEntityIndex(-1);
        }

        void SetInspectedEntityIndex(int newIndex)
        {
            inspectedEntityIndex = newIndex;

            if (events.onCurrentSelectedEntityUpdated != null)
            {
                events.onCurrentSelectedEntityUpdated(inspectedEntity);
            }
        }
    }
}