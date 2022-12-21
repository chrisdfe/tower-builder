using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Behaviors.Residents;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Residents
{
    [RequireComponent(typeof(GameWorldEntityList))]
    public class GameWorldResidentsManager : MonoBehaviour, IFindable
    {
        GameWorldEntityList gameWorldEntityList;

        public enum AssetKey
        {
            Resident
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        void Awake()
        {
            gameWorldEntityList = GetComponent<GameWorldEntityList>();
        }

        void Start()
        {
            Setup();
        }

        void OnDestroy()
        {

        }

        public void Setup()
        {
            Registry.appState.Time.events.onTick += OnTick;
        }

        public void Teardown()
        {
            Registry.appState.Time.events.onTick -= OnTick;
        }

        void OnTick(TimeValue time)
        {
            ListWrapper<ResidentBehavior> residentBehaviorsList = Registry.appState.ResidentBehaviors.list;

            foreach (ResidentBehavior residentBehavior in residentBehaviorsList.items)
            {
                Resident resident = residentBehavior.resident;
                GameWorldEntity gameWorldEntity = gameWorldEntityList.FindByEntity(resident);
                GameWorldResident gameWorldResident = gameWorldEntity.GetComponent<GameWorldResident>();

                TimeValue nextTickTimeValue = Registry.appState.Time.queries.nextTickTimeValue;

                CellCoordinates currentCellCoordinates = resident.cellCoordinatesList.bottomLeftCoordinates;
                CellCoordinates nextCellCoordinates = resident.cellCoordinatesList.bottomLeftCoordinates;

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
            Static API
        */
        public static GameWorldResidentsManager Find() =>
            GameWorldFindableCache.Find<GameWorldResidentsManager>("ResidentsManager");
    }
}
