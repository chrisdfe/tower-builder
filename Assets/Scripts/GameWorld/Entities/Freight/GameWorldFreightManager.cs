using TowerBuilder.DataTypes.Entities.Freights;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Freight
{
    public class GameWorldFreightManager : EntityTypeManager, IFindable
    {
        /* 
            Static Interface
        */
        public static GameWorldFreightManager Find() =>
            GameWorldFindableCache.Find<GameWorldFreightManager>("FreightManager");
    }
}
