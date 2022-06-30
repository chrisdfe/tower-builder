using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.State;

using TowerBuilder.State.Rooms;
using TowerBuilder.State.Rooms.Connections;
using UnityEngine;

namespace TowerBuilder.State.Routes
{
    public class RouteFinderError
    {
        public string message { get; private set; } = "";

        public RouteFinderError(string message)
        {
            this.message = message;
        }
    }
}