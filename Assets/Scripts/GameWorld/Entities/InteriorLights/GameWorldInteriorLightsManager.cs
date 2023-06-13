using TowerBuilder.DataTypes.Entities.InteriorLights;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.InteriorLights
{
    public class GameWorldInteriorLightsManager : EntityTypeManager, IFindable
    {
        /* 
            Static API
        */
        public static GameWorldInteriorLightsManager Find() =>
            GameWorldFindableCache.Find<GameWorldInteriorLightsManager>("InteriorLightsManager");
    }
}
