using System.Collections.Generic;
using TowerBuilder.ApplicationState.Entities.Rooms;
using TowerBuilder.ApplicationState.UI;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
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
            public delegate void InspectedEntityListEvent(ListWrapper<Entity> entityList);
            public InspectedEntityListEvent onInspectedEntityListUpdated;

            public delegate void CurrentSelectedEntityEvent(Entity entity);
            public CurrentSelectedEntityEvent onCurrentSelectedEntityUpdated;
        }

        public Events events;

        public ListWrapper<Entity> inspectedEntityList { get; private set; } = new ListWrapper<Entity>();
        public int inspectedEntityIndex { get; private set; } = -1;

        public Entity inspectedEntity
        {
            get
            {
                if (inspectedEntityIndex == -1)
                {
                    return null;
                }

                return inspectedEntityList.items[inspectedEntityIndex];
            }
        }

        public InspectToolState(AppState appState, State state, Input input) : base(appState, state)
        {
            events = new Events();
        }

        public override void Setup()
        {
            base.Setup();

            appState.Entities.Furnitures.events.onItemsRemoved += OnFurnituresRemoved;
            appState.Entities.Residents.events.onItemsRemoved += OnResidentsRemoved;

            appState.UI.events.onSecondaryActionPerformed += OnSecondaryActionPerformed;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.Furnitures.events.onItemsRemoved -= OnFurnituresRemoved;
            appState.Entities.Residents.events.onItemsRemoved -= OnResidentsRemoved;

            appState.UI.events.onSecondaryActionPerformed -= OnSecondaryActionPerformed;

            inspectedEntityList = new ListWrapper<Entity>();
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
        void OnFurnituresRemoved(ListWrapper<Furniture> furnitureList)
        {
            OnEntitiesRemoved<Furniture>(furnitureList);
        }

        void OnResidentsRemoved(ListWrapper<Resident> residentsList)
        {
            OnEntitiesRemoved<Resident>(residentsList);
        }

        void OnEntitiesRemoved<EntityType>(ListWrapper<EntityType> entityList)
            where EntityType : Entity
        {
            bool shouldReset = false;

            foreach (Entity entity in entityList.items)
            {
                if (inspectedEntityList.Contains(entity))
                {
                    shouldReset = true;
                    inspectedEntityList.Remove(entity);
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
            if (!(inspectedEntity is Resident)) return;

            Resident resident = (inspectedEntity as Resident);
            CellCoordinates targetCellCoordinates = appState.UI.currentSelectedCell;
            Furniture furnitureAtTarget = appState.Entities.Furnitures.queries.FindFurnitureAtCell(targetCellCoordinates);

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