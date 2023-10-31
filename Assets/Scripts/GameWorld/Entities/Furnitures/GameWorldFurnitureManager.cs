using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Furnitures
{
    public class GameWorldFurnitureManager : EntityTypeManagerBase, IFindable
    {
        /* 
            Static Interface
        */
        public static GameWorldFurnitureManager Find() =>
            GameWorldFindableCache.Find<GameWorldFurnitureManager>("FurnitureManager");
    }
}