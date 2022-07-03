using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
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