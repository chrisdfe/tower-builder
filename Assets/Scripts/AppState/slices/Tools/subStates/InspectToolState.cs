using System.Collections.Generic;
using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.ApplicationState.UI;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public class InspectToolState : ToolStateBase
    {
        public struct Input
        {
            public Room currentInspectedRoom;
        }

        public class Events
        {
            public delegate void InspectedEntityListEvent(EntityList entityList);
            public InspectedEntityListEvent onInspectedEntityListUpdated;

            public delegate void CurrentSelectedEntityEvent(EntityBase entity);
            public CurrentSelectedEntityEvent onCurrentSelectedEntityUpdated;
        }

        public Events events;

        public EntityList inspectedEntityList { get; private set; } = new EntityList();
        public int inspectedEntityIndex { get; private set; } = -1;

        public EntityBase inspectedEntity
        {
            get
            {
                if (inspectedEntityIndex == -1)
                {
                    return null;
                }

                return inspectedEntityList.entities[inspectedEntityIndex];
            }
        }

        public InspectToolState(AppState appState, State state, Input input) : base(appState, state)
        {
            events = new Events();
        }

        public override void Setup()
        {
            base.Setup();

            appState.Furnitures.events.onItemsRemoved += OnFurnituresRemoved;
            appState.Residents.events.onResidentsRemoved += OnResidentsRemoved;

            appState.UI.events.onSecondaryActionPerformed += OnSecondaryActionPerformed;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Furnitures.events.onItemsRemoved -= OnFurnituresRemoved;
            appState.Residents.events.onResidentsRemoved -= OnResidentsRemoved;

            appState.UI.events.onSecondaryActionPerformed -= OnSecondaryActionPerformed;

            inspectedEntityList = new EntityList();
            inspectedEntityIndex = -1;

            if (events.onInspectedEntityListUpdated != null)
            {
                events.onInspectedEntityListUpdated(inspectedEntityList);
            }

            if (events.onCurrentSelectedEntityUpdated != null)
            {
                events.onCurrentSelectedEntityUpdated(inspectedEntity);
            }
        }

        public override void OnSelectionStart(SelectionBox selectionBox)
        {
            if (appState.UI.currentSelectedCellEntityList != inspectedEntityList)
            {
                inspectedEntityIndex = -1;
            }

            inspectedEntityList = appState.UI.currentSelectedCellEntityList;

            if (inspectedEntityList.Count > 0)
            {
                // Start at the top and work down as the user clicks
                if (inspectedEntityIndex > 0)
                {
                    // move down the list
                    inspectedEntityIndex = inspectedEntityIndex - 1;
                }
                else
                {
                    // back up to the top
                    inspectedEntityIndex = inspectedEntityList.Count - 1;
                }
            }
            else
            {
                if (inspectedEntityIndex != -1)
                {
                    inspectedEntityIndex = -1;
                }
            }

            if (events.onInspectedEntityListUpdated != null)
            {
                events.onInspectedEntityListUpdated(inspectedEntityList);
            }

            if (events.onCurrentSelectedEntityUpdated != null)
            {
                events.onCurrentSelectedEntityUpdated(inspectedEntity);
            }
        }

        /* 
            Event handlers
         */
        void OnFurnituresRemoved(FurnitureList furnitureList)
        {
            bool shouldReset = false;

            foreach (Furniture furniture in furnitureList.items)
            {
                if (inspectedEntityList.Contains(furniture))
                {
                    shouldReset = true;
                    inspectedEntityList.Remove(furniture);
                }
            }

            if (shouldReset)
            {
                // back up to the top
                inspectedEntityIndex = inspectedEntityList.Count - 1;

                if (events.onCurrentSelectedEntityUpdated != null)
                {
                    events.onCurrentSelectedEntityUpdated(inspectedEntity);
                }
            }
        }

        void OnResidentsRemoved(ResidentsList residentsList)
        {
            bool shouldReset = false;

            foreach (Resident resident in residentsList.items)
            {
                if (inspectedEntityList.Contains(resident))
                {
                    shouldReset = true;
                    inspectedEntityList.Remove(resident);
                }
            }

            if (shouldReset)
            {
                // back up to the top
                inspectedEntityIndex = inspectedEntityList.Count - 1;

                if (events.onCurrentSelectedEntityUpdated != null)
                {
                    events.onCurrentSelectedEntityUpdated(inspectedEntity);
                }
            }
        }

        void OnSecondaryActionPerformed()
        {
            if (!(inspectedEntity is ResidentEntity)) return;

            Resident resident = (inspectedEntity as ResidentEntity).resident;
            CellCoordinates targetCellCoordinates = appState.UI.currentSelectedCell;
            Furniture furnitureAtTarget = appState.Furnitures.queries.FindFurnitureAtCell(targetCellCoordinates);

            if (furnitureAtTarget != null)
            {
                appState.ResidentBehaviors.SendResidentTo(resident, furnitureAtTarget);
            }
            else
            {
                appState.ResidentBehaviors.SendResidentTo(resident, targetCellCoordinates);
            }
        }
    }
}