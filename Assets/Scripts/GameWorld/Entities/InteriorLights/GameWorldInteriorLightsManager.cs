using TowerBuilder.DataTypes.Entities.InteriorLights;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.InteriorLights
{
    public class GameWorldInteriorLightsManager : EntityTypeManagerBase, IFindable
    {
        /* 
            Static Interface
        */
        public static GameWorldInteriorLightsManager Find() =>
            GameWorldFindableCache.Find<GameWorldInteriorLightsManager>("InteriorLightsManager");
    }
}
