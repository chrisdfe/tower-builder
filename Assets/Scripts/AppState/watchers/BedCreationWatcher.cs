using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using UnityEngine;

namespace TowerBuilder.ApplicationState
{
    public class BedCreationWatcher : WatcherBase
    {
        // Queue<Furniture> furnitureToMoveResidentsInFor = new Queue<Furniture>();

        public BedCreationWatcher(AppState appState) : base(appState) { }

        public override void Setup()
        {
            appState.Furnitures.events.onFurnituresAdded += OnFurnituresAdded;
            appState.Furnitures.events.onFurnituresBuilt += OnFurnituresBuilt;
        }

        public override void Teardown()
        {
            appState.Furnitures.events.onFurnituresAdded -= OnFurnituresAdded;
            appState.Furnitures.events.onFurnituresBuilt -= OnFurnituresBuilt;
        }

        void OnFurnituresAdded(FurnitureList furnitureList)
        {
            MoveNewResidentInIfThereIsRoom(furnitureList);
        }

        void OnFurnituresBuilt(FurnitureList furnitureList)
        {
            MoveNewResidentInIfThereIsRoom(furnitureList);
        }

        void MoveNewResidentInIfThereIsRoom(FurnitureList furnitureList)
        {
            furnitureList.items.ForEach(furniture => MoveNewResidentInIfThereIsRoom(furniture));
        }

        void MoveNewResidentInIfThereIsRoom(Furniture furniture)
        {
            if (furniture.isInBlueprintMode || furniture.homeSlotCount == 0) return;

            // TODO -
            // 1) check that the bedroom is accessible from the front door

            ResidentsList residentsLivingAtSlotList = appState.FurnitureHomeSlotOccupations.queries.GetResidentsLivingAt(furniture);

            if (residentsLivingAtSlotList.Count >= furniture.homeSlotCount) return;

            // Move a new resident in
            Resident resident = new Resident();

            // place it where the resident is for now
            resident.cellCoordinates = furniture.cellCoordinates;
            appState.Residents.AddResident(resident);

            // add ownership
            FurnitureHomeSlotOccupation homeSlotOccupation = new FurnitureHomeSlotOccupation()
            {
                resident = resident,
                furniture = furniture
            };
            appState.FurnitureHomeSlotOccupations.AddFurnitureHomeSlotOccupation(homeSlotOccupation);
        }
    }
}
