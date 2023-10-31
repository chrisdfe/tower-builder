using TowerBuilder.DataTypes.Entities.Floors;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Floors
{
    public class GameWorldFloorsManager : EntityTypeManagerBase, IFindable
    {
        /* 
            Static Interface
        */
        public static GameWorldFloorsManager Find() =>
            GameWorldFindableCache.Find<GameWorldFloorsManager>("FloorsManager");
    }
}
