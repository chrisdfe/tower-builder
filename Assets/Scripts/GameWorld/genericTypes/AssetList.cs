using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    [Serializable]
    public class SerializableKeyValueList<KeyType, ValueType>
        where KeyType : struct
        where ValueType : class
    {
        [Serializable]
        public class ValueTypeWrapper
        {
            public KeyType key;
            public ValueType value;
        }

        [SerializeField]
        public List<ValueTypeWrapper> list = new List<ValueTypeWrapper>();

        public int Count { get => list.Count; }

        public ValueType FindByKey(KeyType key) =>
            list.Find(wrapper => wrapper.key.Equals(key))?.value;
    }

    [Serializable]
    public class AssetList<KeyType> : SerializableKeyValueList<KeyType, GameObject>
        where KeyType : struct
    { }

    [Serializable]
    public class MaterialsList<KeyType> : SerializableKeyValueList<KeyType, Material>
        where KeyType : struct
    { }

    [Serializable]
    public class MeshAssetList<KeyType> : AssetList<KeyType>
        where KeyType : struct
    {
        public void ReplaceMaterials()
        {
            foreach (AssetList<KeyType>.ValueTypeWrapper wrapper in list)
            {
                GameObject gameObject = wrapper.value;
                MaterialsReplacer.ReplaceMaterials(gameObject.transform);
            }
        }
    }
}
