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
        public List<ValueTypeWrapper> assetList = new List<ValueTypeWrapper>();

        public int Count { get => assetList.Count; }

        public ValueType FindByKey(KeyType key) =>
            assetList.Find(wrapper => wrapper.key.Equals(key))?.value;
    }

    [Serializable]
    public class AssetList<KeyType> : SerializableKeyValueList<KeyType, GameObject>
        where KeyType : struct
    { }

    [Serializable]
    public class MaterialsList<KeyType> : SerializableKeyValueList<KeyType, Material>
        where KeyType : struct
    { }
}
