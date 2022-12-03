using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.TransportationItems;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    public interface IFindable { }

    public static class GameWorldFindableCache
    {
        static Dictionary<string, IFindable> findableCache = new Dictionary<string, IFindable>();

        public static FindableType Find<FindableType>(string key)
            where FindableType : MonoBehaviour, IFindable
        {
            FindableType cached = null;

            if (findableCache.ContainsKey(key))
            {
                cached = findableCache[key] as FindableType;
            }

            if (cached == null)
            {
                FindableType found = GameObject.Find(key).GetComponent<FindableType>();

                if (found != null)
                {
                    findableCache[key] = found;
                    cached = found;
                }
            }

            return cached;
        }
    }
}