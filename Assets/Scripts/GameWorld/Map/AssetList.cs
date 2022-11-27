using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.Rooms;
using TowerBuilder.GameWorld.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld
{
    [Serializable]
    public class AssetList<KeyType>
        where KeyType : struct
    {
        [Serializable]
        public class AssetWrapper<AssetKeyType>
        {
            public AssetKeyType key;
            public GameObject gameObject;
        }

        [SerializeField]
        public List<AssetWrapper<KeyType>> assetList = new List<AssetWrapper<KeyType>>();

        public GameObject FindByKey(KeyType key)
        {
            AssetWrapper<KeyType> assetWrapper = assetList.Find(wrapper => wrapper.key.Equals(key));

            if (assetWrapper != null)
            {
                return assetWrapper.gameObject;
            }

            return null;
        }
    }
}
