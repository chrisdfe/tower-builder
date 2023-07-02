using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Behaviors.Residents;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Residents
{
    public class GameWorldResidentsManager : EntityTypeManager, IFindable
    {
        public override void Setup()
        {
            base.Setup();
            Registry.appState.Time.onTick += OnTick;
        }

        public override void Teardown()
        {
            base.Teardown();
            Registry.appState.Time.onTick -= OnTick;
        }

        void OnTick(TimeValue time)
        {
            ListWrapper<ResidentBehavior> residentBehaviorsList = Registry.appState.Behaviors.Residents.list;

            foreach (ResidentBehavior residentBehavior in residentBehaviorsList.items)
            {
                Resident resident = residentBehavior.resident;
                GameWorldEntity gameWorldEntity = FindByEntity(resident);
                GameWorldResident gameWorldResident = gameWorldEntity.GetComponent<GameWorldResident>();

                TimeValue nextTickTimeValue = Registry.appState.Time.nextTickTimeValue;

                CellCoordinatesList residentAbsoluteCellCoordinatesList = Registry.appState.EntityGroups.GetAbsoluteCellCoordinatesList(resident);
                CellCoordinates currentCellCoordinates = residentAbsoluteCellCoordinatesList.bottomLeftCoordinates;
                CellCoordinates nextCellCoordinates = residentAbsoluteCellCoordinatesList.bottomLeftCoordinates;

                if (residentBehavior.currentState == ResidentBehavior.StateKey.Traveling)
                {
                    var (current, next) = residentBehavior.routeProgress.currentAndNextCell;

                    currentCellCoordinates = current;
                    nextCellCoordinates = next;
                }

                gameWorldResident.currentAndNextPosition = new CurrentAndNext<(TimeValue, DataTypes.CellCoordinates)>(
                    (time, currentCellCoordinates),
                    (nextTickTimeValue, nextCellCoordinates)
                );
            }
        }

        /* 
            Static Interface
        */
        public static GameWorldResidentsManager Find() =>
            GameWorldFindableCache.Find<GameWorldResidentsManager>("ResidentsManager");
    }
}
