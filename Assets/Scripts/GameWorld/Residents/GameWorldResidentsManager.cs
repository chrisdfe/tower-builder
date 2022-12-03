using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Behaviors;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Residents
{
    public class GameWorldResidentsManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Resident
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        public static GameWorldResidentsManager Find() =>
            GameWorldFindableCache.Find<GameWorldResidentsManager>("ResidentsManager");
    }
}
