using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Routes
{
    public enum RouteSegmentType
    {
        WalkingAcrossRoom,
        UsingRoomConnection,
        UsingStairs,
        UsingElevator,
    }
}