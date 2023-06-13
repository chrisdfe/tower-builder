using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Furnitures
{
    public class GameWorldFurnitureManager : EntityTypeManager, IFindable
    {
        /* 
            Static API
        */
        public static GameWorldFurnitureManager Find() =>
            GameWorldFindableCache.Find<GameWorldFurnitureManager>("FurnitureManager");
    }
}