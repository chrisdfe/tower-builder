using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;

using TowerBuilder.ApplicationState.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Routes
{
    public enum RouteSegmentType
    {
        WalkingAcrossRoom,
        UsingRoomConnection,
        UsingStairs,
        UsingElevator,
    }
}