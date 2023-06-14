using TowerBuilder.DataTypes.Entities.Wheels;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Wheels
{
    public class GameWorldWheelsManager : EntityTypeManager, IFindable
    {
        /* 
            Static Interface
        */
        public static GameWorldWheelsManager Find() =>
            GameWorldFindableCache.Find<GameWorldWheelsManager>("WheelsManager");
    }
}
