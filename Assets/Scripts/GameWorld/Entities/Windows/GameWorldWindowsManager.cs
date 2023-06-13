using TowerBuilder.DataTypes.Entities.Windows;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Windows
{
    public class GameWorldWindowsManager : EntityTypeManager, IFindable
    {
        /* 
            Static API
        */
        public static GameWorldWindowsManager Find() =>
            GameWorldFindableCache.Find<GameWorldWindowsManager>("WindowsManager");
    }
}
