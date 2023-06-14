using TowerBuilder.DataTypes.Entities.Floors;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Floors
{
    public class GameWorldFloorsManager : EntityTypeManager, IFindable
    {
        /* 
            Static Interface
        */
        public static GameWorldFloorsManager Find() =>
            GameWorldFindableCache.Find<GameWorldFloorsManager>("FloorsManager");
    }
}
