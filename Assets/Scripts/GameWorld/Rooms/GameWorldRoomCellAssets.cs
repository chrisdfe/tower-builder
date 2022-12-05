using System.Collections.Generic;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameWorldRoomCellAssetsScriptableObject", order = 1)]
    public class GameWorldRoomCellAssetsScriptableObject : ScriptableObject
    {
        public enum ModelKey
        {
            Default,
            Wheels
        }

        public AssetList<ModelKey> assetList = new AssetList<ModelKey>();

        public enum ColorKey
        {
            Base,
            Hover,
            Inspected,
            Destroy,
            ValidBlueprint,
            InvalidBlueprint
        }

        public static Dictionary<ColorKey, Color> ColorMap = new Dictionary<ColorKey, Color>() {
            { ColorKey.Base, Color.grey },
            { ColorKey.Hover, Color.green },
            { ColorKey.Inspected, Color.cyan },
            { ColorKey.Destroy, Color.red },
            { ColorKey.ValidBlueprint, Color.blue },
            { ColorKey.InvalidBlueprint, Color.red },
        };

        public class SkinConfig
        {
            public bool hasInteriorLights = false;
        }

        public static Dictionary<RoomSkinKey, SkinConfig> SkinConfigMap = new Dictionary<RoomSkinKey, SkinConfig>() {
            {
                RoomSkinKey.Default,
                new SkinConfig() {
                    hasInteriorLights = true,
                }
            },
            {
                RoomSkinKey.Wheels,
                new SkinConfig() {
                    hasInteriorLights = false,
                }
            }
        };
    }
}