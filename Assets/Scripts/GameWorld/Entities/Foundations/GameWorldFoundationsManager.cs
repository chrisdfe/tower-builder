using TowerBuilder.DataTypes.Entities.Foundations;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Foundations
{
    public class GameWorldFoundationsManager : EntityTypeManager, IFindable
    {
        /* 
            Static API
        */
        public static GameWorldFoundationsManager Find() =>
            GameWorldFindableCache.Find<GameWorldFoundationsManager>("FoundationsManager");
    }
}
