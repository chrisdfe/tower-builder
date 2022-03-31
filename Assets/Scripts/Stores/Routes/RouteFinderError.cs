using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.Rooms.Connections;
using UnityEngine;

namespace TowerBuilder.Stores.Routes
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