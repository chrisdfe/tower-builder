using TowerBuilder.DataTypes.Entities.TransportationItems;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.TransportationItems
{
    public class GameWorldTransportationManager : EntityTypeManagerBase, IFindable
    {
        /*
            Statics
        */
        public static GameWorldTransportationManager Find() =>
            GameWorldFindableCache.Find<GameWorldTransportationManager>("TransportationManager");
    }
}