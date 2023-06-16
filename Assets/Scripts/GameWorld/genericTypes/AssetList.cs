using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    [Serializable]
    public class SerializableKeyValueList<KeyType, ValueType>
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

        public ValueType ValueFromKey(KeyType key) =>
            list.Find(wrapper => wrapper.key.Equals(key))?.value;

        public KeyType KeyFromValue(ValueType value) =>
            list.Find(wrapper => wrapper.value.Equals(value)).key;

        public bool HasKey(KeyType key) => list.Find(wrapper => wrapper.key.Equals(key)) != null;
    }

    [Serializable]
    public class AssetList : SerializableKeyValueList<string, GameObject>
    { }

    [Serializable]
    public class MaterialsList : SerializableKeyValueList<string, Material>
    { }

    [Serializable]
    public class MeshAssetList : AssetList
    {
        public void ReplaceMaterials()
        {
            foreach (AssetList.ValueTypeWrapper wrapper in list)
            {
                GameObject gameObject = wrapper.value;
                MaterialsReplacer.ReplaceMaterials(gameObject.transform);
            }
        }
    }
}
