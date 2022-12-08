using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    // Cell position in relation to another cell
    // TODO - put this in CellCoordinates?
    public enum CellOrientation
    {
        None,
        Above,
        AboveRight,
        Right,
        BelowRight,
        Below,
        BelowLeft,
        Left,
        AboveLeft
    }
}


