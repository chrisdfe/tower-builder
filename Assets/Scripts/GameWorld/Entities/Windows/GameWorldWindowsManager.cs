using TowerBuilder.DataTypes.Entities.Windows;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Windows
{
    public class GameWorldWindowsManager : EntityTypeManagerBase, IFindable
    {
        /* 
            Static Interface
        */
        public static GameWorldWindowsManager Find() =>
            GameWorldFindableCache.Find<GameWorldWindowsManager>("WindowsManager");
    }
}
