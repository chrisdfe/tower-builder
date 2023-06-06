using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    public class Diff<T>
    {
        public List<T> additions;
        public List<T> deletions;
    }
}


