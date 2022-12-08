using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    // Cell position in relation to another cell
    [Flags]
    public enum CellOrientation
    {
        None = 0,
        Above = 1,
        AboveRight = 2,
        Right = 4,
        BelowRight = 8,
        Below = 16,
        BelowLeft = 32,
        Left = 64,
        AboveLeft = 128
    }
}


