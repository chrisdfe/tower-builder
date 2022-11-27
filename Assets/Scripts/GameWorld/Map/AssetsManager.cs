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
    public class AssetsManager : MonoBehaviour
    {
        [SerializeField]
        public List<GameObject> assets = new List<GameObject>();

        [SerializeField]
        public List<GameObject> transportationItemModels = new List<GameObject>();

        public GameObject FindByName(string key)
        {
            return assets.Find(asset => asset.name == key);
        }

        public static AssetsManager Find()
        {
            return GameObject.Find("AssetsManager").GetComponent<AssetsManager>();
        }
    }
}
