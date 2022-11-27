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
    public class AssetList
    {
        [Serializable]
        public class AssetWrapper
        {
            public string key;
            public GameObject gameObject;
        }

        [SerializeField]
        public List<AssetWrapper> assetList = new List<AssetWrapper>();

        public GameObject FindByKey(string key)
        {
            AssetWrapper assetWrapper = assetList.Find(wrapper => wrapper.key == key);

            if (assetWrapper != null)
            {
                return assetWrapper.gameObject;
            }

            return null;
        }
    }
}
