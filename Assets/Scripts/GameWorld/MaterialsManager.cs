using UnityEngine;

namespace TowerBuilder.GameWorld
{
    public class MaterialsManager : MonoBehaviour
    {
        public enum MaterialKey
        {
            Primary,
            Secondary,
            Glass,
        }

        public AssetList<MaterialKey> materialList = new AssetList<MaterialKey>();
    }
}