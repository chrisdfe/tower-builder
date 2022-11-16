using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.DataTypes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Routes
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