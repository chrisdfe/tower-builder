using TowerBuilder.DataTypes.Entities.Windows;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.Windows
{
    public class GameWorldWindowsManager : EntityTypeManager, IFindable
    {
        /* 
            Static Interface
        */
        public static GameWorldWindowsManager Find() =>
            GameWorldFindableCache.Find<GameWorldWindowsManager>("WindowsManager");
    }
}
