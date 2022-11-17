using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public static class UIDGenerator
    {
        static Dictionary<string, int> resourceIdMap = new Dictionary<string, int>();

        public static int Generate(string resourceType)
        {
            if (resourceIdMap.ContainsKey(resourceType))
            {
                int currentId = resourceIdMap[resourceType];
                resourceIdMap[resourceType] = Interlocked.Increment(ref currentId);
            }
            else
            {
                resourceIdMap.Add(resourceType, 1);
            }

            return resourceIdMap[resourceType];
        }
    }
}