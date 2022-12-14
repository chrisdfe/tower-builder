using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Residents
{
    public class GameWorldResidentsManager : MonoBehaviour, IFindable
    {
        public enum AssetKey
        {
            Resident
        }

        public AssetList<AssetKey> assetList = new AssetList<AssetKey>();

        /* 
            Static API
        */
        public static GameWorldResidentsManager Find() =>
            GameWorldFindableCache.Find<GameWorldResidentsManager>("ResidentsManager");
    }
}
