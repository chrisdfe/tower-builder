using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Residents
{
    public class GameWorldResidentsManager : EntityTypeManagerBase, IFindable
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
            // TODO - this all seems like something that should live in GameWorldResident not here
            foreach (Resident resident in Registry.appState.Entities.Residents.list.items)
            {
                GameWorldEntity gameWorldEntity = FindByEntity(resident);
                GameWorldResident gameWorldResident = gameWorldEntity.GetComponent<GameWorldResident>();

                TimeValue nextTickTimeValue = Registry.appState.Time.nextTickTimeValue;

                CellCoordinatesList cellCoordinatesList = resident.relativeCellCoordinatesList;
                CellCoordinates currentCellCoordinates = cellCoordinatesList.bottomLeftCoordinates;
                CellCoordinates nextCellCoordinates = cellCoordinatesList.bottomLeftCoordinates;

                if (resident.behavior.currentState == ResidentBehavior.StateKey.Traveling)
                {
                    var (current, next) = resident.behavior.routeProgress.currentAndNextCell;

                    currentCellCoordinates = current;
                    nextCellCoordinates = next;
                }

                gameWorldResident.currentAndNextPosition = new CurrentAndNext<(TimeValue, CellCoordinates)>(
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
