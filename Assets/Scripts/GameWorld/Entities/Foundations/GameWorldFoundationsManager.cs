using TowerBuilder.DataTypes.Entities.Foundations;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Foundations
{
    public class GameWorldFoundationsManager : EntityTypeManagerBase, IFindable
    {
        /* 
            Static Interface
        */
        public static GameWorldFoundationsManager Find() =>
            GameWorldFindableCache.Find<GameWorldFoundationsManager>("FoundationsManager");
    }
}
