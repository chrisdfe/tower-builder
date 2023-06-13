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

        public AssetList assetList = new AssetList();

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
            Registry.appState.Time.onTick += OnTick;
        }

        public void Teardown()
        {
            Registry.appState.Time.onTick -= OnTick;
        }

        void OnTick(TimeValue time)
        {
            ListWrapper<ResidentBehavior> residentBehaviorsList = Registry.appState.Behaviors.Residents.list;

            foreach (ResidentBehavior residentBehavior in residentBehaviorsList.items)
            {
                Resident resident = residentBehavior.resident;
                GameWorldEntity gameWorldEntity = gameWorldEntityList.FindByEntity(resident);
                GameWorldResident gameWorldResident = gameWorldEntity.GetComponent<GameWorldResident>();

                TimeValue nextTickTimeValue = Registry.appState.Time.nextTickTimeValue;

                CellCoordinates currentCellCoordinates = resident.absoluteCellCoordinatesList.bottomLeftCoordinates;
                CellCoordinates nextCellCoordinates = resident.absoluteCellCoordinatesList.bottomLeftCoordinates;

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
